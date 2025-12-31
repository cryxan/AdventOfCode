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
        private Circuits  circuits;
        
        [SetUp]
        public void Setup()
        {
            circuits = new Circuits();
        }

        [TestCase("Day8CircuitExample.txt", 10, 40)]
        [TestCase("Day8Circuit.txt", 1000, 0)]
        public void CircuitsReturnExpectedValue(string filename, int numberOfConnections, int expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = circuits.LoadAndAnalyseCircuits(fileToLoad, numberOfConnections);
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [TestCase("Day8CircuitExample.txt", 10, 25272)]
        [TestCase("Day8Circuit.txt", 1000, 724454082)]
        public void CircuitsReturnExpectedValueOnOneCircuit(string filename, int numberOfConnections, long expected)
        {
            var fileToLoad = Path.Combine(GetThisFilePath(), "TestFiles", filename);
            
            var result = circuits.LoadAndAnalyseToSingleCircuit(fileToLoad);
            
            Assert.That(result, Is.EqualTo(expected));
        }

        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
            
        }
    }
}