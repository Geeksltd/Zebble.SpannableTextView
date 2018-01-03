namespace Zebble
{
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Zebble.UWP;

    internal class SpanableTextViewRenderer : INativeRenderer
    {
        SpannableTextView View;
        UWPSpannableTextBlock Result;

        async Task<FrameworkElement> INativeRenderer.Render(Renderer renderer)
        {
            View = (SpannableTextView)renderer.View;
            Result = new UWPSpannableTextBlock(renderer);

            return Result.Render();
        }

        public void Dispose() { }
    }
}