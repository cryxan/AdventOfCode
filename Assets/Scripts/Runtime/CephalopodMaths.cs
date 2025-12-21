using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdventOfCode
{
    public class CephalopodMaths
    {
        public CephalopodMaths()
        {
        }

        public int LoadCephalopodMathsProblem(string filename)
        {
            if (!File.Exists(filename))
            {
                return -1;
            }
            
            var text = File.ReadAllText(filename);
            
            var textLines = text.Split("\r\n");

            var problemLines = new List<List<string>>();
            
            foreach (var textLine in textLines)
            {
                var line = textLine.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                problemLines.Add(line);
            }

            var checkCount = problemLines[0].Count;
            foreach (var problemLine in problemLines)
            {
                if (problemLine.Count != checkCount)
                {
                    return -2;
                }
            }

            foreach (var operation in problemLines[^1])
            {
                if (operation != "+" && operation != "*") return -3;
            }
            
            var operations =  problemLines[^1];
            var grandTotal = 0;
            for (var col = 0; col < operations.Count; col++)
            {
                switch (operations[col])
                {
                    case "+":
                        grandTotal += AddColumn(problemLines, col);
                        break;
                    case "*":
                        grandTotal += MultiplyColumn(problemLines, col);
                        break;
                }
            }
            
            return grandTotal;
        }

        private int MultiplyColumn(List<List<string>> problemLines, int col)
        {
            var total = 1;
            for (var y = 0; y < (problemLines.Count-1); y++)
            {
                if (int.TryParse(problemLines[y][col], out var value))
                {
                    total *= value;
                }
                else
                {
                    total += 0;
                }
            }
            return total;
        }

        private int AddColumn(List<List<string>> problemLines, int col)
        {
            var total = 0;
            for (var y = 0; y < (problemLines.Count-1); y++)
            {
                if (int.TryParse(problemLines[y][col], out var value))
                {
                    total += value;
                }
                else
                {
                    total += 0;
                }
            }
            return total;
        }
    }
}