using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Audited.Formula
{
	public abstract class Amount : IComparable<Amount>
	{
		public virtual decimal Value { get => throw new InvalidOperationException($"{nameof(Value)} must me implemented."); }
		public virtual List<Amount> AuditLog { get => throw new InvalidOperationException($"{nameof(AuditLog)} must me implemented."); private set { } }
		public virtual string Equation { get => throw new InvalidOperationException($"{nameof(Equation)} must me implemented."); }
		public static Amount operator +(Amount a) => a;
		public static Amount operator -(Amount a) => new FixedAmount(-a.Value, a.AuditLog);
		public static Amount operator +(Amount a, Amount b) => new FixedAmount(a.Value + b.Value, a, b);
		public static Amount operator -(Amount a, Amount b) => a + (-b);
		public static Amount operator *(Amount a, Amount b) => new FixedAmount(a.Value * b.Value, a, b);
		public static Amount operator /(Amount a, Amount b) => b.Value == 0 ? throw new DivideByZeroException() : new FixedAmount(a.Value / b.Value, a, b);
		public static Amount Zero => (FixedAmount)0m;
		public static Amount Of(int value) => (FixedAmount)value;
		public static Amount Of(decimal value) => (FixedAmount)value;
		public static Amount Of(double value) => (FixedAmount)Convert.ToDecimal(value);
		public static bool operator ==(Amount a, Amount b) => a.Equals(b);
		public static bool operator !=(Amount a, Amount b) => !a.Equals(b);
		public override bool Equals(object obj) => CompareWithLogsSharing(this, (Amount)obj, (a, b) => a.Equals(b));
		public int CompareTo([AllowNull] Amount other) => CompareWithLogsSharing(this, other, (a, b) => a.CompareTo(b));
		public static bool operator <(Amount a, Amount b) => CompareWithLogsSharing(a, b, (aVal, bVal) => aVal < bVal);
		public static bool operator >(Amount a, Amount b) => CompareWithLogsSharing(a, b, (aVal, bVal) => aVal > bVal);
		public static bool operator <=(Amount a, Amount b) => CompareWithLogsSharing(a, b, (aVal, bVal) => aVal <= bVal);
		public static bool operator >=(Amount a, Amount b) => CompareWithLogsSharing(a, b, (aVal, bVal) => aVal >= bVal);

		private static TReturn CompareWithLogsSharing<TReturn>(Amount a, Amount b, Func<decimal, decimal, TReturn> compareExpression) where TReturn : struct
        {
            var logsFromB = b.AuditLog.Except(a.AuditLog, AmountEqualsByNameComparer.Instance);
            var logsFromA = a.AuditLog.Except(b.AuditLog, AmountEqualsByNameComparer.Instance);

            a.AuditLog.AddRange(logsFromB);
            b.AuditLog.AddRange(logsFromA);

            return compareExpression(a.Value, b.Value);
		}
	}
}