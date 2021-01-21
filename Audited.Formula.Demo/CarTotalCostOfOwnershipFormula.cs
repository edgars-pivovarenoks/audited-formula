using Audited.Formula;

public class CarTotalCostOfOwnershipFormula : AuditedFormula
{
	Amount MonthsInYear = Amount.Of(12);

	public Amount
		YearsOfOwnership,
		PurchasePrice,
		FuelCostMonthly,
		LoanPaymentMonthly,
		MaintenanceCostYearly,
		InsuranceCostYearly,
		TaxCostsYearly;

	protected override Amount Total => Is(() => PurchasePrice + OtherCosts * YearsOfOwnership);

	Amount OtherCosts => Is(() => FuelCostsYearly + LoanInterestYearly + MaintenanceCostYearly + RoundedInsuranceCostYearly + InventedTaxYearly + CarTiresCostYearly);

	Amount FuelCostsYearly => Is(() => FuelCostMonthly * MonthsInYear);

	Amount LoanInterestYearly => Is(() => LoanPaymentMonthly * MonthsInYear - PurchasePrice / YearsOfOwnership);

	Amount RoundedInsuranceCostYearly => Is(() => Math.CeilingThousands(InsuranceCostYearly));

	Amount InventedTaxYearly => Is(() => Math.Max(Amount.Zero, TaxCostsYearly - LoanInterestYearly));

	Amount CarTiresCostYearly => new CarTiresCostYearlyFormula {
		FuelCostsYearly = FuelCostsYearly
	};

}

public class CarTiresCostYearlyFormula : AuditedFormula
{
	Amount TireUsageCoeficient = Amount.Of(1.5);

	public Amount FuelCostsYearly;

	protected override Amount Total => Is(() => TireUsageCoeficient * FuelCostsYearly);
}