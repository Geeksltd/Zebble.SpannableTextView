namespace Zebble
{
    public class SpannableStringRange
    {
        public int Start { get; set; }
        public int End => Start + Length;
        public int Length { get; set; }

        public SpannableStringRange(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public override string ToString() => $"[{Start}, {End} ({Length})]";
    }
}