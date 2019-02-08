namespace Zebble
{
    public class SpannableStringRange
    {
        public int Start { get; set; }
        public int Length { get; set; }

        public SpannableStringRange(int strat, int length)
        {
            Start = strat;
            Length = length;
        }

        public override string ToString() => $"[{Start},{Length}]";
    }
}