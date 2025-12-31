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
        private Dictionary<long, long> xToSmall;
        private Dictionary<long, long> xFromSmall;
        private Dictionary<long, long> yToSmall;
        private Dictionary<long, long> yFromSmall;
        private long xWidth;
        private long yWidth;
        private char[,] compressedMap;

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

            BuildCoordinateMappings();

            InitialiseMap();
            
            //Debug.Log(DumpMap());
            
            return FindLargestCarpetWithBounds();
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
        private void BuildCoordinateMappings()
        {
            var xCoords = carpetCorners.OrderBy(x => x.x)
                .Select(x => x.x).Distinct().ToList();
            var yCoords = carpetCorners.OrderBy(x => x.y)
                .Select(x => x.y).Distinct().ToList();

            xToSmall = BuildMapToSmall(xCoords);
            xFromSmall = BuildMapBack(xToSmall);
            yToSmall = BuildMapToSmall(yCoords);
            yFromSmall = BuildMapBack(yToSmall);

            xWidth = xToSmall.Select(mapping => mapping.Value).Prepend(0L).Max()+1;
            yWidth = yToSmall.Select(mapping => mapping.Value).Prepend(0L).Max()+1;
            
        }
        
        private void InitialiseMap()
        {
            compressedMap = new char[xWidth, yWidth];

            for (var y = 0; y < yWidth; y++)
            {
                for (var x = 0; x < xWidth; x++)
                {
                    compressedMap[x, y] = '.';
                }
            }

            for (var index = 0; index < carpetCorners.Count; index++)
            {
                var start =  carpetCorners[index];
                var end = carpetCorners[(index + 1) % carpetCorners.Count];
                DrawInMap(start, end);
            }

            if (carpetCorners[0].y == carpetCorners[1].y)
            {
                SpanFill(xToSmall[carpetCorners[0].x] + 1,
                    yToSmall[carpetCorners[0].y] + 1
                    , '#');
            }
            else
            {
                SpanFill(xToSmall[carpetCorners[0].x] - 1,
                    yToSmall[carpetCorners[0].y] + 1
                    , '#');
            }

        }

        private void DrawInMap(Vector2Long start, Vector2Long end)
        {
            if (start.x == end.x)
            {
                DrawLineInMapVertical(start, end);
            }
            else
            {
                DrawLineInMapHorizontal(start, end);
            }
        }
        

        private void DrawLineInMapVertical(Vector2Long start, Vector2Long end)
        {
            if (!xToSmall.ContainsKey(start.x) ||
                !yToSmall.ContainsKey(start.y) ||
                !yToSmall.ContainsKey(end.y)) return;
            
            var xStart = xToSmall[start.x];
            var yStart = yToSmall[start.y];
            var yEnd = yToSmall[end.y];
            if (yEnd < yStart)
            {
                (yStart, yEnd) = (yEnd, yStart);
            }
            for (var y = yStart; y <= yEnd; y++)
            {
                compressedMap[xStart, y] = '#';
            }
        }

        private void DrawLineInMapHorizontal(Vector2Long start, Vector2Long end)
        {
            if (!yToSmall.ContainsKey(start.y) ||
                !xToSmall.ContainsKey(start.x) ||
                !xToSmall.ContainsKey(end.x)) return;
            
            var yStart = yToSmall[start.y];
            var xstart = xToSmall[start.x];
            var xEnd = xToSmall[end.x];
            if (xEnd < xstart)
            {
                (xstart, xEnd) = (xEnd, xstart);
            }
            for (var x = xstart; x <= xEnd; x++)
            {
                compressedMap[x, yStart] = '#';
            }
        }

        private long[,] floodOffsets = new long[,] {{0,-1}, {-1,0},{1,0}, {0,1}};
        private void FloodFill(long startX, long startY, char c, char charToFill = '.')
        {
            if ( compressedMap[startX, startY] != charToFill) return;
            
            compressedMap[startX, startY] = c;
            
            for (int i = 0 ; i < floodOffsets.GetLength(0); i++)
            {
                var offsetX = startX+floodOffsets[i,0];
                var offsetY = startY + floodOffsets[i,1];
                if (compressedMap[offsetX, offsetY] == charToFill)
                {
                    FloodFill(offsetX, offsetY, c);
                }
            }
        }

        private void SpanFill(long startX, long startY, char c, char charToFill = '.')
        {
            if ( compressedMap[startX, startY] != charToFill) return;
            
            //Fill left
            var SpanStart = startX;
            var SpanEnd = startX;
            while (SpanStart >= 0 && compressedMap[SpanStart, startY] == charToFill) SpanStart--;
            while (SpanEnd < xWidth && compressedMap[SpanEnd, startY] == charToFill) SpanEnd++;
            for (var x = SpanStart; x <= SpanEnd; x++) compressedMap[x, startY] = c;

            for (var x = SpanStart; x <= SpanEnd; x++)
            {
                if (compressedMap[x, startY-1] == charToFill) SpanFill(x, startY-1, c);
            }
            
            for (var x = SpanStart; x <= SpanEnd; x++)
            {
                if (compressedMap[x, startY+1] == charToFill) SpanFill(x, startY+1, c);
            }
        }
        private String DumpMap()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < yWidth; y++)
            {
                for (var x = 0; x < xWidth; x++)
                {
                    sb.Append(compressedMap[x, y]);
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }
        private Dictionary<long, long> BuildMapToSmall(List<long> coordinates)
        {
            var map = new Dictionary<long, long>();
            var workingIndex = 0;
            foreach (var coordinate in coordinates)
            {
                map[coordinate] = workingIndex;
                workingIndex += 2;
            }

            return map;
        }

        private Dictionary<long, long> BuildMapBack(Dictionary<long, long> mapToSmall)
        {
            var map = new Dictionary<long, long>();
            foreach (var coordinate in mapToSmall)
            {
                map[coordinate.Value] = coordinate.Key;
            }
            return map;
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
        
        private long FindLargestCarpetWithBounds()
        {
            var largestCarpet = 0L;
            for (var index1 = 0; index1 < carpetCorners.Count-1; index1++)
            {
                for (var index2 = index1 + 1; index2 < carpetCorners.Count; index2++)
                {
                    if (ValidCarpet(index1, index2))
                    {
                        var carpetDimension = carpetCorners[index1].Subtract(carpetCorners[index2]);

                        var carpetArea = (math.abs(carpetDimension.x) + 1) * (math.abs(carpetDimension.y) + 1);

                        if (carpetArea > largestCarpet) largestCarpet = carpetArea;
                    }
                }
            }
            return largestCarpet;
        }

        private bool ValidCarpet(int index1, int index2)
        {
            var minX = xToSmall[math.min(carpetCorners[index1].x, carpetCorners[index2].x)];
            var maxX = xToSmall[math.max(carpetCorners[index1].x, carpetCorners[index2].x)];
            var minY = yToSmall[math.min(carpetCorners[index1].y, carpetCorners[index2].y)];
            var maxY = yToSmall[math.max(carpetCorners[index1].y, carpetCorners[index2].y)];

            for (var x = minX; x <= maxX; x++)
            {
                if (compressedMap[x,minY] != '#' || compressedMap[x,maxY] != '#') return false;
            }

            for (var y = minY; y <= maxY; y++)
            {
                if (compressedMap[minX,y] != '#' || compressedMap[maxX,y] != '#') return false;
            }
            
            return true;
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