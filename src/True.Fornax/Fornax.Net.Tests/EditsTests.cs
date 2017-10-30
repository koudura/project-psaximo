using System;
using Fornax.Net.Index.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fornax.Net.Tests
{
    [TestClass]
    public class EditsTests
    {
        [TestMethod]
        public void DefaultEdits() {

            var def = EditFactory.Default;
            var kvp = EditFactory.GetSimilars("anyway", def);

            foreach (var item in kvp) {
                Console.WriteLine("{0} : {1}",item.Key,item.Value);
            }
        }
    }
}
