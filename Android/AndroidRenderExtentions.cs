namespace Zebble
{
    using Android.Graphics;
    using System;

    public static class AndroidRenderExtentions
    {
        internal static void RenderSpannableStringStyle(this SpannableStringStyle style, Android.Text.SpannableString text)
        {
            switch (style.Type)
            {
                case SpannableStringTypes.B:
                case SpannableStringTypes.Bold:
                    text.SetSpan(new Android.Text.Style.StyleSpan(TypefaceStyle.Bold), style.Range.Start, style.Range.Length, Android.Text.SpanTypes.ExclusiveExclusive);
                    break;
                case SpannableStringTypes.I:
                case SpannableStringTypes.Italic:
                    text.SetSpan(new Android.Text.Style.StyleSpan(TypefaceStyle.Italic), style.Range.Start, style.Range.Length, Android.Text.SpanTypes.ExclusiveExclusive);
                    break;
                case SpannableStringTypes.Font:
                    foreach (var parameter in style.Parameters)
                    {
                        switch (parameter.Key)
                        {
                            case SpannableStringParameterTypes.Size:
                                if (float.TryParse(parameter.Value, out float fontSize))
                                {
                                    text.SetSpan(new Android.Text.Style.AbsoluteSizeSpan(Device.Scale.ToDevice(fontSize), true),
                                        style.Range.Start, style.Range.Length, Android.Text.SpanTypes.ExclusiveExclusive);
                                }

                                break;
                            case SpannableStringParameterTypes.Color:
                                text.SetSpan(new Android.Text.Style.ForegroundColorSpan(Zebble.Color.Parse(parameter.Value).Render()),
                                    style.Range.Start, style.Range.Length, Android.Text.SpanTypes.ExclusiveExclusive);
                                break;
                            case SpannableStringParameterTypes.Face:
                                // TODO implement font faces.
                                break;
                            default: break;
                        }
                    }

                    break;
                default: break;
            }
        }
    }
}