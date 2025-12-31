using System.IO;
using System.Numerics;
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

        [TestCase("Day6CephalapodMathsExample.txt", "3263827")]
        [TestCase("Day6CephalopodMaths.txt", "11744693538946")]
        public void LoadCephalopodMathsTest(string fileName, string expectedStr)
        {
            var expected = BigInteger.Parse(expectedStr);
            
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