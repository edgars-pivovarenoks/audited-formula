using System;

namespace Audited.Formula
{
    internal interface AmountMetadata {
        string Name { get; }
        AmountMetadata SetName(string name);
        AmountMetadata SetEquationAsText(Func<string> getEquationAsText);
        string ToSentenceCaseWithValue();
    }
}