using System;

namespace Audited.Formula
{
    internal interface AmountMetadata: AmountName
    {
        AmountMetadata SetName(string name);
        AmountMetadata SetEquationAsText(Func<string> getEquationAsText);
        string ToSentenceCaseWithValue();
    }

    internal interface AmountName
    {
        string Name { get; }
    }
}