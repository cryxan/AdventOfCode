using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdventOfCode
{
    public class Circuits
    {
        List<Vector3Int> circuits = new List<Vector3Int>();
        
        public Circuits()
        {
            
        }

        public int LoadAndAnalyseCircuits(string filename)
        {
            if (!File.Exists(filename)) return -1;
            
            var text = File.ReadAllText(filename);

            var circuitLocationStrings = text.Split("\r\n");
            foreach (var circuitLocationString in circuitLocationStrings)
            {
                var locationArray = circuitLocationString.Split(',');
                var location = new Vector3Int(
                    int.Parse(locationArray[0]),
                    int.Parse(locationArray[1]),
                    int.Parse(locationArray[2]));
                circuits.Add(location);
            }

            return -2;
        }
    }
}