using System;
using Fornax.Net.Index.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fornax.Net.Tests.Analysis.Tools
{
    [TestClass]
    public class SoundexTests
    {
        [TestMethod]
        public void TGetSoundex() {
            var dex = SoundexFactory.GetSoundex("Ahmad");
            Console.WriteLine($"{dex.Word} = {dex.Value}");

            var dex1 = SoundexFactory.GetSoundex("Hahmad");
            Console.WriteLine($"{dex1.Word} = {dex1.Value}");

            var dex2 = SoundexFactory.GetSoundex("fills");
            Console.WriteLine($"{dex2.Word} = {dex2.Value}");

            var dex3 = SoundexFactory.GetSoundex("phillz");
            Console.WriteLine($"{dex3.Word} = {dex3.Value}");

            var vdef = SoundexFactory.Default;
            var acq = SoundexFactory.GetWords("touch", vdef);
        }
    }
}
