using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace artfulplace.Linear.RegexExtension
{
    /// <summary>
    /// Regexで複合条件を扱うための拡張クラス
    /// </summary>
    public static class MultiRegex
    {
        public enum MergeOperater
        {
            /// <summary>
            /// 2つのパターンにマッチするかを判定します
            /// </summary>
            And,
            /// <summary>
            /// 1つ目のパターンにマッチし2つ目のパターンにマッチしないかどうかを判定します
            /// </summary>
            Not,
            /// <summary>
            /// 1つ目のみ、または2つ目のみのパターンにマッチするかどうか判定します
            /// </summary>
            Xor
        }

        /// <summary>
        /// 二つのパターンを用いた正規表現の判定を行います
        /// </summary>
        /// <param name="target"></param>
        /// <param name="input"></param>
        /// <param name="pattern1"></param>
        /// <param name="pattern2"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool IsMatchDouble(string input, string pattern1, string pattern2,MergeOperater op)
        {
            switch (op)
            {
                case MergeOperater.And:
                    return Regex.IsMatch(input, pattern1) && Regex.IsMatch(input, pattern2);
                case MergeOperater.Not:
                    return Regex.IsMatch(input, pattern1) && !(Regex.IsMatch(input, pattern2));
                case MergeOperater.Xor:
                    var res1 = Regex.IsMatch(input, pattern1);
                    var res2 = Regex.IsMatch(input, pattern2);
                    return (res1 && !(res2)) || (!(res1) && (res2));
            }
            return false;
        }

        public static bool IsMatchDouble(string input, string pattern1, string pattern2, MergeOperater op, RegexOptions option)
        {
            switch (op)
            {
                case MergeOperater.And:
                    return Regex.IsMatch(input, pattern1, option) && Regex.IsMatch(input, pattern2, option);
                case MergeOperater.Not:
                    return Regex.IsMatch(input, pattern1, option) && !(Regex.IsMatch(input, pattern2, option));
                case MergeOperater.Xor:
                    var res1 = Regex.IsMatch(input, pattern1, option);
                    var res2 = Regex.IsMatch(input, pattern2, option);
                    return (res1 && !(res2)) || (!(res1) && (res2));
            }
            return false;
        }

        public static bool IsMatchMany(string input, params string[] patterns)
        {
            return patterns.All(x => Regex.IsMatch(input, x));
        }

        public static bool IsMatchMany(string input,RegexOptions option, params string[] patterns)
        {
            return patterns.All(x => Regex.IsMatch(input, x,option));
        }
    }
}
