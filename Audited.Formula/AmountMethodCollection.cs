using System;

namespace Audited.Formula
{
    public abstract class AmountMethodCollection {

        public Amount Call(Func<decimal, decimal> func, Amount p1) => new FixedAmount(func(p1.Value), p1.AuditLog);

        public Amount Call(Func<decimal,decimal,decimal> func, Amount p1, Amount p2) => new FixedAmount(func(p1.Value, p2.Value), p1, p2);
    }

}
