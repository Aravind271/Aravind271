# Lynx City — V2.1 Character Lab (native macOS / Apple Silicon)

V2.1 deliberately stops expanding the open world. This build concentrates on **Lynx**, the Japanese university-student protagonist, before city production resumes.

## What V2.1 adds
- Native macOS-only project policy for Apple Silicon.
- Metal-only standalone rendering configuration.
- IL2CPP Release scripting backend for native builds.
- Linear color space and a stable 60 fps runtime target.
- A new articulated Lynx authoring model with a real joint hierarchy instead of the old block mannequin.
- More readable face construction: head/jaw, eyes/irises/pupils, brows, ears, nose, lips and layered black hair.
- More natural procedural idle/walk/run movement with hips, knees, arms, elbows, torso sway and breathing.
- Five genuinely different 1989–1992 outfits, with silhouette/details rather than only recoloring one outfit.
- Persistent wardrobe choice.
- Close wardrobe camera plus Q/E character rotation.
- Smoother acceleration/deceleration and improved orbit/zoom camera.
- A small early-Heisei character lab environment designed to inspect Lynx without hiding him behind unfinished city systems.
- A production FBX import/validation slot for the final realistic hero model.

## Important art status
The V2.1 generated character is a **higher-quality authoring fallback**, not the final photorealistic publishable Japanese face. A truly publishable hero requires a proper sculpted/rigged character asset. The project now has the correct production slot and import contract for that asset so we do not rewrite gameplay when it arrives.

## Run
Use Unity **6000.0.65f1** on macOS.

1. Add this folder to Unity Hub.
2. Open it.
3. Choose `Lynx City > V2.1 > Open Character Lab`.
4. Press Play.

Controls: WASD move, Shift run, mouse orbit, mouse wheel zoom, C wardrobe, Left/Right or 1–5 outfit, Q/E rotate while wardrobe is open, Esc cursor.

## Native Apple Silicon build
Choose `Lynx City > macOS > Build Native Apple Silicon`.

The editor script forces ARM64, Metal only, IL2CPP Release and a macOS `.app` build. Output: `Builds/macOS-AppleSilicon/Lynx City.app`.

## Development rule
Do not resume Shibuya/Shinjuku/crowd/combat feature expansion until the hero character passes the V2.1 visual and locomotion acceptance bar in `Docs/V2_1_CHARACTER_SPEC.md`.
