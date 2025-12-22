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
    public class CircuitsTest
    {
        Circuits  circuits;
        
        [SetUp]
        public void Setup()
        {
            circuits = new Circuits();
        }

        [TestCase("Day8CircuitExample.txt", 40)]
        [TestCase("Day8Circuit.txt", 0)]
        void CircuitsReturnExpectedValue(string filename, int expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = circuits.LoadAndAnalyseCircuits(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
            
        }
    }
}