using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AdventOfCode
{
    public class MachineSwitchesTest
    {
        private MachineSwitches machineSwitches;

        [SetUp]
        public void Setup()
        {
            machineSwitches = new MachineSwitches();
        }

        [TestCase("Day10MachineSwitchesExample.txt", 7)]
        [TestCase("Day10MachineSwitches.txt", 527)]
        public void MachineSwitchesNeeds(string fileName, int expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", fileName);

            var result = machineSwitches.LoadAndAnalyseSwitches(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [TestCase("Day10MachineSwitchesExample.txt", 33)]
        //[TestCase("Day10MachineSwitches.txt", 0)]
        public void MachineSwitchesJoltageNeeds(string fileName, int expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", fileName);

            var result = machineSwitches.LoadAndAnalyseJoltageSwitches(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
            
        }
    }
}