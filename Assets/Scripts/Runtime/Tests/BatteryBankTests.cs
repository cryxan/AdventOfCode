using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AdventOfCode
{
    public class BatteryBankTests
    {
        private BatteryBank batteryBank;
        //"Day3BatteryValues.txt"

        [SetUp]
        public void Setup()
        {
            batteryBank = new BatteryBank();
        }


        [TestCase("9", true, 9)]
        [TestCase("98", true, 9, 8)]
        [TestCase("987654321111111", true,9,8,7,6,5,4,3,2,1,1,1,1,1,1,1)]
        [TestCase("811111111111119", true,8,1,1,1,1,1,1,1,1,1,1,1,1,1,9)]
        [TestCase("234234234234278", true,2,3,4,2,3,4,2,3,4,2,3,4,2,7,8)]
        [TestCase("818181911112111", true,8,1,8,1,8,1,9,1,1,1,1,2,1,1,1)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("1234a567", false)]

        public void JoltageStringConvertsToArray(string joltageString, bool success, params int[] joltageArray)
        {
            var result = batteryBank.JoltageStringToArray(joltageString);
            if (success)
            {
                Assert.That(result.Length, Is.EqualTo(joltageArray.Length));
                Assert.That(result, Is.EquivalentTo(joltageArray));
            }
            else
            {
                Assert.Null(result);
            }
        }


        [TestCase("98765432111", "0")]
        [TestCase("987654321111", "987654321111")]
        [TestCase("987654321111111", "987654321111")]
        [TestCase("811111111111119", "811111111119")]
        [TestCase("234234234234278", "434234234278")]
        [TestCase("818181911112111", "888911112111")]
        public void FindJoltageTest(string joltageString, string expectedJoltageString)
        {
            var expectedJoltage = BigInteger.Parse(expectedJoltageString);
            
            var result = batteryBank.FindJoltage(joltageString);
            
            Assert.That(result, Is.EqualTo(expectedJoltage));
        }

        [Test]
        public void JoltageBankFromFile()
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", "Day3BatteryValues.txt");
            
            var result = batteryBank.JoltagesFromFile(fileToLoad);
            
            Assert.That(result, Is.EqualTo((BigInteger)167549941654721));
        }

        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
            
        }
    }
}