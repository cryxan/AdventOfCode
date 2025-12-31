using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AdventOfCode
{
    public class IngredientsTest
    {
        private Ingredients ingredients;

        [SetUp]
        public void Setup()
        {
            ingredients = new Ingredients();
        }

        [TestCase("Day5IngredientsExampleBad.txt", -1)]
        [TestCase("Day5IngredientsExample.txt", 3)]
        [TestCase("Day5Ingredients.txt", 681)]
        public void TestLoadAndCheck(string filename, int expectedCount)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = ingredients.LoadAndCheckFile(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expectedCount));
        }
        
        [TestCase("Day5IngredientsExampleBad.txt", "-1")]
        [TestCase("Day5IngredientsExample.txt", "14")]
        [TestCase("Day5Ingredients.txt", "348820208020395")]
        public void LoadAndCheckFreshFile(string filename, string expectedCountStr)
        {
            var expectedCount = BigInteger.Parse(expectedCountStr);
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = ingredients.LoadAndCheckFreshFile(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expectedCount));
        }     
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
            
        }
    }
}