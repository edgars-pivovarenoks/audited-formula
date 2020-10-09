using System;
using System.Text.RegularExpressions;

namespace Audited.Formula
{
    internal class NameUtils
    {
        public static string ToSentence(string str) => !String.IsNullOrWhiteSpace(str) ? UppercaseFirst(Regex.Replace(str, @"[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1])) : "TODO";

        private static string UppercaseFirst(string s) => char.ToUpper(s[0]) + s.Substring(1);
    }
}