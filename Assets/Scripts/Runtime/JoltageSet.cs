using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public class JoltageSet
    {
        public char[] joltageArray;
        public int sortValue;

        public JoltageSet(int size)
        {
            joltageArray = new char[size];
        }
        public JoltageSet(List<int> joltages)
        {
            joltageArray = new char[joltages.Count];
            for (int i = 0; i < joltages.Count; i++)
            {
                joltageArray[i] = (char)joltages[i];
            }
        }

        public JoltageSet(JoltageSet other)
        {
            joltageArray = new char[other.joltageArray.Length];
            for (int i = 0; i < joltageArray.Length; i++)
            {
                joltageArray[i] = other.joltageArray[i];
            }
        }

        public bool Add(JoltageSet other, JoltageSet limits)
        {
            if (other.joltageArray.Length != joltageArray.Length)
            {
                return false;
            }

            for (int i = 0; i < joltageArray.Length; i++)
            {
                if ( (joltageArray[i]+other.joltageArray[i]) > limits.joltageArray[i])  return false;
            }

            for (int i = 0; i < joltageArray.Length; i++)
            {
                joltageArray[i] += other.joltageArray[i];
            }

            return true;
        }

        public bool Subtract(JoltageSet other)
        {
            if (other.joltageArray.Length != joltageArray.Length)
            {
                return false;
            }
            
            for (int i = 0; i < joltageArray.Length; i++)
            {
                joltageArray[i] -= other.joltageArray[i];
            }
            return true;
        }

        public bool Equals(JoltageSet other)
        {
            for (var i = 0; i < joltageArray.Length ; i++)
            {
                if (joltageArray[i] != other.joltageArray[i]) return false;
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            var separator = "";
            foreach (var joltage in joltageArray)
            {
                sb.Append($"{separator}{(int)joltage}");
                separator = ",";
            }
            sb.Append("}").AppendLine();
            return sb.ToString();
        }
    }
}