﻿using System;
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

        public override decimal Value => GetResult().Value;

        public string Name { get; private set; }

        public override List<Amount> AuditLog => GetResult().AuditLog;

        public override string Equation => GetResult().Equation;

        public Calculation(string name, Expression<Func<Amount>> expression, AuditedFormula parentFormula, Dictionary<string, Amount> equationResultsCache)
        {
            _parentFormula = parentFormula;
            Name = name;
            _calculationExpression = expression;
            _calculationResultsCache = equationResultsCache;
        }

        public Amount GetResult()
        {
            if (!_calculationResultsCache.TryGetValue(Name, out Amount result))
            {
                Func<Amount> calculationMethod = _calculationExpression.Compile();
                Amount calculationResult = calculationMethod();

                if (calculationResult is FixedAmount)
                    result = calculationResult;
                else if (calculationResult is Calculation)
                    result = new FixedAmount(calculationResult.Value, calculationResult.AuditLog);

                string cleanExpression = ComposeExpressionAsText();
                string expressionWithValues = UpdateExpressionWithValues(cleanExpression, result);
                ((AmountMetadata)result).SetName(Name).SetEquationAsText(() => expressionWithValues);
                _calculationResultsCache.Add(Name, result);
            }
            return result;
        }

        public override string ToString() => ((AmountMetadata)GetResult()).ToSentenceCaseWithValue();

        private string UpdateExpressionWithValues(string expression, Amount calculationResult)
        {
            string expressionWithValues = expression;

            var amountsWithValues = _calculationResultsCache.Values.Union(calculationResult.AuditLog, AmountEqualsByNameComparer.Instance);

            foreach (AmountMetadata amount in amountsWithValues)
                expressionWithValues = UpdateWithValue(expressionWithValues, amount);

            return expressionWithValues;

            string UpdateWithValue(string oldValue, AmountMetadata amount)
            {
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
                .Replace("()", "")
                .Replace(" => IIF", "IF");
            sanitizedExpression = sanitizedExpression.Substring(0, sanitizedExpression.Length - 1);

            return sanitizedExpression;
        }

        AmountMetadata AmountMetadata.SetName(string name)
        {
            Name = name;
            return this;
        }

        AmountMetadata AmountMetadata.SetEquationAsText(Func<string> _)
        {
            return this;
        }

        string AmountMetadata.ToSentenceCaseWithValue() => $"{NameUtils.ToSentence(Name)}[{Value:0.00}]";
        // string AmountMetadata.ToSentenceCaseWithValue() => ((AmountMetadata)GetResult()).ToSentenceCaseWithValue();

        //public bool Equals([AllowNull] Amount other)
        //{
        //	throw new NotImplementedException();
        //}

        //public static Amount operator *(Calculation a, Calculation b)
        //{
        //    Amount aResult = a.GetResult();
        //    Amount bResult = b.GetResult();

        //    return new FixedAmount(aResult.Value * bResult.Value, aResult, bResult);
        //}


    }
}