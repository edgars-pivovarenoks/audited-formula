using System;

namespace Audited.Formula
{
	public abstract class Formula : Amount
    {
        public virtual Amount Calculate() => throw new InvalidOperationException("Calculate must me implemented.");
    }
}