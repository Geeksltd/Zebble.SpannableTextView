using CoreGraphics;
using CoreText;
using Foundation;
using System.Drawing;
using System.Security.Policy;
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

            RenderSpannableText();
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            var superBool = base.PointInside(point, uievent);

            var textContainer = new NSTextContainer(Bounds.Size);
            var layoutManager = new NSLayoutManager();

            textContainer.LineFragmentPadding = (nfloat)0.0;
            textContainer.LineBreakMode = UILineBreakMode.CharacterWrap;
            textContainer.MaximumNumberOfLines = 10;

            layoutManager.AddTextContainer(textContainer);

            if (AttributedString == null)
            {
                return false;
            }

            var textStorage = new NSTextStorage(AttributedString.Value);
            //textStorage.AddAttribute(UIStringAttributeKey.Font, !, NSMakeRange(0, attributedText.length));
            textStorage.AddLayoutManager(layoutManager);
            var locationOfTouchInLabel = point;
            var textBoundingBox = layoutManager.GetUsedRectForTextContainer(textContainer);
            var alignmentOffset = 0.0;

            //switch (this.Align)
            //{
            //    case MyEnum.left:
            //    case MyEnum.natural:
            //    case MyEnum.justified:
            //        {
            //            alignmentOffset = 0.0D;
            //            break;
            //        }
            //    case MyEnum.center:
            //        {
            //            alignmentOffset = 0.5D;
            //            break;
            //        }
            //    case MyEnum.right:
            //        {
            //            alignmentOffset = 1.0D;
            //            break;
            //        }
            //    default:
            //        {
            //            fatalError();
            //            break;
            //        }
            //}

            var xOffset = ((Bounds.Size.Width - textBoundingBox.Size.Width) * alignmentOffset) - textBoundingBox.X;
            var yOffset = ((Bounds.Size.Height - textBoundingBox.Size.Height) * alignmentOffset) - textBoundingBox.Y;
            var locationOfTouchInTextContainer = new CGPoint(locationOfTouchInLabel.X - xOffset, locationOfTouchInLabel.Y - yOffset);
            var characterIndex = layoutManager.GetCharacterIndex(locationOfTouchInTextContainer, textContainer, out var _);
            var lineTapped = (int)(Math.Ceiling(locationOfTouchInLabel.Y / /*font.lineHeight*/1)) - 1;
            var rightMostPointInLineTapped = new CGPoint(Bounds.Size.Width, /*font.lineHeight*/1 * lineTapped);
            var charsInLineTapped = layoutManager.GetCharacterIndex(rightMostPointInLineTapped, textContainer, out var _);

            //if (characterIndex < charsInLineTapped == null)
            //{
            //    return false;
            //}

            var attributeName = UIStringAttributeKey.Link;
            var attributeValue = AttributedString?.GetAttribute(attributeName, (nint)characterIndex, out var _);
            if (attributeValue is null) return false;

            if (attributeValue is not NSUrl url) return false;

            View.LinkTapped.Raise(new EventArgs<string>(url.ToString()));

            return superBool;
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