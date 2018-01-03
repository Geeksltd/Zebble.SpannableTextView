namespace Zebble
{
    using System.Threading.Tasks;
    using Zebble.AndroidOS;

    internal class SpanableTextViewRenderer : INativeRenderer
    {
        SpannableTextView View;
        SpannableAndroidTextView Result;

        async Task<Android.Views.View> INativeRenderer.Render(Renderer render)
        {
            View = (SpannableTextView)render.View;
            Result = new SpannableAndroidTextView(View);

            return Result;
        }

        public void Dispose() => Result.Dispose();
    }
}