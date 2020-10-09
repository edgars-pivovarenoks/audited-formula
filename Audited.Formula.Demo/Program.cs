using Audited.Formula;
using System;

public class Program
{
	public static void Main() {
		var carTCO = new CarTotalCostOfOwnershipFormula {
			PurchasePrice = Amount.Of(5600.00),
			YearsOfOwnership = Amount.Of(5),
			FuelCostMonthly = Amount.Of(123.45),
			MainteneceCostYearly = Amount.Of(433.00),			
			InsuranceCostYearly = Amount.Of(223.00),
			LoanPaymentMonthly = Amount.Of(105.00),
			TaxCostsYearly = Amount.Of(116.00)
		};		

		Amount tax = carTCO.Calculate();

		WriteAuditLog(tax);
	}
	
	private static void WriteAuditLog(Amount taxResult) {
		foreach (Amount calculation in taxResult.AuditLog)
			Console.WriteLine(calculation.Equation);
	}
}