using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Audited.Formula
{
    internal class FixedAmount : Amount, AmountMetadata
    {
        private List<Amount> auditLog = new List<Amount>();

        // todo:implement nested level counter
        // todo:implement remaining math operators
        public decimal Value { get; }

        public string Name { get; private set; }

        public string Equation { get; private set; }

        public static implicit operator FixedAmount(decimal value) => new FixedAmount(value, "<Input value>");

        public IList<Amount> AuditLog { get => auditLog; }

        public override string ToString() => ((AmountMetadata)this).ToSentenceCaseWithValue();

        internal FixedAmount(decimal value, Amount a, Amount b) : this(value) => auditLog.AddRange(MergeLogs(a, b));

        internal FixedAmount(decimal value, IEnumerable<Amount> calculationsLog, string userFriendlyName = null) : this(value, userFriendlyName) => auditLog.AddRange(calculationsLog);

        internal FixedAmount(decimal value, string userFriendlyName = null)
        {
            auditLog = new List<Amount>();
            Value = value;
            Name = userFriendlyName;
        }

        private static IEnumerable<Amount> MergeLogs(Amount a, Amount b) => a.AuditLog.Union(b.AuditLog);

        AmountMetadata AmountMetadata.SetName(string name)
        {
            Name = name;
            return this;
        }

        AmountMetadata AmountMetadata.SetEquationAsText(Func<string> getEquationAsText)
        {
            Equation = $"{this} = {getEquationAsText()}";
            AuditLog.Add(this);
            return this;
        }

        string AmountMetadata.ToSentenceCaseWithValue() => $"{NameUtils.ToSentence(Name)}[{Value:0.00}]";
    }
}