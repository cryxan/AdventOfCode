using System.IO;
using System.Numerics;

namespace AdventOfCode
{
    public class BatteryBank
    {
        const int batteriesToUse = 12;
        public BatteryBank()
        {
            
        }

        public BigInteger JoltagesFromFile(string filename)
        {
            if (!File.Exists(filename)) return 0;
            
            var text = File.ReadAllText(filename);

            var joltageArray = text.Split("\r\n");

            return JoltageFromArray(joltageArray);
        }

        public BigInteger JoltageFromArray(string[] joltages)
        {
            BigInteger joltage = 0;
            foreach (var joltageString in joltages)
            {
                joltage += FindJoltage(joltageString);
            }
            return joltage;
        }
        public BigInteger FindJoltage(string joltageBank)
        {
            var intJoltages = JoltageStringToArray(joltageBank);
            
            if (intJoltages == null || joltageBank.Length < batteriesToUse) return 0;

            
            if (joltageBank.Length == batteriesToUse) return BigInteger.Parse(joltageBank);

            var batteriesFound = 0;
            var startIndex = 0;
            var foundString = "";
            while (batteriesFound < batteriesToUse)
            {
                var foundVoltage = 0;
                var foundIndex = -1;
                var searchLimit = joltageBank.Length - (batteriesToUse-batteriesFound);
                for (var i = startIndex; i <= searchLimit; i++)
                {
                    if (intJoltages[i] <= foundVoltage) continue;
                    
                    foundVoltage = intJoltages[i];
                    foundIndex = i;
                }
                startIndex = foundIndex+1;
                foundString += foundVoltage.ToString();
                batteriesFound++;
            }

            return BigInteger.Parse(foundString);
        }

        public int[] JoltageStringToArray(string joltageBank)
        {
            if (string.IsNullOrEmpty(joltageBank)) return null;
                
            var batteries = new int[joltageBank.Length];
            
            for (var i = 0; i < joltageBank.Length; i++)
            {
                var joltageChar = joltageBank[i];
                if (char.IsDigit(joltageChar))
                {
                    batteries[i] = joltageBank[i] - '0';
                }
                else
                {
                    return null;
                }
            }
            return batteries;
        }
    }
}