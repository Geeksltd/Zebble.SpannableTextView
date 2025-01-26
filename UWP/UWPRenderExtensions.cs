namespace Zebble.UWP
{
    using Windows.UI.Text;
    using doc = Windows.UI.Xaml.Documents;
    using media = Windows.UI.Xaml.Media;
    using Olive;

    public static class UWPRenderExtensions
    {
        internal static doc.Span RenderSpannableStringStyle(this SpannableStringStyle style)
        {
            doc.Span result;
            switch (style.Type)
            {
                case SpannableStringTypes.B:
                case SpannableStringTypes.Bold:
                    result = new doc.Span { FontWeight = FontWeights.Bold };
                    break;
                case SpannableStringTypes.I:
                case SpannableStringTypes.Italic:
                    result = new doc.Span { FontStyle = FontStyle.Italic };
                    break;
                case SpannableStringTypes.Font:
                    var span = new doc.Span();
                    foreach (var parameter in style.Parameters)
                    {
                        switch (parameter.Key)
                        {
                            case SpannableStringParameterTypes.Size:
                                if (double.TryParse(parameter.Value, out double fontSize))
                                    span.FontSize = fontSize;
                                break;
                            case SpannableStringParameterTypes.Color:
                                span.Foreground = new media.SolidColorBrush(Color.Parse(parameter.Value).Render());
                                break;
                            case SpannableStringParameterTypes.Face:
                                // TODO implement font faces.
                                break;
                            default: break;
                        }
                    }

                    result = span;
                    break;
                default:
                    result = new doc.Span();
                    break;
            }

            if (style.InnerText.HasValue()) result.Inlines.Add(new doc.Run { Text = style.InnerText });
            return result;
        }
    }
}