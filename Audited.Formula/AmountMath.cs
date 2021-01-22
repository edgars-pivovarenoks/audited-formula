﻿using System;

namespace Audited.Formula
{
    public class AmountMath : AmountMethodCollection
    {
        // todo : wrap remaining Math functions

        // todo : typical financial math could go to separate helper

        public Amount Abs(Amount value) => Call(Math.Abs, value);

        public Amount Truncate(Amount value) => Call(Math.Truncate, value);

        public Amount Min(Amount val1, Amount val2) => Call(Math.Min, val1, val2);

        public Amount Max(Amount val1, Amount val2) => Call(Math.Max, val1, val2);

        public Amount Round(Amount d, int decimals) => Call(val => Math.Round(val, decimals), d);

        public Amount FloorThousands(Amount v) => Call(val => Math.Floor(val / 1000m) * 1000, v);

        public Amount FloorHundreds(Amount v) => Call(val => Math.Floor(val / 1000m) * 1000, v);

        public Amount CeilingHundreds(Amount v) => Call(val => Math.Ceiling(val / 1000m) * 1000, v);

        public Amount CeilingThousands(Amount v) => Call(val => Math.Ceiling(val / 1000m) * 1000, v);

        public Amount NotGreaterThanZero(Amount value) => Call(val => Math.Min(val, 0), value);

        public Amount NotLessThanZero(Amount value) => Call(val => Math.Max(0, val), value);
    }

    public abstract class AmountMethodCollection {

        public Amount Call(Func<decimal, decimal> func, Amount p1) => new FixedAmount(func(p1.Value), p1.AuditLog);

        public Amount Call(Func<decimal,decimal,decimal> func, Amount p1, Amount p2) => new FixedAmount(func(p1.Value, p2.Value), p1, p2);
    }

}
