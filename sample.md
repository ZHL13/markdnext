---
title: MarkDNext showcase
version: 0.5.0
status: ready for screenshots
---

<p align="center">
  <img src="Logo.png" alt="MarkDNext logo" width="104">
</p>

# MarkDNext

A native Windows Markdown editor for fast source editing, focused WYSIWYG blocks,
and a live preview that stays useful while you write.

> Write in Markdown, check the rendered result immediately, and export a clean
> HTML or PDF document when the draft is ready.

## What This Sample Shows

- [x] Live Markdown preview with GitHub-style tables and task lists
- [x] KaTeX inline and display formulas with bundled offline fonts
- [x] Syntax-highlighted code fences
- [x] Local image resolution from the document folder
- [x] HTML export with copied image assets
- [ ] Your next long-form note, lab report, or project README

## Project Snapshot

| Area | Detail | Shortcut or Menu |
| --- | --- | --- |
| Editing | Source mode and WYSIWYG block mode | `View -> WYSIWYG` |
| Completion | Optional Markdown snippets | `Ctrl+H` |
| Search | Editor or preview, based on focus | `Ctrl+F` |
| Export | HTML with assets, plus PDF | `File -> Export` |
| Appearance | Themes, Mica, Acrylic, fonts, line spacing | `View`, `Theme`, `Format` |

## Math Preview

Inline math stays compact: $E = mc^2$ and $\alpha + \beta = \gamma$.

Display math uses KaTeX display mode:

$$
\int_0^1 x^2\,dx = \frac{1}{3}
$$

And a slightly larger expression for screenshots:

$$
\sum_{n=1}^{\infty}\frac{1}{n^2}
=
\frac{\pi^2}{6}
\qquad
\int_{-\infty}^{\infty} e^{-x^2}\,dx
=
\sqrt{\pi}
$$

## Code Fence

```csharp
public static string DescribeExport(string fileName, bool copyAssets)
{
    var mode = copyAssets ? "HTML with local assets" : "PDF";
    return $"{fileName}: {mode}";
}

Console.WriteLine(DescribeExport("release-notes.md", copyAssets: true));
```

## Local Image

The preview resolves relative image paths from the Markdown file folder.

![MarkDNext logo](Logo.png)

## Notes

Use this file for screenshots of split view, preview-only mode, theme switching,
and WYSIWYG block editing. The same document also exercises export paths because
it contains text, tables, formulas, code, and a local image.
