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
        private List<CarpetCorner> carpetCorners;
        private List<LineSeqment> lineSeqments = new List<LineSeqment>();
        private List<Boundry> horizontalBoundries;
        private List<Boundry> verticalBoundries;


        public Carpet()
        {
            carpetCorners = new List<CarpetCorner>();
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

            BuildLineSegmentBoundries();
            
            return FindLargestCarpetWithBounds();
        }

        private long FindLargestCarpetWithBounds()
        {
            var largestCarpet = 0L;
            for (var index1 = 0; index1 < carpetCorners.Count-1; index1++)
            {
                for (var index2 = index1 + 1; index2 < carpetCorners.Count; index2++)
                {
                    var carpetArea = 0L;
                    
                    var carpetCorner1 = carpetCorners[index1];
                    var carpetCorner2 = carpetCorners[index2];
                    if (carpetCorner1.verticalSegment == carpetCorner2.verticalSegment ||
                        carpetCorner1.horizontalSegment == carpetCorner2.horizontalSegment)
                    {
                        // both on same line segment need no further processing
                        carpetArea = CarpetArea(carpetCorner1.position, carpetCorner2.position);
                    }
                    else if ( !EarlyOut( carpetCorner1, carpetCorner2) )
                    {
                        carpetArea = AreaIfInBounds(carpetCorners[index1].position, carpetCorners[index2].position);
                    }

                    
                    if (carpetArea > largestCarpet) largestCarpet = carpetArea;
                }
            }
            return largestCarpet;
        }

        private bool EarlyOut(CarpetCorner carpetCorner1, CarpetCorner carpetCorner2)
        {
            if (carpetCorner1.position.x < carpetCorner2.position.x)
            {
                if (carpetCorner1.verticalSegment.boundry.boundryOrientation != Boundry.Orientation.up ||
                    carpetCorner2.verticalSegment.boundry.boundryOrientation != Boundry.Orientation.down)
                {
                    return false;
                }
            }
            else
            {
                if (carpetCorner1.verticalSegment.boundry.boundryOrientation != Boundry.Orientation.down ||
                    carpetCorner2.verticalSegment.boundry.boundryOrientation != Boundry.Orientation.up)
                {
                    return false;
                }
            }

            if (carpetCorner1.position.y < carpetCorner2.position.y)
            {
                if (carpetCorner1.horizontalSegment.boundry.boundryOrientation != Boundry.Orientation.right ||
                    carpetCorner2.horizontalSegment.boundry.boundryOrientation != Boundry.Orientation.left)
                {
                    return false;
                }
            }
            else
            {
                if (carpetCorner1.horizontalSegment.boundry.boundryOrientation != Boundry.Orientation.left ||
                    carpetCorner2.horizontalSegment.boundry.boundryOrientation != Boundry.Orientation.right)
                {
                    return false;
                }
            }
            return true;
        }

        private long AreaIfInBounds(Vector2Long corner1, Vector2Long corner2)
        {
            var minx = Math.Min(corner1.x, corner2.x);
            var maxx = Math.Max(corner1.x, corner2.x);
            var miny = Math.Min(corner1.y, corner2.y);
            var maxy = Math.Max(corner1.y, corner2.y);

            var topLeft = new Vector2Long(minx, miny);
            var topRight = new Vector2Long(maxx, miny);
            var bottomLeft = new Vector2Long(minx, maxy);
            var bottomRight = new Vector2Long(maxx, maxy);

            if (!CheckVerticalBound(topLeft, topRight)) return 0;
            if (!CheckVerticalBound(bottomLeft, bottomRight)) return 0;
            if (!CheckHorizontalBound(topLeft, bottomLeft)) return 0;
            if (!CheckHorizontalBound(topRight, bottomRight)) return 0;
            
            return (maxx - minx + 1)*(maxy-miny+1);
        }

        private long CarpetArea(Vector2Long corner1, Vector2Long corner2)
        {
            return (math.abs(corner1.x-corner2.x)+1)*(math.abs(corner1.y-corner2.y)+1);
        }

        private bool CheckVerticalBound(Vector2Long leftCoord, Vector2Long rightCoord)
        {
            for (var i = 0; i < verticalBoundries.Count - 1; i++)
            {
                if (verticalBoundries[i].bound <= leftCoord.x &&
                    InBound(verticalBoundries[i].boundStart,
                            verticalBoundries[i].boundEnd,
                            leftCoord.y) &&
                            verticalBoundries[i].boundryOrientation == Boundry.Orientation.up)
                {
                    for (var j = i + 1; j < verticalBoundries.Count; j++)
                    {
                        if ( InBound(verticalBoundries[j].boundEnd,
                                     verticalBoundries[j].boundStart, rightCoord.y) &&
                             verticalBoundries[j].boundryOrientation == Boundry.Orientation.down)
                        {
                            return rightCoord.x <= verticalBoundries[j].bound;
                        }
                    }

                    break;
                }
            }
            return false;
        }
        private bool CheckHorizontalBound(Vector2Long topCoord, Vector2Long bottomCoord)
        {
            for (var i = 0; i < horizontalBoundries.Count - 1; i++)
            {
                if (horizontalBoundries[i].bound <= topCoord.y &&
                    InBound(horizontalBoundries[i].boundStart,
                        horizontalBoundries[i].boundEnd,
                        topCoord.x) &&
                    horizontalBoundries[i].boundryOrientation == Boundry.Orientation.right)
                {
                    for (var j = i + 1; j < horizontalBoundries.Count; j++)
                    {
                        if ( InBound(horizontalBoundries[j].boundEnd,
                            horizontalBoundries[j].boundStart, bottomCoord.x) &&
                            horizontalBoundries[j].boundryOrientation == Boundry.Orientation.left)
                        {
                            return bottomCoord.y <= horizontalBoundries[j].bound;
                        }
                    }

                    break;
                }
            }
            return false;
        }
        private bool InBound(long boundStart, long boundEnd, long value)
        {
            if (boundStart <= boundEnd) return boundStart <=value && value <= boundEnd;
            
            return boundEnd <= value && value <= boundStart;
        }

        private void BuildLineSegmentBoundries()
        {
            for (var i = 0; i < carpetCorners.Count; i++)
            {
                var corner1 = carpetCorners[i];
                var corner2 = carpetCorners[(i + 1) % carpetCorners.Count];
                var lineSegment = new LineSeqment(corner1.position, corner2.position);
                lineSeqments.Add(lineSegment);
                if (corner1.position.x == corner2.position.x)
                {
                    corner1.verticalSegment = lineSegment;
                    corner2.verticalSegment = lineSegment;
                }
                else
                {
                    corner1.horizontalSegment = lineSegment;
                    corner2.horizontalSegment = lineSegment;
                }
                    
            }
            
            var horizontalBoundries = new List<Boundry>();
            var verticalBoundries =  new List<Boundry>();

            foreach (var lineSegment in lineSeqments)
            {
                var boundry = new Boundry(lineSegment.start, lineSegment.end);
                lineSegment.boundry = boundry;
                if (boundry.boundryOrientation is Boundry.Orientation.right or Boundry.Orientation.left)
                {
                    horizontalBoundries.Add(boundry);
                }
                else
                {
                    verticalBoundries.Add(boundry);
                }
            }
            
            this.horizontalBoundries = horizontalBoundries.OrderBy(x => x.bound).ToList();
            this.verticalBoundries = verticalBoundries.OrderBy(x => x.bound).ToList();
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
                carpetCorners.Add(new CarpetCorner(new Vector2Long(long.Parse(tileCoordinateStrings[0]), long.Parse(tileCoordinateStrings[1]))));
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
                    var carpetDimension = carpetCorners[index1].position.Subtract(carpetCorners[index2].position);
                    
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

    public class CarpetCorner
    {
        public Vector2Long position;
        
        public LineSeqment horizontalSegment;
        public LineSeqment verticalSegment;
        
        public CarpetCorner(Vector2Long position)
        {
            this.position = position;
        }
    }

    public class LineSeqment
    {
        public Vector2Long start;
        public Vector2Long end;
        public Boundry boundry;
        public LineSeqment(Vector2Long start, Vector2Long end)
        {
            this.start = start;
            this.end = end;
        }
    }

    public class Boundry
    {
        public long bound;
        public long boundStart;
        public long boundEnd;
        public Orientation boundryOrientation;
        
        public Boundry(Vector2Long start, Vector2Long end)
        {
            if (start.x == end.x)
            {
                // Vertical boundary
                bound = start.x;
                boundStart = start.y;
                boundEnd = end.y;
                boundryOrientation = start.y < end.y ? Orientation.down : Orientation.up;
            }
            else 
            {
                // Horizontal boundry
                bound = start.y;
                boundStart = start.x;
                boundEnd = end.x;
                boundryOrientation = start.x < end.x ? Orientation.right : Orientation.left;
            }
        }
        public enum Orientation
        {
            right,
            down,
            left,
            up
        }
    }


}