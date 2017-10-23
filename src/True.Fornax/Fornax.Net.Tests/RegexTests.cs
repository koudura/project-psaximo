using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fornax.Net.Tests
{
    [TestClass]
    internal class RegexTests
    {
        [TestMethod]
        public void Operations() {
            var rel_AND = new Regex(@"([\S]+ AND [\S]+)|([\S]+ NOT [\S]+)|([\S]+ OR [\S]+)", RegexOptions.Compiled); //all expressive boolean captures.

            var and_op = new Regex(@" &[\S]+"); //operator for direct capture
            
            var and_op_term = new Regex(@" &[\w]+"); // operator till term
            var and_op_prec = new Regex(@" &[(][^)]*[)]"); //& operator with precedence indicator

            var phrase = new Regex("[\"][\\s\\S]+[\"]", RegexOptions.Compiled);

            var range_proximity = new Regex(@"[\S]+ [%]\d+ [\S]+",RegexOptions.Compiled);
            var range_adjacency = new Regex(@"[\S]+ [$]\d+ [\S]+", RegexOptions.Compiled);

            string phrasable = "[\"][\\s\\S]+[\"]"; //phrase capture syntax.
            string precedable = "[(][^)]*[)]"; //precedence capture syntax.

            string range_default = @"";
            string range_preced = string.Format($"{precedable} [%]\\d+ {precedable}"); //proximity range with precedence => (*) %n (*);
            string range_phrase = string.Format($"{phrasable} [%]\\d+ {phrasable}"); // proximity range query with phrase => "*" %n "*";


            string sample1 = "find (humble OR proud) %2 (\"stupid\" &dumb)";
            /***
             * this is to be interpreted as...
             * 1. (humble OR proud) - get all documents with first occurence of precedence say D{h|p}
             * 2. ("stupid" &dumb) - get documents with "stupid" then get all documents with dumb - use [Boolean-AND] rule i.e.
             * ------say (D{s} = postings with "stupid" and  D{d} = postings with dumb) then intersect/merge D{s} with D{d} => D{sd}
             * 3. from D{h|p} and D{sd} use [Range-Proximty] rule i.e.
             * -------say (D{sdhp} = merge/intersection of list of D{h|p} and D{sd}, then for each d in D{sdhp} get positional index of each term, if
             * -------e.g D1 = humble makes a stupid man, D2 = proud people are dumb, D3 = stupid is the proud => D1[0,1,2,3,4], D2[0,1,2,3], D3[0,1,2,3]
             * -------if humble.pos - stupid.pos <2, ignore doc.
             * */
        }
    }
}
