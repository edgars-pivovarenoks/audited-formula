using System;
using System.Collections.Generic;
using System.Linq;

namespace Audited.Formula
{
	internal class FixedAmount : Amount, AmountMetadata
    {
        // todo:implement nested level counter
        // todo:implement remaining math operators
        public override decimal Value { get; }

        private string equation;
        public override string Equation => equation;

        public string Name { get; private set; }

        public static implicit operator FixedAmount(decimal value) => new FixedAmount(value, "<Input value>");

        private List<Amount> auditLog = new List<Amount>();
        public override IList<Amount> AuditLog => auditLog;

        public override string ToString() => ((AmountMetadata)this).ToSentenceCaseWithValue();

        internal FixedAmount(decimal value, Amount a, Amount b) : this(value) => auditLog.AddRange(MergeLogs(a, b));

        internal FixedAmount(decimal value, IEnumerable<Amount> calculationsLog, string userFriendlyName = null) : this(value, userFriendlyName) => auditLog.AddRange(calculationsLog);

        internal FixedAmount(decimal value, string userFriendlyName = null)
        {            
            Value = value;
            Name = userFriendlyName;
        }

        private static IEnumerable<Amount> MergeLogs(Amount a, Amount b) => a.AuditLog.Union(b.AuditLog, new AmountEqualsByNameComparer());

        AmountMetadata AmountMetadata.SetName(string name)
        {
            Name = name;
            return this;
        }

        AmountMetadata AmountMetadata.SetEquationAsText(Func<string> getEquationAsText)
        {
            equation = $"{this} = {getEquationAsText()}";
            AuditLog.Add(this);
            return this;
        }

        string AmountMetadata.ToSentenceCaseWithValue() => $"{NameUtils.ToSentence(Name)}[{Value:0.00}]";
	}
}
