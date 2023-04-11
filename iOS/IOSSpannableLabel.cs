using Foundation;

namespace Zebble.IOS
{
    public class IOSSpannableLabel : IosLabel
    {
        SpannableTextView View;

        public IOSSpannableLabel(SpannableTextView view) : base(view)
        {
            View = view;

            view.SpannableTextChanged.HandleActionOn(Thread.UI, RenderSpannableText);

            UserInteractionEnabled = true;

            RenderSpannableText();
        }

        void RenderSpannableText()
        {
            if (View.ParsedText == null) return;

            foreach (var spannableStyle in View.ParsedText)
            {
                spannableStyle.RenderSpannableStringStyle(View, AttributedString);

                if (spannableStyle.Children.Count > 0)
                    RenderChildSpannableStyle(AttributedString, spannableStyle);
            }

            Label.AttributedText = AttributedString;
            View.LineHeightChanged.Raise();
        }

        void RenderChildSpannableStyle(NSMutableAttributedString text, SpannableStringStyle parentStyle)
        {
            foreach (var style in parentStyle.Children)
            {
                style.RenderSpannableStringStyle(View, text);

                if (style.Children.Count > 0) RenderChildSpannableStyle(text, style);
            }
        }
    }
}