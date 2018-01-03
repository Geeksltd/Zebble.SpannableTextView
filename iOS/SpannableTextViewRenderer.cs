namespace Zebble
{
    using System.Threading.Tasks;
    using UIKit;
    using Zebble.IOS;

    internal class SpanableTextViewRenderer : INativeRenderer
    {
        SpannableTextView View;
        IOSSpannableLabel Result;

        async Task<UIView> INativeRenderer.Render(Renderer renderer)
        {
            View = (SpannableTextView)renderer.View;
            Result = new IOSSpannableLabel(View);

            return Result;
        }

        public void Dispose() => Result.Dispose();
    }
}