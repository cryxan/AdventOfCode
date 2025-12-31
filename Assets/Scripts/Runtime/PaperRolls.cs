using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace AdventOfCode
{
    public class PaperRolls
    {
        private char[][] rollsArray;

        public int LoadPaperRollsAndAnalyseOnce(string filename)
        {
            if (LoadRollsArray(filename, out var loadPaperRollsAndAnalyseOnce)) return loadPaperRollsAndAnalyseOnce;

            return AnalysePaperRollsAndUpdate();
        }
        public int LoadPaperRollsAndAnalyseAndRepeat(string filename)
        {
            if (LoadRollsArray(filename, out var loadPaperRollsAndAnalyseOnce)) return loadPaperRollsAndAnalyseOnce;

            var total = 0;
            var count = 0;
            do
            {
                count = AnalysePaperRollsAndUpdate();
                total += count;

            } while (count> 0);

            return total;
        }
        private bool LoadRollsArray(string filename, out int loadPaperRollsAndAnalyseOnce)
        {
            loadPaperRollsAndAnalyseOnce = -3;
            
            if (!File.Exists(filename))
            {
                loadPaperRollsAndAnalyseOnce = -2;
                return true;
            }
            
            var text = File.ReadAllText(filename);
            
            var textLines = text.Split("\r\n");
            rollsArray = new char[textLines.Length][];
            for ( var y = 0; y < textLines.Length; y++)
            {
                rollsArray[y] = textLines[y].ToCharArray();
            }
            return false;
        }

        public int AnalysePaperRollsAndUpdate()
        {
            var lineLength = rollsArray[0].Length;
            foreach (var rollLine in rollsArray)
            {
                if (rollLine.Length != lineLength) return -1;
            }

            List<Vector2Int> foundRolls = new List<Vector2Int>();
            
            var rollCount = 0;
            for (var y = 0; y < rollsArray.Length; y++)
            {
                for (var x = 0; x < rollsArray[y].Length; x++)
                {
                    if (CountNeighbors(x, y) < 4)
                    {
                        rollCount++;
                        foundRolls.Add(new Vector2Int(x, y));
                    }
                }
            }

            foreach (var roll in foundRolls)
            {
                rollsArray[roll.y][roll.x] = 'x';
            }
            
            return rollCount;
        }

        private int CountNeighbors(int x, int y)
        {
            var count = -1;

            if (rollsArray[y][x] != '@') return 8;
            
            for (var xCheck = x-1; xCheck <= x+1; xCheck++)
            {
                for (var yCheck = y-1; yCheck <= y+1; yCheck++)
                {
                    count += CheckRoll(xCheck, yCheck);
                }
            }
            
            return count;
        }

        private int CheckRoll(int x, int y)
        {
            if (x < 0 || y < 0) return 0;
            
            if ( y >= rollsArray.Length) return 0;
            if ( x >= rollsArray[y].Length) return 0;

            return rollsArray[y][x] == '@' ? 1 : 0;
        }


    }
}