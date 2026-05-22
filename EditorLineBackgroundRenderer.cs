using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System.Windows;
using System.Windows.Media;

namespace MarkDNext;

public sealed class EditorLineBackgroundRenderer : IBackgroundRenderer
{
    private readonly TextEditor _editor;

    public EditorLineBackgroundRenderer(TextEditor editor)
    {
        _editor = editor;
    }

    public KnownLayer Layer => KnownLayer.Background;

    public Brush? Background { get; set; }

    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        if (Background is null || _editor.Document is null)
        {
            return;
        }

        textView.EnsureVisualLines();
        foreach (var visualLine in textView.VisualLines)
        {
            if (!ShouldHighlightLine(visualLine.FirstDocumentLine, visualLine.LastDocumentLine))
            {
                continue;
            }

            var top = visualLine.VisualTop - textView.VerticalOffset;
            var rect = new Rect(0, top, textView.ActualWidth, visualLine.Height);
            drawingContext.DrawRectangle(Background, null, rect);
        }
    }

    private bool ShouldHighlightLine(DocumentLine firstLine, DocumentLine lastLine)
    {
        var caretLine = _editor.TextArea.Caret.Line;
        for (var line = firstLine; line is not null && line.LineNumber <= lastLine.LineNumber; line = line.NextLine)
        {
            if (line.LineNumber == caretLine || IsFullySelected(line))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsFullySelected(DocumentLine line)
    {
        var selection = _editor.TextArea.Selection;
        if (selection.IsEmpty)
        {
            return false;
        }

        var requiredEndOffset = line.Length == 0
            ? Math.Min(_editor.Document.TextLength, line.EndOffset + line.DelimiterLength)
            : line.EndOffset;

        foreach (var segment in selection.Segments)
        {
            if (segment.StartOffset <= line.Offset && segment.EndOffset >= requiredEndOffset)
            {
                return true;
            }
        }

        return false;
    }
}
