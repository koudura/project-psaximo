using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Util.Collections.Generic;

namespace Fornax.Net
{
    public class Configuration
    {
        public Configuration() {
         
        }




        public Configuration Reset() {
            return new Configuration();
        }

        public void Save() {
            ConfigFactory.SaveSettings();
        }

    }
}
