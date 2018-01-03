namespace Zebble
{
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SpannableTextView : TextView, IRenderedBy<SpanableTextViewRenderer>
    {
        string spannableText;
        internal List<SpannableStringStyle> ParsedText;
        public readonly AsyncEvent SpannableTextChanged = new AsyncEvent();

        public string SpannableText
        {
            get => spannableText;
            set
            {
                if (spannableText == value) return;
                spannableText = value;
                var spannableString = new SpannableString(spannableText);
                ParsedText = spannableString.ParseText();
                Text = spannableString.Stripedtext;

                SpannableTextChanged.Raise();
            }
        }

        protected override float CalculateAutoHeight()
        {
            float currentHeight;
            currentHeight = base.CalculateAutoHeight();

            if (ParsedText != null && ParsedText.Count > 0) currentHeight = CalculateSpannableTextAutoHeight();

            return currentHeight;
        }

        float CalculateSpannableTextAutoHeight()
        {
            float textViewHeight = 0;
            var textWidth = Font.GetTextWidth("T");
            var textViewWidth = ActualWidth - (Padding.Horizontal() + Margin.Horizontal() + Border.TotalHorizontal);
            var numberOfCharachterInLine = textViewWidth / textWidth;
            var linesCount = (int)Font.GetTextWidth(text) / textViewWidth;
            var allStyles = ParsedText.Flatten((style, except) => style.Children.Except(except));

            for (int i = 1; i <= linesCount; i++)
            {
                var maximumFontSize = allStyles.Where(s => s.Range.Start <= (numberOfCharachterInLine.Round(0) * i) && s.Parameters != null)
                    .Select(f => f.Parameters.SingleOrDefault(p => p.Key == SpannableStringParameterTypes.Size)).Max(f => Convert.ToDouble(f.Value));
                var lineHeight = new Font(Font.Name, maximumFontSize == 0 ? Font.EffectiveSize : (float)maximumFontSize).GetLineHeight();

                if (LineHeight.HasValue) lineHeight += LineHeight.Value;
                textViewHeight += lineHeight;
            }

            return textViewHeight;
        }
    }

    public class SpannableString
    {
        string InputText;
        public string Stripedtext;
        readonly List<SpannableStringStyle> ParsedItems = new List<SpannableStringStyle>();

        List<string> AcceptedTags => Enum.GetNames(typeof(SpannableStringTypes)).ToList();

        public SpannableString(string inputText) { InputText = inputText; }

        public List<SpannableStringStyle> ParseText()
        {
            Stripedtext = StripHTML(InputText);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(InputText);

            ParseTags(htmlDocument.DocumentNode.ChildNodes);

            return ParsedItems;
        }

        Dictionary<SpannableStringParameterTypes, string> ExtractFontParameters(HtmlAttributeCollection attributes)
        {
            var result = new Dictionary<SpannableStringParameterTypes, string>();
            foreach (var attribute in attributes)
            {
                var isCorrectkey = Enum.TryParse(attribute.Name.CapitaliseFirstLetters(), out SpannableStringParameterTypes key);
                if (!isCorrectkey) continue;
                result.Add(key, attribute.Value);
            }

            return result;
        }

        SpannableStringStyle ExtractStyle(HtmlNode node, SpannableStringRange range)
        {
            var isCorrecttype = Enum.TryParse(node.Name.CapitaliseFirstLetters(), out SpannableStringTypes type);
            return new SpannableStringStyle
            {
                InnerText = node.InnerText,
                Parameters = node.Name.ToLower() == "font" ? ExtractFontParameters(node.Attributes) : null,
                Range = range,
                Type = isCorrecttype ? type : SpannableStringTypes.PlainText
            };
        }

        void ParseTags(HtmlNodeCollection children, SpannableStringStyle parent = null)
        {
            foreach (var node in children)
            {
                var startIndex = Stripedtext.IndexOf(node.InnerText);
                var range = new SpannableStringRange(startIndex, startIndex + node.InnerText.Length);
                var style = ExtractStyle(node, range);

                if (node.ChildNodes.Count > 0 && node.ChildNodes.Count(n => AcceptedTags.Any(at => at.ToLower() == n.Name)) > 0)
                {
                    style.InnerText = "";
                    ParseTags(node.ChildNodes, style);
                }

                if (parent == null) ParsedItems.Add(style);
                else parent.Children.Add(style);
            }
        }

        string StripHTML(string input) => Regex.Replace(input, "<.*?>", string.Empty);
    }

    public class SpannableStringStyle
    {
        public string InnerText { get; set; }
        public SpannableStringRange Range { get; set; }
        public SpannableStringTypes Type { get; set; }
        public Dictionary<SpannableStringParameterTypes, string> Parameters { get; set; }

        public List<SpannableStringStyle> Children { get; set; } = new List<SpannableStringStyle>();
    }

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

    public enum SpannableStringTypes
    {
        Bold,
        B,
        Italic,
        I,
        Font,
        PlainText
    }

    public enum SpannableStringParameterTypes
    {
        Size,
        Color,
        Face
    }
}