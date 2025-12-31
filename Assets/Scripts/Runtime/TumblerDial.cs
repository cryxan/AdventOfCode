

using System.IO;

namespace AdventOfCode
{
    public class TumblerDial
    {
        internal int dialPosition;
        internal int zeroHitCount = 0; 
        public TumblerDial()
        {
            dialPosition = 50;
        }

        public bool DialFile(string fileName, out int ZeroHitCount)
        {
            ZeroHitCount = 0;
            
            if (!File.Exists(fileName)) return false;
            
            var text = File.ReadAllText(fileName);
            var dialcodes = text.Split("\r\n");
            DialSequence(dialcodes);
            ZeroHitCount = this.zeroHitCount;
            return true;
        }
        public int  DialSequence(string[] dialCodes)
        {
            foreach (var dialCode in dialCodes)
            {
                MoveDial(dialCode);
            }
            return zeroHitCount;
        }

        public void MoveDial(string dialCode)
        {
            if (DialChange(dialCode, out var changeValue))
            {
                var changeIncrement = changeValue < 0 ? -1 : 1;

                do
                {
                    dialPosition = (100+dialPosition + changeIncrement) % 100;
                
                    if (dialPosition == 0) zeroHitCount++;
                    
                    changeValue -= changeIncrement;
                    
                } while (changeValue != 0);
                
            }
        }

        internal bool DialChange(string dialCode, out int changeValue)
        {
            changeValue = 0;
            
            if (dialCode.Length < 2 || dialCode[0] != 'L' && dialCode[0] != 'R') return false;

            var directionModifier = 1;
            
            if (dialCode[0] == 'L') directionModifier = -1;
            
            var dialNumber = dialCode.Substring(1);
            
            if (!int.TryParse(dialNumber, out var number)) return false;
            
            if (number <= 0) return false;

            changeValue = number * directionModifier;
            
            return true;
        }
    }

}