﻿using Fornax.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fornax.Net.Analysis
{
    public class Automaton
    {
        private readonly string _text;
        private QueryType _type;
        private Tuple<int, List<string>> _automata;

        private static readonly string[] states = new string[] { "k", "r", "w", "b2", "t", "p", "f", "z", "fq", "pa", "pp", "b1" };
        private static int n = states.Length;

        private readonly static string regex_wild = @"(^\s*\w\w+[*]\s*$)|(^\s*[*]\w\w+\s*$)|(^\s*\w\w+[*]\w\w+\s*$)";

        internal QueryType GetClass()
        {
            throw new NotImplementedException();
        }

        private readonly static string regex_phrase = "^[\"][\\s\\S]+[\"]$";
        private readonly static string regex_truncated = @"(^\s*\w\w+[?]\s*$)|(^\s*[?]\w\w+\s*$)|(^\s*\w\w+[?]\w\w+\s*$)";
        private readonly static string regex_boolType1 = @"(^[\S]+ AND [\S]+$)|(^[\S]+ NOT [\S]+$)|(^[\S]+ OR [\S]+$)";
        private readonly static string regex_boolType2 = @"(^[&][\S]+$)|(^[!][\S]+$)";
        private readonly static string regex_proximity = @"(^[\S]+ [$]\d+ [\S]+$)|(^[\S]+ [%]\d+ [\S]+$)";
        private readonly static string regex_frequency = @"(^[\S]+ [>]\d+$)";
        private readonly static string regex_precedence = @"^[(][^)]*[)]$";

        public Automaton(string text)
        {
            _text = string.Intern(text.Trim());
            AllocateState();
        }

        internal int GetStartState()
        {
            foreach (var ch in _text)
            {
                if (Char.IsWhiteSpace(ch))
                {
                    return 0;
                }
            }
            return 1;
        }

        internal void AllocateState()
        {
            List<string> accepts;
            if (GetStartState() == 0)
            {
                accepts = new List<string>() {
                    { states[5] }, { states[6] } ,{ states[7]},{ states[8]},{ states[9]},{ states[10]},{states[11] } }; //spacy queries
                _automata = new Tuple<int, List<string>>(0, accepts);
            }
            else
            {
                accepts = new List<string>() {
                     { states[0] }, { states[1] } ,{ states[2]},{ states[3]},{ states[4]} //spaceless queries
                };
                _automata = new Tuple<int, List<string>>(1, accepts);
            }
        }


        internal void Classify()
        {
            if (_automata.Item1 == 0)
            {
                var precision = IsPreceded(_text);
                if (!precision.Success)
                {
                   
                }
            }
        }

        private (bool Success, string Value, int Index) IsPreceded(string text)
        {
            var match = Regex.Match(text, regex_precedence, RegexOptions.Compiled);
            return (match.Success, match.Value, match.Index);
        }

        private bool IsWildcard(string _text)
        {
            return Regex.IsMatch(_text, regex_wild, RegexOptions.IgnorePatternWhitespace);
        }

        private bool IsBoolType1(string _text)
        {
            return Regex.IsMatch(_text, regex_boolType1, RegexOptions.IgnorePatternWhitespace);
        }

        private bool IsBoolType2(string _text)
        {
            return Regex.IsMatch(_text, regex_boolType2, RegexOptions.IgnorePatternWhitespace);
        }

        private bool IsPhrase(string _text)
        {
            return Regex.IsMatch(_text, regex_phrase, RegexOptions.IgnorePatternWhitespace);
        }

        private bool IsProximity(string _text)
        {
            return Regex.IsMatch(_text, regex_proximity, RegexOptions.IgnorePatternWhitespace);
        }
        private bool IsTrunc(string _text)
        {
            return Regex.IsMatch(_text, regex_truncated, RegexOptions.IgnorePatternWhitespace);
        }


        private (bool Success, string Value, int Index) IsWildCard(string text)
        {
            var match = Regex.Match(text, regex_wild, RegexOptions.Compiled);
            return (match.Success, match.Value, match.Index);
        }
    }

}
