using System.IO;
using System.Text;
using UnityEngine;

namespace AdventOfCode
{
    public class Tachyon
    {
        private char[][] tachyonMatrix;
        private long[][] pathCounts;

        public Tachyon()
        {
        }

        public long LoadAndAnalyseTachyonMatrixWorlds(string filename)
        {
            if (!LoadTachyonLayout(filename)) return -1;

            return AnalyseTachyonMatrixWorlds();
        }
        public int LoadAndAnalyseTachyonMatrixSplits(string filename)
        {
            if (!LoadTachyonLayout(filename)) return -1;

            return  AnalyseTachyonMatrixSplits();
        }

        private long AnalyseTachyonMatrixWorlds()
        {
            var sourceCol = SourceCol();
            if (sourceCol < 0) return sourceCol;

            _ = AnalyseTachyonMatrixForWorlds();
            InitialisePathCounts();

            return AnalyseMatrixForWorlds();
        }

        private long AnalyseMatrixForWorlds()
        {
            for (var y = 0; y < pathCounts.Length; y++)
            {
                for (var x = 0; x < pathCounts[y].Length; x++)
                {
                    if (GetSymbolAt(x, y) == 'S')
                    {
                        // Only one on this line
                        pathCounts[y][x] = 1L;
                        break;
                    }
                    
                    if (GetSymbolAt(x, y) == '|')
                    {
                        pathCounts[y][x] = pathCounts[y - 1][x];
                    }
                    else if (GetSymbolAt(x, y) == '^')
                    {
                        pathCounts[y][x-1] += pathCounts[y - 1][x] + pathCounts[y-1][x-1];
                        pathCounts[y - 1][x - 1] = 0L;
                        pathCounts[y][x+1] += pathCounts[y - 1][x] + pathCounts[y-1][x+1];
                        pathCounts[y - 1][x + 1] = 0L;
                    }
                }

                y++;
                for (var x = 0; x < pathCounts[y].Length; x++)
                {
                    pathCounts[y][x] = pathCounts[y - 1][x];
                }
            }
            
            Debug.Log(DumpCounts(pathCounts));

            var total = 0L;
            foreach (var value in pathCounts[pathCounts.Length - 1])
            {
                total += value;
            }
            
            return total;
        }

        private char GetSymbolAt(int x, int y)
        {
            if (y < 0 || y >= tachyonMatrix.Length) return '.';
            
            if (x < 0 || x >= tachyonMatrix[0].Length) return '.';
            
            return tachyonMatrix[y][x];
        }

        private int AnalyseTachyonMatrixSplits()
        {
            var sourceCol = SourceCol();
            if (sourceCol < 0) return sourceCol;

            tachyonMatrix[1][sourceCol] = '|';
            var splits = 0;

            for (var workingRow = 2; workingRow < tachyonMatrix.Length; workingRow += 2)
            {
                for (var workingCol = 0; workingCol < tachyonMatrix[workingRow].Length; workingCol++)
                {
                    if (tachyonMatrix[workingRow][workingCol] == '.' &&
                        tachyonMatrix[workingRow - 1][workingCol] == '|')
                    {
                        tachyonMatrix[workingRow][workingCol] = '|';
                    }
                    else if (tachyonMatrix[workingRow][workingCol] == '^' &&
                        tachyonMatrix[workingRow - 1][workingCol] == '|')
                    {
                        splits++;
                        
                        if (tachyonMatrix[workingRow][workingCol-1] != '|')
                            tachyonMatrix[workingRow][workingCol-1] = '+';
                        
                        if (tachyonMatrix[workingRow][workingCol+1] != '|')
                            tachyonMatrix[workingRow][workingCol+1] = '+';
                    }
                    else if (tachyonMatrix[workingRow][workingCol] == '^' &&
                             tachyonMatrix[workingRow - 1][workingCol] == '.')
                    {
                        tachyonMatrix[workingRow][workingCol] = '.';
                    }
                }

                for (var workingCol = 0; workingCol < tachyonMatrix[workingRow].Length; workingCol++)
                {
                    if (tachyonMatrix[workingRow][workingCol] is '+' or '|')
                    {
                        tachyonMatrix[workingRow + 1][workingCol] = '|';
                    }
                }
            }
            
            Debug.Log(DumpMatrix(tachyonMatrix));
            
            return splits;
        }
        private int AnalyseTachyonMatrixForWorlds()
        {
            var sourceCol = SourceCol();
            if (sourceCol < 0) return sourceCol;

            tachyonMatrix[1][sourceCol] = '|';
            var splits = 0;

            for (var workingRow = 2; workingRow < tachyonMatrix.Length; workingRow += 2)
            {
                for (var workingCol = 0; workingCol < tachyonMatrix[workingRow].Length; workingCol++)
                {
                    if (tachyonMatrix[workingRow][workingCol] == '.' &&
                        tachyonMatrix[workingRow - 1][workingCol] == '|')
                    {
                        tachyonMatrix[workingRow][workingCol] = '|';
                    }
                    else if (tachyonMatrix[workingRow][workingCol] == '^' &&
                        tachyonMatrix[workingRow - 1][workingCol] == '|')
                    {
                        splits++;
                        
                        tachyonMatrix[workingRow][workingCol-1] = '+';
                        
                        tachyonMatrix[workingRow][workingCol+1] = '+';
                    }
                    else if (tachyonMatrix[workingRow][workingCol] == '^' &&
                             tachyonMatrix[workingRow - 1][workingCol] == '.')
                    {
                        tachyonMatrix[workingRow][workingCol] = '.';
                    }
                }

                for (var workingCol = 0; workingCol < tachyonMatrix[workingRow].Length; workingCol++)
                {
                    if (tachyonMatrix[workingRow][workingCol] is '+' or '|')
                    {
                        tachyonMatrix[workingRow + 1][workingCol] = '|';
                    }
                }
            }
            
            Debug.Log(DumpMatrix(tachyonMatrix));
            
            return splits;
        }
        private string DumpMatrix(char[][] matrix)
        {
            var builder = new StringBuilder();
            foreach (var line in matrix)
            {
                foreach (var c in line)
                    builder.Append(c);
                builder.AppendLine();
            }
            return builder.ToString();
        }

        private string DumpCounts(long[][] counts)
        {
            var builder = new StringBuilder();
            foreach (var countLine in counts)
            {
                foreach (var c in countLine)
                {
                    builder.Append(c.ToString("D2")).Append(" ");
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        private int SourceCol()
        {
            for (var i = 0; i < tachyonMatrix[0].Length; i++)
            {
                if (tachyonMatrix[0][i] == 'S') return i;
            }

            return -2;
        }
        
        private bool LoadTachyonLayout(string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }
            
            var text = File.ReadAllText(filename);
            
            var textLines = text.Split("\r\n");
            
            tachyonMatrix = new char[textLines.Length][];
            for (var y = 0; y < textLines.Length; y++)
            {
                tachyonMatrix[y] = textLines[y].ToCharArray();
            }

            return true;
        }

        private void InitialisePathCounts()
        {
            
            var height = tachyonMatrix.Length;
            var width = tachyonMatrix[0].Length;
            
            pathCounts = new long[height][];
            
            for (var y =0; y < pathCounts.Length; y++)
            {
                pathCounts[y] = new long[width];
                for (var x =0; x < width; x++)
                {
                    pathCounts[y][x] = 0L;
                }
            }
        }
    }
}