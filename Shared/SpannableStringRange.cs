using Olive;

namespace Zebble
{
    public class SpannableStringRange
    {
        public int Start { get; set; }
        public int End => Start + Length;
        public int Length { get; set; }

        public SpannableStringRange(int start, int length)
        {
            Start = start.LimitMin(0);
            Length = length.LimitMin(0);
        }

        public override string ToString() => $"[{Start}, {End} ({Length})]";
    }
}