# audited-formula
Strategy pattern inspired tool to compose formulas and log intermediate calculations.

## Problem 
```csharp
    using System;

    public class Program
    {
      public static void Main()
      {
        int monthsInYear = 12;
        decimal purchasePrice = 5600.00m;
        decimal yearsOfOwnership = 5m;
        decimal fuelCostMonthly = 123.45m;
        decimal mainteneceCostYearly = 433.00m;
        decimal insuranceCostYearly = 223.00m;
        decimal loanPaymentMonthly = 105.00m;
        decimal taxCostsYearly = 116.00m;		

        decimal fuelCostsYearly = fuelCostMonthly * monthsInYear;
        decimal loanInterestYearly = loanPaymentMonthly * monthsInYear - purchasePrice / yearsOfOwnership;
        decimal roundedInsuranceCostYearly =  Math.Ceiling(insuranceCostYearly / 1000) * 1000;;
        decimal inventedTaxYearly = Math.Max(0m, taxCostsYearly - loanInterestYearly);
        decimal otherCosts = fuelCostsYearly + loanInterestYearly + mainteneceCostYearly + roundedInsuranceCostYearly + inventedTaxYearly;
        decimal total = purchasePrice + otherCosts * yearsOfOwnership;

        Console.WriteLine($"Result : {total}");
      }
    }
    
    // Result : 20872.00
```

## Proposal 
```csharp
    using Audited.Formula;
    using System;
    
    public class CarTotalCostOfOwnershipFormula : AuditedFormula
    {
      Amount MonthsInYear = Amount.Of(12);

      public Amount
        YearsOfOwnership,
        PurchasePrice,
        FuelCostMonthly,
        LoanPaymentMonthly,
        MainteneceCostYearly,
        InsuranceCostYearly,
        TaxCostsYearly;

      protected override Amount Total => Is(() => PurchasePrice + OtherCosts * YearsOfOwnership);

      Amount OtherCosts => Is(() => FuelCostsYearly 
            + LoanInterestYearly 
            + MaintenanceCostYearly 
            + RoundedInsuranceCostYearly 
            + InventedTaxYearly);

      Amount FuelCostsYearly => Is(() => FuelCostMonthly * MonthsInYear);

      Amount LoanInterestYearly => Is(() => LoanPaymentMonthly * MonthsInYear - PurchasePrice / YearsOfOwnership);

      Amount RoundedInsuranceCostYearly => Is(() => Math.CeilingThousands(InsuranceCostYearly));

      Amount InventedTaxYearly => Is(() => Math.Max(Amount.Zero, TaxCostsYearly - LoanInterestYearly));
    }   

    public class Program
    {
      public static void Main() {
        Formula carTcoFormula = new CarTotalCostOfOwnershipFormula {
          PurchasePrice = Amount.Of(5600.00),
          YearsOfOwnership = Amount.Of(5),
          FuelCostMonthly = Amount.Of(123.45),
          MaintenanceCostYearly = Amount.Of(433.00),			
          InsuranceCostYearly = Amount.Of(223.00),
          LoanPaymentMonthly = Amount.Of(105.00),
          TaxCostsYearly = Amount.Of(116.00)
        };		

        Amount carTco = carTcoFormula.Calculate();

	WriteResult(carTco);
	WriteAuditLog(carTco.AuditLog);
      }
      
      static void WriteAuditLog(IList<Amount> auditLog) => auditLog.Select(l => l.Equation).ToList().ForEach(Console.WriteLine);
      
      static void WriteResult(Amount result) => Console.WriteLine($"Result : {result.Value}");      
    }
```    
    
### Calculation log
- Purchase Price[5600.00] = Input value
- Fuel Cost Monthly[123.45] = Input value
- Months In Year[12.00] = Input value
- Fuel Costs Yearly[1481.40] = Fuel Cost Monthly[123.45] * Months In Year[12.00]
- Loan Payment Monthly[105.00] = Input value
- Years Of Ownership[5.00] = Input value
- Loan Interest Yearly[140.00] = (Loan Payment Monthly[105.00] * Months In Year[12.00]) - (Purchase Price[5600.00] / Years Of Ownership[5.00])
- Maintenance Cost Yearly[433.00] = Input value
- Insurance Cost Yearly[223.00] = Input value
- Rounded Insurance Cost Yearly[1000.00] = CeilingThousands(Insurance Cost Yearly[223.00]
- Tax Costs Yearly[116.00] = Input value
- Invented Tax Yearly[0.00] = Max(Amount.Zero, (Tax Costs Yearly[116.00] - Loan Interest Yearly[140.00])
- Other Costs[3054.40] = (((Fuel Costs Yearly[1481.40] + Loan Interest Yearly[140.00]) + Maintenance Cost Yearly[433.00]) + Rounded Insurance Cost Yearly[1000.00]) + Invented Tax Yearly[0.00]
- Total[20872.00] = Purchase Price[5600.00] + (Other Costs[3054.40] * Years Of Ownership[5.00])
