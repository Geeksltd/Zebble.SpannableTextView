namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using Olive;

    public class SpannableTextView : TextView, IRenderedBy<SpanableTextViewRenderer>
    {
        string spannableText = string.Empty;
        internal List<SpannableStringStyle> ParsedText;
        public readonly AsyncEvent SpannableTextChanged = new AsyncEvent();

        public readonly AsyncEvent<EventArgs<string>> LinkTapped = new();

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

        public override void Dispose()
        {
            SpannableTextChanged?.Dispose();
            ParsedText?.Clear();
            ParsedText = null;
            base.Dispose();
        }
    }
}