namespace AdventOfCode
{
    public class Press
    {
        public uint currentState;
        public uint previousPress;

        public Press(uint currentState, uint previousPress)
        {
            this.currentState = currentState;
            this.previousPress = previousPress;
        }
    }
}