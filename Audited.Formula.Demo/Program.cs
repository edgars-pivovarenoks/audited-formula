using Audited.Formula;
using System;
using System.Linq;

public class Program
{
	public static void Main() {
        Formula carTcoFormula = new CarTotalCostOfOwnershipFormula
        {
            PurchasePrice = Amount.Of(5600.00),
            YearsOfOwnership = Amount.Of(5),
            FuelCostMonthly = Amount.Of(123.45),
            SomeFuelCheck = Amount.Of(122.45),
            MaintenanceCostYearly = Amount.Of(433.00),
            InsuranceCostYearly = Amount.Of(223.00),
            LoanPaymentMonthly = Amount.Of(105.00),
            TaxCostsYearly = Amount.Of(116.00),
            OtherCostsLimit = Amount.Of(1800.00)
        };

        Amount result = carTcoFormula.Calculate();
		decimal resultValue = result.Value;

		WriteResult();
		WriteAuditLog();

		void WriteAuditLog() => result.AuditLog.Select(l => l.Equation).ToList().ForEach(Console.WriteLine);
		void WriteResult() => Console.WriteLine($"Result : {resultValue}");
	}	
}