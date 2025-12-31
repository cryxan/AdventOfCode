using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AdventOfCode
{
    public class CarpetTest
    {
        private Carpet carpet;
        [SetUp]
        public void Setup()
        {
            carpet = new Carpet();
        }
        
        [TestCase("Day9CarpetExample.txt", 50)]
        [TestCase("Day9Carpet.txt", 4754955192)]

        public void CircuitsReturnExpectedLargestCarpet(string filename, long expectArea)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = carpet.LoadFileAndFindLargestCarpet(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expectArea));
        }

        [TestCase("Day9CarpetExample.txt", 24)]
        [TestCase("Day9Carpet.txt", 1568849600)]

        public void CircuitsReturnExpectedLargestCarpetInBoundry(string filename, long expectArea)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = carpet.LoadFileAndFindLargestCarpetInBoundary(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expectArea));
        }
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
        }
    }
}