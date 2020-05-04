namespace Zebble
{
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Zebble.UWP;

    internal class SpanableTextViewRenderer : INativeRenderer
    {
        SpannableTextView View;
        UWPSpannableTextBlock Result;

        Task<FrameworkElement> INativeRenderer.Render(Renderer renderer)
        {
            View = (SpannableTextView)renderer.View;
            Result = new UWPSpannableTextBlock(renderer);

            return Result.Render(renderer);
        }

        public void Dispose() { }
    }
}