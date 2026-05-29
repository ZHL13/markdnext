# Changelog

## 0.6.0 - 2026-05-30

- Reorganized the repository into `src/`, `resources/`, `examples/`, `docs/`, and `scripts/` so the source tree and GitHub view are cleaner.
- Added WYSIWYG block reordering with hover handles, drag feedback, and drop-line placement.
- Improved WYSIWYG theme refresh behavior so drag handles remain available after switching themes.
- Added editor current-line highlighting, distinct selection coloring, and configurable editor line spacing.
- Added source and WYSIWYG formatting shortcuts, inline selection wrapping, and first/last-line arrow-key behavior.
- Added view-mode shortcuts: `Ctrl+W` for WYSIWYG, `Ctrl+E` for editor only, `Ctrl+R` for preview only, `Ctrl+T` for split view, and `Ctrl+Q` to close the window.
- Moved theme import/export into the theme chooser, added complete light/dark theme export, and persist imported user themes under AppData.
- Added HTML/PDF export options and copied local HTML export images into an adjacent `assets` folder with rewritten relative paths.
- Fixed KaTeX rendering to use the bundled KaTeX font output more reliably.
- Updated packaging to produce a single versioned release executable in `dist`.

## 0.5.1 - 2026-05-28

- Added common Markdown formatting shortcuts in the source editor: `Ctrl+B` for bold, `Ctrl+I` for italic, `Ctrl+U` for underline, and `Ctrl+K` for strikethrough.
- Added the same formatting shortcuts to WYSIWYG block editing.
- Added selection wrapping for inline math and inline code: pressing `$` or `` ` `` around selected text now wraps the selection.
- Improved editor boundary navigation: `Up` on the first line moves to the line start, and `Down` on the last line moves to the line end, including `Shift+Up` and `Shift+Down` selection expansion.
