using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;

namespace Audited.Formula
{
    public interface Amount
    {
        decimal Value { get; }
        IList<Amount> AuditLog { get; }
        string Equation { get; }
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
    }    
}