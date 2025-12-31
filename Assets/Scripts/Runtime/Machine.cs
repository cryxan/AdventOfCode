using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Machine
    {
        public string config;
        public uint lightTarget;
        
        public List<uint> buttons = new List<uint>();
        
        public List<int> joltages = new List<int>();
        public List<JoltageSet> buttonIncrements = new List<JoltageSet>();

        public JoltageSet joltageSet;
        public int lightCount;


        public Machine(string machineConfig)
        {
            config =  machineConfig;

            var configLines = machineConfig.Split(' ');
            foreach (var configLine in configLines)
            {
                switch (configLine[0])
                {
                    case '[':
                        SetupLights(configLine);
                        break;
                    case '(':
                        AddButton(configLine);
                        break;
                    case '{':
                        SetupJoltages(configLine);
                        break;
                }
            }
            joltageSet = new JoltageSet(joltages);
            buttonIncrements = buttonIncrements.OrderByDescending( x => x.sortValue ).ToList();
        }
        
        public int FindPressCount(int pressLimit)
        {
            var pressCount = 0;
            var pressSet = new List<Press>();
            pressSet.Add(new Press(0U,0U));
            while (pressCount < pressLimit)
            {
                pressCount++;
                
                var nextPressSet = new List<Press>();
                foreach (var press in pressSet)
                {
                    foreach (var button in buttons)
                    {
                        if (press.previousPress == button) continue;
                        
                        var newState = press.currentState ^ button;
                        
                        if (newState == lightTarget) return pressCount;
                        
                        nextPressSet.Add(new Press(newState, button));
                    }
                }
                pressSet = nextPressSet;
            }

            return 0;
        }

        public int FindJoltagePressCount()
        {
            var seedJoltageSet = new JoltageSet(lightCount);
            return FindJoltagePressCount(seedJoltageSet, 0);
        }

        public int FindJoltagePressCount(JoltageSet joltageSet, int currenPress)
        {
            var tempPresses = currenPress + 1;
            


            return -1;
        }

        private void SetupJoltages(string configLine)
        {
            var configTmp = configLine.Replace("{", "").Replace("}", "").Split(',');
            joltages = configTmp.Select(index => int.Parse((string)index)).ToList();
        }

        private void AddButton(string configLine)
        {
            var configTmp = configLine.Replace("(", "").Replace(")", "").Split(',');
            var toggleList = configTmp.Select(index => int.Parse(index)).ToList();
            uint toggleBits = 0;

            var buttonIncrement = new JoltageSet(lightCount);

            var sortIndex = 0;
            
            foreach (var toggle in toggleList)
            {
                toggleBits |= 1U << toggle;
                buttonIncrement.joltageArray[toggle] = (char)1;
                sortIndex++;
            }
            buttonIncrement.sortValue = sortIndex;
            buttonIncrements.Add(buttonIncrement);
            buttons.Add(toggleBits);
        }

        private void SetupLights(string configLine)
        {
            var lineState = configLine.Replace("[", "").Replace("]", "");
            lightCount = lineState.Length;
            lightTarget = 0;
            for (var i = 0; i < lineState.Length; i++)
            {
                if (lineState[i] == '#') lightTarget |= 1U << i;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Config = {config}").AppendLine();
            sb.Append($"LightTarget = {BinaryString(lightTarget)}").AppendLine();
            sb.AppendLine("Buttons = {");
            var separator = "";
            foreach (var button in buttons)
            {
                sb.Append($"{separator}{BinaryString(button)}");
                separator = ",";
            }

            sb.Append("}").AppendLine();

            sb.Append("JoltageButtons = {");
            sb.Append("}").AppendLine();
            sb.Append("Joltages = {");
            separator = "";
            foreach (var joltage in joltages)
            {
                sb.Append($"{separator}{joltage}");
                separator = ",";
            }
            sb.Append("}").AppendLine();
            sb.Append($"JoltageSet = {joltageSet}").AppendLine();

            return sb.ToString();
        }

        private string BinaryString(uint value)
        {
            var sb = new StringBuilder();
            
            for (var digit = 31; digit >= 0; digit--)
            {
                var mask = 1U << digit;
                sb.Append((value & mask) != 0 ? '1' : '0');
            }
            return sb.ToString();
        }


    }
}