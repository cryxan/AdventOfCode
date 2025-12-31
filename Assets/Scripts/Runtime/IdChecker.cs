using System.IO;
using System.Numerics;

namespace AdventOfCode
{
    public class IdChecker
    {
        internal BigInteger checkAccumulator = 0;
        public IdChecker()
        {
            
        }

        public bool CheckIdFile(string filename)
        {
            if (!File.Exists(filename)) return false;
            
            var text = File.ReadAllText(filename);
            
            return CheckIdRangesString(text);
        }
        public bool CheckIdRangesString(string idRanges)
        {
            var idRangeArray = idRanges.Replace("\r","").Replace("\n","").Split(',');
            
            foreach (var idRange in idRangeArray)
            {
                if (ParseRange(idRange, out var start, out var end))
                {
                    for (var idTocheck = start; idTocheck <= end; idTocheck++)
                    {
                        if (!IdValid(idTocheck.ToString()))
                        {
                            checkAccumulator += idTocheck;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public bool ParseRange(string rangeString, out BigInteger start, out BigInteger end)
        {
            start = 0;
            end = 0;

            var rangeValues = rangeString.Split('-');
            if (rangeValues.Length != 2) return false;

            return BigInteger.TryParse(rangeValues[0], out start) && BigInteger.TryParse(rangeValues[1], out end);
        }
        public bool IdValid(string id)
        {
            /*
            if ((id.Length % 2) != 0) return true;
            
            var part1 = id.Substring(0, id.Length / 2);
            var part2 = id.Substring(id.Length / 2);
            
            return  !(part1.Equals(part2));
            */
            
            // Biggest sequence is the length divided by 2
            var startLength = id.Length / 2;
            for (var lengthToCheck = startLength; lengthToCheck > 0; lengthToCheck--)
            {
                var repeatCount = id.Length / lengthToCheck;
                if (id.Length == (repeatCount * lengthToCheck))
                {
                    // Have a possible repeat sequence to check
                    var sequence = id.Substring(0,lengthToCheck);
                    var foundInvalid = true;
                    for (var chrIndex = lengthToCheck; chrIndex < id.Length; chrIndex += lengthToCheck)
                    {
                        var sequenceElement = id.Substring(chrIndex, lengthToCheck);
                        if (sequence != sequenceElement)
                        {
                            foundInvalid = false;
                            break;
                        } 
                    }
                    if (foundInvalid) return false;
                }
            }

            return true;
        }
    }
}