namespace Zebble.IOS
{
    using Foundation;
    using Olive;
    using System;
    using UIKit;

    public static class IOSRenderExtentions
    {
        internal static void RenderSpannableStringStyle(this SpannableStringStyle style, SpannableTextView view, NSMutableAttributedString attributedString)
        {
            try
            {
                var range = new NSRange(style.Range.Start, style.Range.Length);
                switch (style.Type)
                {
                    case SpannableStringTypes.B:
                    case SpannableStringTypes.Bold:
                        attributedString.AddAttribute(UIStringAttributeKey.Font, UIFont.BoldSystemFontOfSize(view.Font.Size), range);
                        break;
                    case SpannableStringTypes.I:
                    case SpannableStringTypes.Italic:
                        attributedString.AddAttribute(UIStringAttributeKey.Font, UIFont.ItalicSystemFontOfSize(view.Font.Size), range);
                        break;
                    case SpannableStringTypes.Font:
                        foreach (var parameter in style.Parameters)
                        {
                            switch (parameter.Key)
                            {
                                case SpannableStringParameterTypes.Size:
                                    if (!float.TryParse(parameter.Value, out float fontSize)) break;
                                    var font = UIFont.FromName(view.Font.Name, fontSize);
                                    if (font == null) break;
                                    attributedString.AddAttribute(UIStringAttributeKey.Font, font, range);
                                    break;
                                case SpannableStringParameterTypes.Color:
                                    attributedString.AddAttribute(UIStringAttributeKey.ForegroundColor, Color.Parse(parameter.Value).Render(), range);
                                    break;
                                // TODO implement font faces.
                                // case SpannableStringParameterTypes.Face: break;
                                default: break;
                            }
                        }
                        break;
                    case SpannableStringTypes.A:
                        attributedString.AddAttribute(
                            UIStringAttributeKey.Link,
                            NSUrl.FromString($"https://www.test.com/?value={style.InnerText}"),
                            range);

                        UIRuntime.OnOpenUrl.Handle(url =>
                        {
                            view.LinkTapped.Raise(new EventArgs<string>(url.ToString()));
                        });

                        UIRuntime.OnOpenUrlWithOptions.Handle(url =>
                        {
                            view.LinkTapped.Raise(new EventArgs<string>(url.ToString()));
                        });
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                Log.For(typeof(IOSRenderExtentions)).Error(ex, $"Id: {view?.Id.Or("null")}, Text: {view?.Text.Or("null")}, Range: {style?.Range}");
            }
        }
    }
}