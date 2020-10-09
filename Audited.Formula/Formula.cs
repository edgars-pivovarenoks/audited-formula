using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Audited.Formula
{
    public abstract class Formula
    {
        private Dictionary<string, Amount> _amountsCache;

        protected abstract Amount Total { get; }

        protected AmountMath Math { get; } = new AmountMath(); // todo : injectable

        internal string GetMathMemberName() => nameof(Math);

        public Amount Calculate()
        {
            _amountsCache = new Dictionary<string, Amount>();
            SetAmountNames(this);
            return Total;
        }

        private void SetAmountNames(Formula calculation)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            List<FieldInfo> amountFields = GetType().GetMembers(bindFlags)
                .Where(m => m.MemberType == MemberTypes.Field).Select(m => (FieldInfo)m)
                .Where(m => typeof(Amount).IsAssignableFrom(m.FieldType))
                .ToList();

            foreach (FieldInfo field in amountFields) {
                Amount amount = (Amount)field.GetValue(calculation);
                ((AmountMetadata)amount).SetName(field.Name).SetEquationAsText(() => "<Input value>");
                _amountsCache.Add(field.Name, amount);
            }
        }

        protected Amount Is(Expression<Func<Amount>> expression)
        {
            string equationName = null;
            StackFrame[] fr = new StackTrace().GetFrames();
            if (fr != null)
                equationName = fr[1].GetMethod().Name.Replace("get_", "");

            return new Calculation(equationName ?? "#call stack error#", expression, this, _amountsCache);
        }

    }
}