using Audited.Formula;

public class CarTotalCostOfOwnershipFormula : AuditedFormula<AmountMath>
{
	Amount MonthsInYear = Amount.Of(12);

	public Amount
		YearsOfOwnership,
		PurchasePrice,
		FuelCostMonthly,
		SomeFuelCheck,
		LoanPaymentMonthly,
		MaintenanceCostYearly,
		InsuranceCostYearly,
		TaxCostsYearly,
		OtherCostsLimit;

	protected override Amount Total => Is(() => PurchasePrice + OtherCostsLimited * YearsOfOwnership);

	Amount OtherCostsLimited => Is(() => OtherCosts > OtherCostsLimit ?  OtherCosts : OtherCostsLimit);

	Amount OtherCosts => Is(() => FuelCostsYearly + LoanInterestYearly + MaintenanceCostYearly + RoundedInsuranceCostYearly + InventedTaxYearly + CarTiresCostYearly);

	Amount FuelCostsYearly => Is(() => SomeFuelCheck == FuelCostMonthly ? Zero : FuelCostMonthly * MonthsInYear);

	Amount LoanInterestYearly => Is(() => LoanPaymentMonthly * MonthsInYear - PurchasePrice / YearsOfOwnership);

	Amount RoundedInsuranceCostYearly => Is(() => Math.CeilingThousands(InsuranceCostYearly));

	Amount InventedTaxYearly => Is(() => Math.Max(Zero, TaxCostsYearly - LoanInterestYearly));

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
