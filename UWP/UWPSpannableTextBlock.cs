using Windows.UI.Xaml.Documents;

namespace Zebble.UWP
{
    public class UWPSpannableTextBlock : UWPTextBlock
    {
        SpannableTextView View;

        public UWPSpannableTextBlock(Renderer renderer) : base(renderer)
        {
            View = renderer.View as SpannableTextView;

            View.SpannableTextChanged.HandleActionOn(Thread.UI, RenderSpannableText);

            RenderSpannableText();
        }

        void RenderSpannableText()
        {
            if (View.ParsedText == null) return;

            Result.Text = "";
            foreach (var spannableStyle in View.ParsedText)
            {
                var element = spannableStyle.RenderSpannableStringStyle();

                if (spannableStyle.Children.Count > 0)
                    RenderChildSpannableStyle(element, spannableStyle);

                Result.Inlines.Add(element);
            }

            View.LineHeightChanged.Raise();
        }

        void RenderChildSpannableStyle(Span parent, SpannableStringStyle parentStyle)
        {
            foreach (var style in parentStyle.Children)
            {
                var element = style.RenderSpannableStringStyle();
                if (style.Children.Count > 0) RenderChildSpannableStyle(element, style);

                parent.Inlines.Add(element);
            }
        }

    }
}
