using System;

namespace Audited.Formula
{
    public class AmountMath
    {
        // todo : wrap remaining Math functions

        public Amount Max(Amount a, Amount b) => new FixedAmount(Math.Max(a.Value, b.Value), a, b);

        public Amount CeilingThousands(Amount v) => new FixedAmount(Math.Ceiling(v.Value / 1000m) * 1000, v.AuditLog);

        public Amount Max(object zero, Amount amount)
        {
            throw new NotImplementedException();
        }
    }
}
