using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Audited.Formula
{
    internal class Calculation : Amount, AmountMetadata
    {
        private Dictionary<string, Amount> _calculationResultsCache;
        private Expression<Func<Amount>> _calculationExpression;
        private AuditedFormula _parentFormula; 
        private string _name;

        public decimal Value => GetResult().Value;

        public string Name => _name;

        public IList<Amount> AuditLog => GetResult().AuditLog;

        public string Equation => GetResult().Equation;

        public Calculation(string name, Expression<Func<Amount>> expression, AuditedFormula parentFormula, Dictionary<string, Amount> equationResultsCache)
        {
            _parentFormula = parentFormula;
            _name = name;
            _calculationExpression = expression;
            _calculationResultsCache = equationResultsCache;
        }

        public Amount GetResult() {
            if (!_calculationResultsCache.TryGetValue(_name, out Amount result)) {
                result = _calculationExpression.Compile()();
                string cleanExpression = ComposeExpressionAsText();
                string expressionWithValues = UpdateExpressionWithValues(cleanExpression, result);
                ((AmountMetadata)result).SetName(_name).SetEquationAsText(() => expressionWithValues);
                _calculationResultsCache.Add(_name, result);
            }
            return result;
        }

        public override string ToString() => ((AmountMetadata)GetResult()).ToSentenceCaseWithValue();

        private string UpdateExpressionWithValues(string expression, Amount calculationResult)
        {
            string expressionWithValues = expression;

            var amountsWithValues = _calculationResultsCache.Values.Union(calculationResult.AuditLog);

            foreach (AmountMetadata amount in amountsWithValues)
                expressionWithValues = UpdateWithValue(expressionWithValues, amount);

            return expressionWithValues;

            string UpdateWithValue(string oldValue, AmountMetadata amount) {
                var tokenLookup = new Regex(@$"\b{amount.Name}\b");
                return tokenLookup.Replace(oldValue, amount.ToSentenceCaseWithValue());
            }
        }

        private string ComposeExpressionAsText()
        {
            string calculationExpressionText = _calculationExpression.ToString();
            string formulaName = _parentFormula.GetType().Name;
            // todo : smarter sanitizer with regex-es for example
            string sanitizedExpression = calculationExpressionText
                .Replace($"() => value({formulaName}).{_parentFormula.GetMathMemberName()}.", "")
                .Replace("() => (", "")
                .Replace($"value({formulaName}).", "")
                .Replace("Convert(", "")
                .Replace(", Amount)", "")
                .Replace("()", "");
            sanitizedExpression = sanitizedExpression.Substring(0, sanitizedExpression.Length - 1);
            
            return sanitizedExpression;
        }

        AmountMetadata AmountMetadata.SetName(string name)
        {
            _name = name;
            return this;
        }

        AmountMetadata AmountMetadata.SetEquationAsText(Func<string> _)
        {
            return this;
        }

        string AmountMetadata.ToSentenceCaseWithValue() => ((AmountMetadata)GetResult()).ToSentenceCaseWithValue();

        //public static Amount operator *(Calculation a, Calculation b)
        //{
        //    Amount aResult = a.GetResult();
        //    Amount bResult = b.GetResult();

        //    return new FixedAmount(aResult.Value * bResult.Value, aResult, bResult);
        //}

    }
}