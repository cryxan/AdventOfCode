using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdventOfCode
{
    public class Circuits
    {
        List<JunctionBox> junctionBoxes = new List<JunctionBox>();
        
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
                var location = new Vector3(
                    int.Parse(locationArray[0]),
                    int.Parse(locationArray[1]),
                    int.Parse(locationArray[2]));
                junctionBoxes.Add(new JunctionBox(location));
            }

            return -2;
        }
    }

    public class JunctionBox
    {
        public Vector3 location;
        
        public JunctionBox(Vector3 location)
        {
            this.location = location;
        }
    }
}