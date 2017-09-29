using System;
using Fornax.Net.Util.Numerics;
using Fornax.Net.Analysis.Normalization;
using Fornax.Net.Common.Snowball.en;
using Fornax.Net.Common.Snowball.fr;
using Fornax.Net.Util.IO.Readers;

namespace Fornax.Net.Simulator
{
    class Program
    {
        static void Main(string[] args) {

            Console.WriteLine(Number.IsPrime(54));
            Console.WriteLine(Maths.Function.Asinh(0.5));

            string file = @"C:\Users\Kodex Zone\Source\Repos\project-psaximo\res\Box\en\en_stem.txt";
            //IDictionary<string,string> dictionary = WordReader.GetStemTable(new System.IO.FileInfo(file), true);
            //foreach (var item in dictionary.Keys) {
            //    Console.WriteLine(item);
            //}

            string word = "sifting";
            string word1 = "abaisserai";

            FornaxStemmer stemmer = new FornaxStemmer();
            Console.WriteLine(stemmer.StemWord(word1));
            PorterStemmer porter = new PorterStemmer();
            Console.WriteLine(porter.Stem(word1));

            FrenchStemmer french = new FrenchStemmer();
            Console.WriteLine(french.Stem(word1));

            BufferedReader reader = new BufferedReader(file);
            Console.WriteLine(reader.GetContent());
            
        }
    }
}
