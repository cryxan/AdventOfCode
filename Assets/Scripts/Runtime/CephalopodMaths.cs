using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Android;

namespace AdventOfCode
{
    public class CephalopodMaths
    {
        private char[][] mathMatrix;

        public CephalopodMaths()
        {
        }

        public BigInteger LoadCephalopodMathsProblem(string filename)
        {
            if (!File.Exists(filename))
            {
                return -1;
            }
            
            var text = File.ReadAllText(filename);
            
            var textLines = text.Split("\r\n");

            mathMatrix = new char[textLines.Length][];
            for (var y = 0; y < textLines.Length; y++)
            {
                mathMatrix[y] = textLines[y].ToCharArray();
            }

            var readColumn = 0;
            var numberList = new List<BigInteger>();
            var operation = ' ';
            var total = BigInteger.Parse("0");
            
            while (readColumn < mathMatrix[0].Length)
            {
                if (ColumnIsEmpty(readColumn))
                {
                    total += DoOperation(operation, numberList);
                    readColumn++;
                    operation = ' ';
                    numberList = new List<BigInteger>();
                    continue;
                }

                numberList.Add(GetNumber(readColumn));
                if (mathMatrix[^1][readColumn] is '+' or '*')
                {
                    operation = mathMatrix[^1][readColumn];
                }
                readColumn++;
            }
            total += DoOperation(operation, numberList);

            return total;
        }

        private BigInteger DoOperation(char operation, List<BigInteger> numberList)
        {
            switch (operation)
            {
                case '+':
                    return DoAddition(numberList);
                case '*':
                    return DoMultiplication(numberList);
            }

            return 0;
        }

        private BigInteger DoMultiplication(List<BigInteger> numberList)
        {
            var result = BigInteger.Parse("1");
            foreach (var number in numberList)
            {
                result *= number;
            }
            return result;
        }

        private BigInteger DoAddition(List<BigInteger> numberList)
        {
            var result = BigInteger.Parse("0");
            foreach (var number in numberList)
            {
                result += number;
            }
            return result;
        }

        private BigInteger GetNumber(int col)
        {
            var numberStr = "";
            foreach (var mathLine in mathMatrix)
            {
                if (char.IsDigit(mathLine[col])) numberStr += mathLine[col];
            }
            return BigInteger.Parse(numberStr);
        }
        
        private bool ColumnIsEmpty(int col)
        {
            foreach (var mathLine in mathMatrix)
            {
                if (mathLine[col] != ' ') return false;
            }
            return true;
        }
    }
}