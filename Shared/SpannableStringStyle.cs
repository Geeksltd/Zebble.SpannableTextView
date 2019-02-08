namespace Zebble
{
    using System.Collections.Generic;

    public class SpannableStringStyle
    {
        public string InnerText { get; set; }
        public SpannableStringRange Range { get; set; }
        public SpannableStringTypes Type { get; set; }
        public Dictionary<SpannableStringParameterTypes, string> Parameters { get; set; }

        public List<SpannableStringStyle> Children { get; set; } = new List<SpannableStringStyle>();
    }
}