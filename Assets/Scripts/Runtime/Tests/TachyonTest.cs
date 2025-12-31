using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AdventOfCode
{
    public class TachyonTest
    {
        private Tachyon tachyon;

        [SetUp]
        public void Setup()
        {
            tachyon = new Tachyon();
        }

        [TestCase("Day7TachyonExample.txt", 21)]
        [TestCase("Day7Tachyon.txt", 1672)]
        public void TachyonAnalysisSplitTest(string filename, int expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = tachyon.LoadAndAnalyseTachyonMatrixSplits(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [TestCase("Day7TachyonExample.txt", 40)]
        [TestCase("Day7Tachyon.txt", 231229866702355)]
        public void TachyonAnalysisWorldTest(string filename, long expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = tachyon.LoadAndAnalyseTachyonMatrixWorlds(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
                
        }
    }
}