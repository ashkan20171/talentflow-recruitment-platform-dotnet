# Stage 19 — Readable Immersive Background

Stage 19 fixes the visual collision between application text and the previous background artwork.

## Changes
- Replaced the previous UI-mockup background (which contained baked-in Persian/English text) with a text-free photographic workspace/city background.
- Application labels and copy now come only from WinForms controls/localization resources.
- Removed recursive transparency: only the page canvas reveals the background; cards/panels/grids keep their solid surfaces for readability.
- Added a subtle neutral veil and soft blur to the photographic asset to improve contrast.
- Login keeps the full-screen visual identity while the login card remains opaque and readable.
- Persian RTL and English LTR behavior is unchanged.

## Design rule
Never place screenshots, logos, labels, menu items or generated text inside the background image. Background assets are decorative only.
