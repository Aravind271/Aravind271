# Production Lynx art slot

V2.1 intentionally isolates art from gameplay. The final hero file will live here as `Lynx_Hero.fbx` (or a prefab built from it).

Production target:
- Japanese male, age 21, 177 cm, lean/healthy university-student build.
- Natural black late-1980s/early-1990s hair; realistic proportions, no anime exaggeration.
- Unity Humanoid-compatible skeleton, clean T-pose, fingers included.
- LOD0 approximately 45k-80k triangles for body/head/clothing combined; lower LODs generated later.
- 2K hero textures as the default target. 4K only for assets that visibly justify the memory cost.
- Separate hair and clothing meshes.
- Face-ready blendshapes: blink L/R, jaw open, smile, frown/anger, pain, surprise as the minimum set.
- No baked proprietary logos or modern branding.

The current runtime-generated V2.1 model is an authoring fallback, not the final publishable face.
