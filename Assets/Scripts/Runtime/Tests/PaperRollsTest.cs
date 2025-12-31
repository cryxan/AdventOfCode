using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AdventOfCode
{
    public class PaperRollsTest
    {
        private PaperRolls paperRolls;
        
        [SetUp]
        public void Setup()
        {
            paperRolls = new PaperRolls();
        }
        
        [TestCase("Day4PaperRollsExample.txt", 13)]
        [TestCase("Day4PaperRollsExampleNoFile.txt", -2)] 
        [TestCase("Day4PaperRollsExampleBad.txt", -1)] 
        [TestCase("Day4PaperRolls.txt", 1384)]
        public void LoadPaperRolls(string fileName, int expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", fileName);
            
            var result = paperRolls.LoadPaperRollsAndAnalyseOnce(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("Day4PaperRollsExample.txt", 43)]
        [TestCase("Day4PaperRollsExampleNoFile.txt", -2)] 
        [TestCase("Day4PaperRollsExampleBad.txt", -1)] 
        [TestCase("Day4PaperRolls.txt", 8013)]
        public void LoadPaperRollsAndRepeat(string fileName, int expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", fileName);
            
            var result = paperRolls.LoadPaperRollsAndAnalyseAndRepeat(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
            
        }
    }
}