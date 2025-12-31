using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventOfCode
{
    public class Circuits
    {
        List<JunctionBox> junctionBoxesList = new List<JunctionBox>();
        HashSet<JunctionBox> junctionBoxesHashset = new HashSet<JunctionBox>();
        List<JunctionDistance> rawJunctionDistancesList = new List<JunctionDistance>();
        List<JunctionDistance> sortedJunctionDistancesList = new List<JunctionDistance>();
        HashSet<CircuitMembers> circuitMembersList = new HashSet<CircuitMembers>();
        
        public Circuits()
        {
            
        }

        public int LoadAndAnalyseCircuits(string filename, int connectionsToMake)
        {
            if (!LoadFile(filename)) return -1;

            BuildJunctionBoxDistances();

            FindConnections(connectionsToMake);

            return CircuitMultiplier();
        }
        
        public long LoadAndAnalyseToSingleCircuit(string filename)
        {
            if (!LoadFile(filename)) return -1;

            BuildJunctionBoxDistances();

            return FindConnectionsForSingle();

            
        }

        private bool LoadFile(string filename)
        {
            if (!File.Exists(filename))
            {

                return false;
            }
            
            var text = File.ReadAllText(filename);

            var circuitLocationStrings = text.Split("\r\n");
            foreach (var circuitLocationString in circuitLocationStrings)
            {
                var locationArray = circuitLocationString.Split(',');
                var location = new Vector3Long(
                    int.Parse(locationArray[0]),
                    int.Parse(locationArray[1]),
                    int.Parse(locationArray[2]));
                var box = new JunctionBox(location);
                junctionBoxesList.Add(box);
                junctionBoxesHashset.Add(box);
            }

            return true;
        }

        private int CircuitMultiplier()
        {
            var circuitSizes = circuitMembersList.OrderByDescending(x => x.junctionBoxes.Count).Select(x => x.junctionBoxes.Count).ToList();
            
            return  circuitSizes[0]*circuitSizes[1]*circuitSizes[2];
        }

        public void BuildJunctionBoxDistances()
        {
            for (var index1 = 0; index1 < junctionBoxesList.Count - 1; index1++)
            {
                for (var index2 = index1 + 1; index2 < junctionBoxesList.Count; index2++)
                {
                    var sqrMagnitude = junctionBoxesList[index1].location.Subtract(junctionBoxesList[index2].location).sqrMagnitude;
                    var distanceEntry = new JunctionDistance {box1 = junctionBoxesList[index1], box2 = junctionBoxesList[index2], sqrDistance = sqrMagnitude};
                    rawJunctionDistancesList.Add(distanceEntry);
                }
            }
            
            sortedJunctionDistancesList = rawJunctionDistancesList.OrderBy(x => x.sqrDistance).ToList();
            Debug.Log($"boxcount = {junctionBoxesList.Count}, distanceCount = {sortedJunctionDistancesList.Count}");;
        }

        private void FindConnections(int connectionsToMake)
        {
            var circuitCountDown = connectionsToMake;
            foreach (var junctionDistance in sortedJunctionDistancesList)
            {
                var box1 = junctionDistance.box1;
                var box2 = junctionDistance.box2;
                if (box1.circuitMembership == null && box2.circuitMembership == null)
                {
                     AddNewCircuit(box1, box2);
                }
                else if (box1.circuitMembership != null && box2.circuitMembership == null)
                {
                    AddCircuitMember(box1, box2);
                }
                else if (box1.circuitMembership == null && box2.circuitMembership != null)
                {
                    AddCircuitMember(box2, box1);
                }
                else if (box1.circuitMembership != box2.circuitMembership)
                {
                    MergeCircuits(box1, box2);
                }

                if (--circuitCountDown == 0) break; // 10 connections made
            }
        }

        private long FindConnectionsForSingle()
        {
            foreach (var junctionDistance in sortedJunctionDistancesList)
            {
                var box1 = junctionDistance.box1;
                var box2 = junctionDistance.box2;
                if (box1.circuitMembership == null && box2.circuitMembership == null)
                {
                    AddNewCircuit(box1, box2);
                }
                else if (box1.circuitMembership != null && box2.circuitMembership == null)
                {
                    AddCircuitMember(box1, box2);
                }
                else if (box1.circuitMembership == null && box2.circuitMembership != null)
                {
                    AddCircuitMember(box2, box1);
                }
                else if (box1.circuitMembership != box2.circuitMembership)
                {
                    MergeCircuits(box1, box2);
                }

                if (box1.circuitMembership.junctionBoxes.Count == junctionBoxesList.Count)
                {
                    return box1.location.x * box2.location.x;
                }
            }

            return -1;
        }

        private void AddNewCircuit(JunctionBox box1, JunctionBox box2)
        {
            var circuitMembers = new CircuitMembers();
            box1.circuitMembership = circuitMembers;
            box2.circuitMembership = circuitMembers;
            circuitMembers.junctionBoxes.Add(box1);
            circuitMembers.junctionBoxes.Add(box2);
            circuitMembersList.Add(circuitMembers);
        }

        private void AddCircuitMember(JunctionBox box1, JunctionBox box2)
        {
            box1.circuitMembership.junctionBoxes.Add(box2);
            box2.circuitMembership = box1.circuitMembership;
        }
        
        private void MergeCircuits(JunctionBox box1, JunctionBox box2)
        {
            var boxList = box2.circuitMembership.junctionBoxes;
            var circuitMembership = box2.circuitMembership;
            foreach (var boxToMerge in boxList)
            {
               AddCircuitMember(box1, boxToMerge);
            }

            circuitMembersList.Remove(circuitMembership);
        }
    }
    


    public class JunctionBox
    {
        public Vector3Long location;
        public CircuitMembers circuitMembership = null;
        
        public JunctionBox(Vector3Long location)
        {
            this.location = location;
            this.circuitMembership = null;
        }
    }

    public class JunctionDistance
    {
        public JunctionBox box1;
        public JunctionBox box2;
        public long sqrDistance;

    }

    public class Vector3Long
    {
        public long x;
        public long y;
        public long z;
        public long sqrMagnitude;
        public Vector3Long(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            sqrMagnitude = (x * x) + (y * y) + (z * z);
        }

        public Vector3Long Subtract(Vector3Long other)
        {
            return new Vector3Long(this.x-other.x, this.y-other.y, this.z-other.z);
        }
    }
    public class CircuitMembers
    {
        public HashSet<JunctionBox> junctionBoxes = new HashSet<JunctionBox>();
    }
}