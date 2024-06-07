using AverageCost;
using CsvHelper;
using System.Globalization;

Console.WriteLine("Hello, World!");

string inputFile = @"C:\Users\Matt\OneDrive\Dev\AverageCost\PartMovementExport_20240606.csv";
string outputFile = @"C:\Users\Matt\OneDrive\Dev\AverageCost\UpdatedAverageCosts.csv";
List<MovementReportRow> outputRows = new();

using var reader = new StreamReader(inputFile);
using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

csv.Context.RegisterClassMap<MovementReportRowMap>();
var rows = csv.GetRecords<MovementReportRow>();


decimal lastGoodAvergeCost = 0;
int rowCounter = 1;
Dictionary<string, decimal> workshopIssues = new();
//Assumes first row is always good
foreach(var row in rows)
{
    if(rowCounter == 1)
    {
        lastGoodAvergeCost = row.AverageCost;
    }
    
    switch(row.MovementType)
    {
        case "Receipt":
            HandleReceiptRow(row);
            break;
        case "Workshop Issue":
            HandleWorkshopIssueRow(row, lastGoodAvergeCost);
            break;
        case "Invoice" :
            row.NewAverageCost = lastGoodAvergeCost;
            break;
        case "Stock Take" :
            row.NewAverageCost = lastGoodAvergeCost;
            break;
        case "Return" :
            row.NewAverageCost = lastGoodAvergeCost; 
            break;
        default:
            Console.WriteLine(row.MovementType);
            break;
    }
    outputRows.Add(row);
    rowCounter++;
}

using var writer = new StreamWriter(outputFile);
using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
csvWriter.WriteRecords(outputRows);

void HandleReceiptRow(MovementReportRow row)
{
    row.NewAverageCost = Math.Round(recalulateAverageCost(lastGoodAvergeCost, row.Quantity, row.CostPrice, row.StockHolding), 2);    
    if(row.NewAverageCost != row.AverageCost)
    {
        row.AveragaCostIsDifferent = true;
    }
    lastGoodAvergeCost = row.NewAverageCost;
}

void HandleWorkshopIssueRow(MovementReportRow row, decimal lastGoodAvergeCost)
{
    
    if(row.Quantity > 0)
    {
        if(DateTime.Compare(DateTime.Parse(row.Date), DateTime.Parse("2023-11-07")) < 0) 
        {
            if(!workshopIssues.TryAdd(row.Reference, row.AverageCost))
            {
                workshopIssues[row.Reference] = row.AverageCost;
            }

            row.NewAverageCost = row.AverageCost;
            row.AveragaCostIsDifferent = false;
        }
        else
        {
            if(!workshopIssues.TryAdd(row.Reference, lastGoodAvergeCost))
            {
                workshopIssues[row.Reference] = lastGoodAvergeCost;
            }

            row.NewAverageCost = lastGoodAvergeCost;
            if(row.NewAverageCost != row.AverageCost)
            {
                row.AveragaCostIsDifferent = true;
            }

        }
    }
    else
    {
        if(workshopIssues.TryGetValue(row.Reference, out decimal value))
        {
            row.NewAverageCost = Math.Round(recalulateAverageCost(value, row.Quantity, row.CostPrice, row.StockHolding), 2);
            if(row.NewAverageCost != row.AverageCost)
            {
                row.AveragaCostIsDifferent = true;
            }
        }
        else
        {
            row.NewAverageCost = Math.Round(recalulateAverageCost(lastGoodAvergeCost, row.Quantity, row.CostPrice, row.StockHolding), 2);
            if(row.NewAverageCost != row.AverageCost)
            {
                row.AveragaCostIsDifferent = true;
            }
        }

    }    



}

decimal recalulateAverageCost(decimal lastGoodAvergeCost, decimal quantity, decimal cost, decimal stockHolding)
{
    var previousStockHolding = stockHolding - quantity;
    var newAverageCost = ((lastGoodAvergeCost * previousStockHolding) + (quantity * cost)) / stockHolding;
    return newAverageCost;
}