namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SpannableTextView : TextView, IRenderedBy<SpanableTextViewRenderer>
    {
        string spannableText = string.Empty;
        internal List<SpannableStringStyle> ParsedText;
        public readonly AsyncEvent SpannableTextChanged = new AsyncEvent();

        public string SpannableText
        {
            get => spannableText;
            set
            {
                if (spannableText == value && Text == value) return;
                spannableText = value.OrEmpty();
                var spannableString = new SpannableString(spannableText);
                ParsedText = spannableString.ParseText();
                Text = spannableString.Stripedtext;
                SpannableTextChanged.Raise();
            }
        }

        float CalculateSpannableTextAutoHeight()
        {
            float textViewHeight = 0;
            var textWidth = Font.GetTextWidth("T");
            var textViewWidth = ActualWidth - (Padding.Horizontal() + Margin.Horizontal() + Border.TotalHorizontal);
            var numberOfCharachterInLine = textViewWidth / textWidth;
            var linesCount = (int)Font.GetTextWidth(text) / textViewWidth;
            var allStyles = ParsedText.Flatten((style, except) => style.Children.Except(except));

            for (var i = 1; i <= linesCount; i++)
            {
                var maximumFontSize = allStyles
                    .Where(s => s.Range.Start <= (numberOfCharachterInLine.Round(0) * i) && s.Parameters != null)
                    .Select(f => Convert.ToDouble(f.Parameters.SingleOrDefault(p => p.Key == SpannableStringParameterTypes.Size).Value))
                    .DefaultIfEmpty(0)
                    .Max(f => f);
                var lineHeight = new Font(Font.Name, maximumFontSize == 0 ? Font.EffectiveSize : (float)maximumFontSize).GetLineHeight();

                if (LineHeight.HasValue) lineHeight += LineHeight.Value;
                textViewHeight += lineHeight;
            }

            return textViewHeight;
        }
    }
}