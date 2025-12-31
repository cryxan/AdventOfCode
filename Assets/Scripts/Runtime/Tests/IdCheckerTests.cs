using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using AdventOfCode;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    public class IdCheckerTests
    {
        IdChecker  idChecker;
        
        [SetUp]
        public void Setup()
        {
            idChecker = new IdChecker();
        }

        [TestCase("1", true)]
        [TestCase("111", false)]
        
        [TestCase("12", true)]
        [TestCase("11", false)]
        
        
        [TestCase("303302", true)]
        
        [TestCase("303303", false)]
        [TestCase("303030", false)]
        [TestCase("303303303", false)]
        
        public void CheckIdValidty(string id, bool valid)
        {
            var result = idChecker.IdValid(id);
            
            Assert.That(result, Is.EqualTo(valid));
        }
        
        [TestCase("a", false, 0,0)]
        [TestCase("a-b", false, 0,0)]
        [TestCase("10", false, 0,0)]
        [TestCase("10-20", true, 10,20)]
        [TestCase("1234-7890", true, 1234,7890)]
        public void CheckIdRangeIsParsed(string idRange , bool valid, int rangeStart, int rangeEnd)
        {
            var result = idChecker.ParseRange(idRange, out var start, out var end);
            
            Assert.That(result, Is.EqualTo(valid));
            Assert.That(start, Is.EqualTo((BigInteger)rangeStart), "Start");
            Assert.That(end, Is.EqualTo((BigInteger)rangeEnd), "End");
        }

        [Test]
        public void CheckIdFile()
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", "Day2IdCodes.txt");
            var result = idChecker.CheckIdFile(fileToLoad);
            
            Assert.That(result, Is.EqualTo(true));
            Assert.That(idChecker.checkAccumulator, Is.EqualTo((BigInteger)25912654282));
        }
        
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
            
        }
    }
}