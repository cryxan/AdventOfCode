using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using UnityEngine;

namespace AdventOfCode
{
    public class MachineSwitches
    {
        public List<Machine> machines = new List<Machine>();
        public MachineSwitches()
        {
            
        }

        public int LoadAndAnalyseSwitches(string filename)
        {
            if (!LoadFile(filename)) return -1;

            var result = 0;
            foreach (var machine in machines)
            {
                result += machine.FindPressCount(10);
            }
            return result;
        }
        
        public int LoadAndAnalyseJoltageSwitches(string filename)
        {
            if (!LoadFile(filename)) return -1;

            var result = 0;
            foreach (var machine in machines)
            {
                var presses = machine.FindJoltagePressCount();
                result += presses;
            }
            return result;
        }

        public bool LoadFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }
            
            var text = File.ReadAllText(filename);
            var textLines = text.Split("\r\n");
            foreach (var textLine in textLines)
            {
                machines.Add(new Machine(textLine));
            }

            DumpMachines();

          
            return true;
        }

        public void DumpMachines()
        {
            var machineNumber = 0;
            var maxJoltageValues = machines.Select(machine => machine.joltages.Count).Prepend(0).Max();
            Debug.Log($"{maxJoltageValues} Joltage values found");
            
            foreach (var machine in machines)
            {
                Debug.Log($"{machineNumber++} {machine}");
            }
        }
    }
}