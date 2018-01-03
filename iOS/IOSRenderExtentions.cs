namespace Zebble.IOS
{
    using Foundation;
    using System;
    using UIKit;

    public static class IOSRenderExtentions
    {
        internal static void RenderSpannableStringStyle(this SpannableStringStyle style, TextView view, NSMutableAttributedString attributedString)
        {
            var styleTextLength = Math.Abs(style.Range.Start - style.Range.Length);
            switch (style.Type)
            {
                case SpannableStringTypes.B:
                case SpannableStringTypes.Bold:
                    attributedString.AddAttribute(UIStringAttributeKey.Font, UIFont.BoldSystemFontOfSize(view.Font.Size),
                        new NSRange(style.Range.Start, styleTextLength));
                    break;
                case SpannableStringTypes.I:
                case SpannableStringTypes.Italic:
                    attributedString.AddAttribute(UIStringAttributeKey.Font, UIFont.ItalicSystemFontOfSize(view.Font.Size),
                        new NSRange(style.Range.Start, styleTextLength));
                    break;
                case SpannableStringTypes.Font:
                    foreach (var parameter in style.Parameters)
                    {
                        switch (parameter.Key)
                        {
                            case SpannableStringParameterTypes.Size:
                                if (float.TryParse(parameter.Value, out float fontSize))
                                {
                                    attributedString.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(view.Font.Name,
                                        fontSize), new NSRange(style.Range.Start, styleTextLength));
                                }

                                break;
                            case SpannableStringParameterTypes.Color:
                                attributedString.AddAttribute(UIStringAttributeKey.ForegroundColor, Color.Parse(parameter.Value).Render(),
                                    new NSRange(style.Range.Start, styleTextLength));
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