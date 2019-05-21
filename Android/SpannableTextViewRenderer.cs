namespace Zebble
{
    using Android.Runtime;
    using System.Threading.Tasks;
    using Zebble.AndroidOS;

    internal class SpanableTextViewRenderer : INativeRenderer
    {
        SpannableTextView View;
        SpannableAndroidTextView Result;

        [Preserve]
        public SpanableTextViewRenderer() { }

        public Task<Android.Views.View> Render(Renderer render)
        {
            View = (SpannableTextView)render.View;
            Result = new SpannableAndroidTextView(View);

            return Task.FromResult((Android.Views.View)Result);
        }

        public void Dispose()
        {
            Result?.Dispose();
            Result = null;
        }
    }
}