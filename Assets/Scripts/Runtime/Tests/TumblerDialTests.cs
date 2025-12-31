using System.Collections;
using System.Collections.Generic;
using System.IO;
using AdventOfCode;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    public class TumblerDialTests
    {
        private TumblerDial tumblerDial;
        [SetUp]
        public void Setup()
        {
            tumblerDial = new TumblerDial();
        }

        [Test]
        public void DialStartsAt50()
        {
            var tumbler = new TumblerDial();
            
            Assert.That(tumbler.dialPosition, Is.EqualTo(50));
        }

        [TestCase("L1", true, -1)]
        [TestCase("L123", true, -123)]
        [TestCase("L20", true, -20)]
        
        [TestCase("R1", true, 1)]
        [TestCase("R123", true, 123)]
        [TestCase("R10", true, 10)]
        
        [TestCase("L-1", false,0)]
        [TestCase("R-1", false,0)]
        public void DialChangeShouldBe(string dialCode, bool success, int expectedChange)
        {
            var result = tumblerDial.DialChange(dialCode, out var changeValue);
            
            Assert.That(result, Is.EqualTo(success));
            Assert.That(changeValue, Is.EqualTo(expectedChange));
        }

        [TestCase(50, "L1", 49)]
        [TestCase(0, "L1", 99)]
        [TestCase(50, "R1", 51)]
        [TestCase(99, "R1", 0)]
        [TestCase(50, "R-1", 50)]
        public void DialPositionShouldBe(int startPosition, string dialCode, int expectedPosition)
        {
            tumblerDial.dialPosition = startPosition;
            
            tumblerDial.MoveDial(dialCode);
            
            Assert.That(tumblerDial.dialPosition, Is.EqualTo(expectedPosition));
        }
        
        [TestCase(50, 0, "L1")]
        [TestCase(50, 1, "L50")]
        [TestCase(50, 2, "L50", "L1", "R1")]
        [TestCase(50, 3, "L50", "L1", "R101")]

        [TestCase(50, 2, "L150")]
        [TestCase(50, 3, "L250")]
        [TestCase(50, 2, "R150")]
        [TestCase(50, 3, "R250")]

        public void DialSequenceShouldCountZeroHitsAndPasses(int startPosition, int expectedZeroCount, params string[] dialCodes)
        {
            tumblerDial.dialPosition = startPosition;
            
            var result = tumblerDial.DialSequence(dialCodes);
            
            Assert.That(result, Is.EqualTo(expectedZeroCount));
        }

        [TestCase("noFile", false)]
        [TestCase( "Day1TumblerCodes.txt", true)]
        public void DialsFromDiskFile(string fileName, bool success)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", fileName);
            
            var result = tumblerDial.DialFile(fileToLoad, out var zeroHitCount);
            
            Assert.That(result, Is.EqualTo(success));
            Assert.That(zeroHitCount, Is.EqualTo(success?5923:0));
        }

        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
            
        }
    }
}