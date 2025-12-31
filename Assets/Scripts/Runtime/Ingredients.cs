using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;

namespace AdventOfCode
{
    public class Ingredients
    {
        private List<BigInteger> ingredients = new List<BigInteger>();
        private List<Tuple<BigInteger, BigInteger>> goodIngredientRanges = new List<Tuple<BigInteger, BigInteger>>();
        public Ingredients()
        {
            
        }

        public BigInteger LoadAndCheckFreshFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return -1;
            }

            LoadFile(filename);
            var mergeHappened = false;

            do
            {
                mergeHappened = false;
                
                for (var i = 0; i < goodIngredientRanges.Count; i++)
                {
                    var (start, end) = goodIngredientRanges[i];

                    for (var j = goodIngredientRanges.Count - 1; j > i; j--)
                    {
                        var (checkStart, checkEnd) = goodIngredientRanges[j];
                        
                        if (checkEnd < start || checkStart > end) continue;
                        
                        var mergeStart = checkStart < start ? checkStart : start;
                        var mergeEnd = checkEnd > end ? checkEnd : end;
                        start = mergeStart;
                        end = mergeEnd;

                        goodIngredientRanges[i] = Tuple.Create(mergeStart, mergeEnd);
                        
                        goodIngredientRanges.RemoveAt(j);
                        
                        mergeHappened = true;
                    }
                }
            } while (mergeHappened);

            BigInteger foundFresh = 0; 
            foreach (var ingredientRange in goodIngredientRanges)
            {
                var (start, end) = ingredientRange;
                var rangeCount = end - start + 1;
                foundFresh += rangeCount;
            }
            return foundFresh;
        }

        public int LoadAndCheckFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return -1;
            }
            
            LoadFile(filename);
            
            var foundCount = 0;
            foreach (var ingredient in ingredients)
            {
                foreach (var range in goodIngredientRanges)
                {
                    var (start, end) = range;
                    if (ingredient >= start && ingredient <= end)
                    {
                        foundCount++;
                        break;
                    }
                }
            }
            return foundCount;
        }

        private void LoadFile(string filename)
        {
            var text = File.ReadAllText(filename);
            
            var textLines = text.Split("\r\n");

            foreach (var textLine in textLines)
            {
                if (string.IsNullOrEmpty(textLine))
                {
                    // Skip empty lines
                    continue;
                }

                if (textLine.Contains("-"))
                {
                    var rangeValues =  textLine.Split("-");
                    goodIngredientRanges.Add(Tuple.Create(BigInteger.Parse(rangeValues[0]), BigInteger.Parse(rangeValues[1])));
                }
                else
                {
                    ingredients.Add(BigInteger.Parse(textLine));
                }
            }
        }
    }
}