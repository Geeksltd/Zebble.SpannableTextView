namespace Zebble.AndroidOS
{
    public class SpannableAndroidTextView : AndroidTextView
    {
        SpannableTextView View;

        public SpannableAndroidTextView(SpannableTextView view) : base(view)
        {
            View = view;

            view.SpannableTextChanged.HandleActionOn(Thread.UI, RenderSpannableText);
            RenderSpannableText();
        }

        void RenderSpannableText()
        {
            if (View?.ParsedText == null) return;

            var spannableText = new Android.Text.SpannableString(View.Text);
            foreach (var spannableStyle in View.ParsedText)
            {
                spannableStyle.RenderSpannableStringStyle(spannableText, View);

                if (spannableStyle.Children.Count > 0)
                    RenderChildSpannableStyle(spannableText, spannableStyle);
            }

            SetText(spannableText, BufferType.Spannable);
            View.LineHeightChanged.Raise();
        }

        void RenderChildSpannableStyle(Android.Text.SpannableString text, SpannableStringStyle parentStyle)
        {
            foreach (var style in parentStyle.Children)
            {
                style.RenderSpannableStringStyle(text, View);

                if (style.Children.Count > 0) RenderChildSpannableStyle(text, style);
            }
        }

        protected override void Dispose(bool disposing)
        {
            View = null;
            base.Dispose(disposing);
        }
    }
}