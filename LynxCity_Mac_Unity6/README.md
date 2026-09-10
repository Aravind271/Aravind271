# Lynx City — macOS Unity 6 prototype foundation

This is an original third-person Japanese university-life / street-brawler prototype foundation for macOS. It is intentionally not a copy of Yakuza/Like a Dragon or any other commercial title.

## What works immediately
- Lynx third-person movement and camera
- 4-hit light combo, heavy strike, dodge and Heat-style special attack
- Health, enemies and street encounters
- Dense procedural pedestrians with local interaction prompts
- Konbini, restaurant, bicycle shop and shopping-mall purchase interactions
- University lecture interaction
- Dating/affinity interaction
- Karaoke booth with Japanese/English **original placeholder song titles** and AudioClip hooks
- Taxi travel to Shibuya, Shinjuku and an Aizu-style countryside destination
- Yen, inventory, HP and special-meter HUD
- Keyboard/mouse + basic gamepad input

## Run it
1. Install Unity Hub and Unity 6.0 LTS with Mac Build Support.
2. Open this folder as a Unity project.
3. Create or open any empty 3D scene and press Play. `LynxCityBootstrap` builds the prototype automatically at runtime.
4. If Unity asks to enable the new Input System, accept and restart the editor.

## Controls
- WASD: move
- Shift: sprint
- Hold right mouse and move mouse: camera
- Left mouse: light combo
- Right mouse: heavy attack
- Space: dodge
- Q: special/Heat attack when meter is high enough
- E: interact / talk / buy / taxi / date / karaoke
- Gamepad: left stick move, right stick camera, West light, North heavy, East dodge, Right Shoulder special, South interact

## macOS build
In Unity: File > Build Profiles/Build Settings > macOS. For an M-series Mac choose Apple Silicon. For a distributable build that supports both older Intel Macs and Apple Silicon, choose the universal Intel 64-bit + Apple Silicon option if available in your installed Unity version.

## Moving from prototype primitives to a realistic 3D game
The scripts deliberately use capsules/cubes so the project can run without shipping third-party assets. Replace them with legally licensed high-quality assets:
- Lynx: realistic rigged male human model + facial rig + blendshapes
- NPCs: crowd LOD models with animation variety
- Tokyo: original or properly licensed city assets, signs and interiors
- Animations: original/licensed locomotion, reactions, grabs, wall hits, knockdowns and finishers
- Audio: original/licensed ambience, dialogue and music

For photorealistic visuals on modern Macs, migrate the code into a Unity 6 HDRP project or a carefully tuned URP project. Metal is supported on macOS, including HDRP on macOS.

## Recommended next production systems
1. Animator state machine + motion-matched locomotion
2. Lock-on, guarding, parry, grabs, contextual finishers and weapon pickup
3. Hit reactions, ragdoll blending and environmental collisions
4. Additive scene streaming for Shibuya, Shinjuku, campus, interiors and countryside
5. NavMesh crowds with pooling, LOD and spawn budgets
6. Real shop menus, food buffs, clothing, bicycle ownership and inventory
7. University calendar, attendance, exams, assignments, research lab and part-time jobs
8. Dialogue graph, friendship/dating events and reputation
9. Karaoke rhythm gameplay using original or properly licensed tracks
10. Save/load, settings, controller remapping and macOS notarization

## Copyright / asset note
Do not copy SEGA/RGG source code, models, textures, animations, UI, maps, dialogue, logos, music, or proprietary game data. The combat here is an original cinematic-brawler implementation using broad genre mechanics. Real locations such as Shibuya and Shinjuku can be represented with your own/licensed geometry and data.

### One-click scene/build helpers
On first import an Editor helper creates `Assets/LynxCity/Scenes/LynxCity_Main.unity` and adds it to Build Settings. You can also use:
- `Lynx City > Open Main Scene`
- `Lynx City > Build macOS`

The build helper invokes Unity's macOS standalone builder. Final signing/notarization and architecture selection should still be verified in Build Profiles before distribution.
