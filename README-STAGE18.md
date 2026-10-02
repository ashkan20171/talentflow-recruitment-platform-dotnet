# Stage 18 — Immersive Workspace Background

This stage introduces a full-window visual identity for TalentFlow while preserving the existing role-based recruitment workflows.

## Visual changes
- Bundled offline recruitment/workspace background under `Assets/`.
- Full workspace background behind application pages.
- Page containers using the standard application background are made transparent so the visual can breathe through unused space while cards, grids and forms remain readable.
- Login content and branding regions use the same visual language.
- Branding region applies a dark navy readability overlay.
- Existing Persian RTL / English LTR behavior is preserved.
- No network dependency is required for the background asset.

## Implementation
`Core/VisualAssets.cs` owns safe asset loading and background application. The image is copied to the output directory by MSBuild using `PreserveNewest`.
