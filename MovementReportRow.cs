using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
namespace AverageCost;

public class MovementReportRow
{

[Name("Part Number")]
public string PartNumber {get; set;}
[Name("Description")]
public string Description {get; set;}
[Name("Date")]
public string Date {get; set;}
[Name("Type")]
public string MovementType {get; set;}
[Name("Part Purchase Order Type")]
public string PartPurchaseOrderType {get; set;}
[Name("Reference")]
public string Reference {get; set;}
[Name("Narrative")]
public string Narrative{get; set;}
[Name("User")]
public string User{get; set;}
[Name("Account")]
public string Account {get; set;}
[Name("Quantity")]
public decimal Quantity {get; set;}
[Name("Cost")]
public decimal Cost {get; set;}
[Name("Sale")]
public decimal Sale {get; set;}
[Name("Retail")]
public decimal Retail {get; set;}
[Name("Product Group")]
public string ProductGroup {get; set;}
[Name("Sales Discount Code")]
public string SalesDiscountCode {get; set;}
[Name("Purchase Discount Code")]
public string PurchaseDiscountCode {get; set;}
[Name("Manufacturer Bonus")]
public string ManufacturerBonus {get; set;}
[Name("Van Route")]
public string VanRoute {get; set;}
[Name("Van Run")]
public string VanRun {get; set;}
[Name("Type Of Supply")]
public string TypeOfSupply {get; set;}
[Name("FranchiseID")]
public string FranchiseID {get; set;}
[Name("Category")]
public string Category {get; set;}
[Name("Supplier Order Number")]
public string SupplierOrderNumber {get; set;}
[Name("Ship To")]
public string ShipTo {get; set;}
[Name("Average Cost")]
public decimal AverageCost{get; set;}
[Name("Retail Price")]
public decimal RetailPrice{get; set;} 
[Name("Cost Price")]
public decimal CostPrice{get; set;}
[Name("Stock Holding")]
public decimal StockHolding{get; set;}
[Name("Last Issue Date")]
public string LastIssueDate{get; set;}
[Name("Last Receipt Date")]
public string LastReceiptDate{get; set;}
[Name("New Average Cost")]
public decimal NewAverageCost{get; set;}
[Name("AveragaCostIsDifferent")]
public bool AveragaCostIsDifferent{get; set;}
}

public sealed class MovementReportRowMap : ClassMap<MovementReportRow>
{
    public MovementReportRowMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.NewAverageCost).Ignore();
        Map(m => m.AveragaCostIsDifferent).Ignore();
    }
}