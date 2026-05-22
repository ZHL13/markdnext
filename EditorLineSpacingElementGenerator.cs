using ICSharpCode.AvalonEdit.Rendering;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MarkDNext;

public sealed class EditorLineSpacingElementGenerator : VisualLineElementGenerator
{
    private int _generatedLineOffset = -1;

    public double TargetLineHeight { get; set; }

    public double BaselinePadding { get; set; }

    public override void StartGeneration(ITextRunConstructionContext context)
    {
        _generatedLineOffset = -1;
        base.StartGeneration(context);
    }

    public override int GetFirstInterestedOffset(int startOffset)
    {
        if (TargetLineHeight <= 0 || CurrentContext?.Document is null)
        {
            return -1;
        }

        var document = CurrentContext.Document;
        if (startOffset > document.TextLength)
        {
            return -1;
        }

        var line = document.GetLineByOffset(Math.Min(startOffset, document.TextLength));
        if (startOffset <= line.Offset && _generatedLineOffset != line.Offset)
        {
            return line.Offset;
        }

        return -1;
    }

    public override VisualLineElement ConstructElement(int offset)
    {
        _generatedLineOffset = offset;
        var targetBaseline = Math.Clamp(TargetLineHeight - BaselinePadding, 0, TargetLineHeight);
        return new LineHeightSpacerElement(TargetLineHeight, targetBaseline);
    }

    private sealed class LineHeightSpacerElement : VisualLineElement
    {
        private readonly double _height;
        private readonly double _baseline;

        public LineHeightSpacerElement(double height, double baseline)
            : base(1, 0)
        {
            _height = height;
            _baseline = baseline;
        }

        public override TextRun CreateTextRun(int startVisualColumn, ITextRunConstructionContext context)
        {
            return new LineHeightSpacerRun(_height, _baseline, context.GlobalTextRunProperties);
        }
    }

    private sealed class LineHeightSpacerRun : TextEmbeddedObject
    {
        private readonly double _height;
        private readonly double _baseline;
        private readonly TextRunProperties _properties;

        public LineHeightSpacerRun(double height, double baseline, TextRunProperties properties)
        {
            _height = height;
            _baseline = baseline;
            _properties = properties;
        }

        public override LineBreakCondition BreakBefore => LineBreakCondition.BreakPossible;

        public override LineBreakCondition BreakAfter => LineBreakCondition.BreakPossible;

        public override bool HasFixedSize => true;

        public override CharacterBufferReference CharacterBufferReference => new(" ", 0);

        public override int Length => 1;

        public override TextRunProperties Properties => _properties;

        public override TextEmbeddedObjectMetrics Format(double remainingParagraphWidth)
        {
            return new TextEmbeddedObjectMetrics(0, _height, _baseline);
        }

        public override Rect ComputeBoundingBox(bool rightToLeft, bool sideways)
        {
            return new Rect(0, -_baseline, 0, _height);
        }

        public override void Draw(DrawingContext drawingContext, Point origin, bool rightToLeft, bool sideways)
        {
        }
    }
}
