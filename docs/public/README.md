# Assets

This directory contains static resources for documentation:

- Images (PNG, JPG, GIF)
- SVG diagrams
- UML diagrams
- Screenshots

## Asset Usage

When referencing assets in documentation:

```markdown
![Screenshot](../assets/screenshot-overview.png)
```

Assets in `public/` directory are served directly by VitePress and should be referenced with `/` prefix:

```markdown
![Logo](/logo.svg)
```
