using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AdventOfCode
{
    public class CephalopodMathsTest
    {
        private CephalopodMaths cephalopodMaths;

        [SetUp]
        public void Setup()
        {
            cephalopodMaths = new CephalopodMaths();
        }

        //[TestCase("Day6CephalapodMathsExample.txt", 4277556)]
        [TestCase("Day6CephalopodMaths.txt", 0)]
        public void LoadCephalopodMathsTest(string fileName, int expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", fileName);

            var result = cephalopodMaths.LoadCephalopodMathsProblem(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }
    
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
                
        }
    }
}