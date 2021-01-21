using System;
using System.Collections.Generic;

namespace Audited.Formula
{
	public abstract class Amount
    {
        public virtual decimal Value { get => throw new InvalidOperationException($"{nameof(Value)} must me implemented."); }
        public virtual IList<Amount> AuditLog { get => throw new InvalidOperationException($"{nameof(AuditLog)} must me implemented."); private set { } }
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
        public override bool Equals(object obj) => Value.Equals(((Amount)obj).Value);
	}
}