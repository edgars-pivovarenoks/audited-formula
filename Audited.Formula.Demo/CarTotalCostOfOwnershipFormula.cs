using Audited.Formula;

public class CarTotalCostOfOwnershipFormula : Formula
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

	Amount OtherCosts => Is(() => FuelCostsYearly + LoanInterestYearly + MainteneceCostYearly + RoundedInsuranceCostYearly + InventedTaxYearly);

	Amount FuelCostsYearly => Is(() => FuelCostMonthly * MonthsInYear);

	Amount LoanInterestYearly => Is(() => LoanPaymentMonthly * MonthsInYear - PurchasePrice / YearsOfOwnership);

	Amount RoundedInsuranceCostYearly => Is(() => Math.CeilingThousands(InsuranceCostYearly));

	Amount InventedTaxYearly => Is(() => Math.Max(Amount.Zero, TaxCostsYearly - LoanInterestYearly));
}