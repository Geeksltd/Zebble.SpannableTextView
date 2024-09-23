namespace Zebble
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Zebble.WinUI;

    internal class SpannableTextViewRenderer : INativeRenderer
    {
        SpannableTextView View;
        WinUISpannableTextBlock Result;

        Task<FrameworkElement> INativeRenderer.Render(Renderer renderer)
        {
            View = (SpannableTextView)renderer.View;
            Result = new WinUISpannableTextBlock(renderer);

            return Result.Render(renderer);
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}