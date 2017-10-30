using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Analysis
{
    public class Automaton
    {
        private readonly string _text;

        private static readonly string[] states = new string[] {"k","p","r","w","b2","t","f","z","fq","pa","p","b1"};


        public Automaton(string text) {
            _text = text;
        }

        public bool GetStartState() {
            return true;
        }
    }

}
