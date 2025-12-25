using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

namespace AdventOfCode
{
    public class Carpet
    {
        private List<Vector2Long> carpetCorners;
 
        public Carpet()
        {
            carpetCorners = new List<Vector2Long>();
        }

        public long LoadFileAndFindLargestCarpet(string filename)
        {
            if (!LoadFile(filename))
                return -1;
            return FindLargestCarpet();
        }
        
        public long LoadFileAndFindLargestCarpetInBoundary(string filename)
        {
            if (!LoadFile(filename))
                return -1;

            return -2;
        }

        public bool LoadFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }
            
            var text = File.ReadAllText(filename);
            
            var coordinateLines = text.Split("\r\n");
            foreach (var coordinateLine in coordinateLines)
            {
                var tileCoordinateStrings = coordinateLine.Split(",");
                carpetCorners.Add(new Vector2Long(long.Parse(tileCoordinateStrings[0]), long.Parse(tileCoordinateStrings[1])));
            }
            return true;
        }
        
        private long FindLargestCarpet()
        {
            var largestCarpet = 0L;
            for (var index1 = 0; index1 < carpetCorners.Count-1; index1++)
            {
                for (var index2 = index1 + 1; index2 < carpetCorners.Count; index2++)
                {
                    var carpetDimension = carpetCorners[index1].Subtract(carpetCorners[index2]);
                    
                    var carpetArea = (math.abs(carpetDimension.x)+1) * (math.abs(carpetDimension.y)+1);
                    
                    if (carpetArea > largestCarpet) largestCarpet = carpetArea;
                }
            }
            return largestCarpet;
        }
    }
    


    public class Vector2Long
    {
        public long x;
        public long y;

        public long sqrMagnitude;
        public Vector2Long(long x, long y)
        {
            this.x = x;
            this.y = y;
            sqrMagnitude = (x * x) + (y * y);
        }

        public Vector2Long Subtract(Vector2Long other)
        {
            return new Vector2Long(this.x-other.x, this.y-other.y);
        }
    }

}