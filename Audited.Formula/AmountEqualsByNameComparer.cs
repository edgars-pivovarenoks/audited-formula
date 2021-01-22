using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Audited.Formula
{
	public class AmountEqualsByNameComparer : IEqualityComparer<Amount>
	{
		public bool Equals([AllowNull] Amount x, [AllowNull] Amount y) => ((AmountName)x).Name.Equals(((AmountName)y).Name);

        public int GetHashCode([DisallowNull] Amount obj) => ((AmountName)obj).Name.GetHashCode();

		public static AmountEqualsByNameComparer Instance => new AmountEqualsByNameComparer();
	}
}
