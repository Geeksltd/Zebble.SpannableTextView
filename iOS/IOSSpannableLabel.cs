using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace Zebble.IOS
{
    public class IOSSpannableLabel : IosLabel
    {
        SpannableTextView View;

        public IOSSpannableLabel(SpannableTextView view) : base(view)
        {
            View = view;

            view.SpannableTextChanged.HandleActionOn(Thread.UI, RenderSpannableText);

            UserInteractionEnabled = true;
            Label.UserInteractionEnabled = true;

            RenderSpannableText();
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            var baseResult = base.PointInside(point, uievent);

            var textContainer = new NSTextContainer(Label.Bounds.Size)
            {
                LineFragmentPadding = (nfloat)0.0,
                LineBreakMode = Label.LineBreakMode,
                MaximumNumberOfLines = (uint)Label.Lines
            };

            var layoutManager = new NSLayoutManager();
            layoutManager.AddTextContainer(textContainer);

            if (Label.AttributedText == null)
            {
                return false;
            }

            var textStorage = new NSTextStorage(Label.AttributedText.Value);

            textStorage.AddAttribute(UIStringAttributeKey.Font, Label.Font, new NSRange(0, Label.AttributedText.Value.Length));
            textStorage.AddLayoutManager(layoutManager);

            var locationOfTouchInLabel = point;
            var textBoundingBox = layoutManager.GetUsedRectForTextContainer(textContainer);
            double alignmentOffset;

            switch (Label.TextAlignment)
            {
                case UITextAlignment.Left:
                case UITextAlignment.Natural:
                case UITextAlignment.Justified:
                    {
                        alignmentOffset = 0.0D;
                        break;
                    }
                case UITextAlignment.Center:
                    {
                        alignmentOffset = 0.5D;
                        break;
                    }
                case UITextAlignment.Right:
                    {
                        alignmentOffset = 1.0D;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(Label.TextAlignment));
            }

            var xOffset = ((Label.Bounds.Size.Width - textBoundingBox.Size.Width) * alignmentOffset) - textBoundingBox.X;
            var yOffset = ((Label.Bounds.Size.Height - textBoundingBox.Size.Height) * alignmentOffset) - textBoundingBox.Y;
            var locationOfTouchInTextContainer = new CGPoint(locationOfTouchInLabel.X - xOffset, locationOfTouchInLabel.Y - yOffset);

            var characterIndex = layoutManager.GetCharacterIndex(locationOfTouchInTextContainer, textContainer, out _);
            var lineTapped = (int)Math.Ceiling(locationOfTouchInLabel.Y / Label.Font.LineHeight) - 1;
            var rightMostPointInLineTapped = new CGPoint(Label.Bounds.Size.Width, Label.Font.LineHeight * lineTapped);
            var charsInLineTapped = layoutManager.GetCharacterIndex(rightMostPointInLineTapped, textContainer, out _);

            if (characterIndex < charsInLineTapped)
            {
                return false;
            }

            var attributeValue = Label.AttributedText.GetAttribute(UIStringAttributeKey.Link, (nint)characterIndex, out _);
            if (attributeValue is null) return false;

            if (attributeValue is not NSString str) return false;

            View.LinkTapped.Raise(new EventArgs<string>(str.ToString()));

            return baseResult;
        }

        void RenderSpannableText()
        {
            if (View.ParsedText == null) return;

            foreach (var spannableStyle in View.ParsedText)
            {
                spannableStyle.RenderSpannableStringStyle(View, AttributedString);

                if (spannableStyle.Children.Count > 0)
                    RenderChildSpannableStyle(AttributedString, spannableStyle);
            }

            Label.AttributedText = AttributedString;
            View.LineHeightChanged.Raise();
        }

        void RenderChildSpannableStyle(NSMutableAttributedString text, SpannableStringStyle parentStyle)
        {
            foreach (var style in parentStyle.Children)
            {
                style.RenderSpannableStringStyle(View, text);

                if (style.Children.Count > 0) RenderChildSpannableStyle(text, style);
            }
        }
    }
}