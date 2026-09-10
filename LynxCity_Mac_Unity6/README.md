# Lynx City — V2 Foundation

**Unity 6 cross-platform Japanese university-life / street-brawler prototype**

Lynx is a Japanese university student living through an original late-1980s / early-1990s Japanese city. V2 moves away from the capsule-only greybox and establishes the game's art direction, character customization, era-specific street dressing, denser Japanese crowds, improved combat controls, and scalable desktop performance.

> This project is an original game. It can use the broad cinematic Japanese street-brawler genre as inspiration, but it must not copy Yakuza/Like a Dragon maps, characters, animations, UI, music, dialogue, story, logos, or proprietary combat implementation.

## What V2 adds

- Proportioned human-shaped Lynx prototype instead of a single capsule.
- Lynx is an original Japanese male university-student character.
- Wardrobe/customization system with 5 era-inspired attire presets.
- Human-shaped Japanese crowd population with varied height and clothing.
- Original 1989 Shinjuku-inspired entertainment district.
- Original 1992 Shibuya-inspired station/shopping district.
- 1991 Aizu-inspired countryside travel area.
- Late-Showa / early-Heisei street props: roads, storefronts, awnings, lamps, vending machines, public phone booth, bicycles, crosswalks and older low-rise buildings.
- Convenience store, restaurant, bicycle shop, department store, university, dating NPC, karaoke and taxi interactions.
- Revised brawler controls: 4-hit light chain, heavy hit, guard, grab/throw, dodge, target lock and Heat-style special action.
- Mouse camera no longer conflicts with heavy attack.
- Automatic performance budget with 60 FPS target and crowd/shadow scaling.
- Build menu entries for macOS, Windows x64 and Linux x64.

## Controls

| Action | Keyboard / Mouse |
|---|---|
| Move | WASD |
| Sprint | Left Shift |
| Camera | Mouse |
| Light combo | Left Mouse |
| Heavy | Right Mouse |
| Guard | F |
| Grab / throw | R |
| Dodge | Space |
| Lock target | Tab |
| Special / Heat action | Q |
| Interact | E |
| Wardrobe | C, then 1–5 |
| Release mouse cursor | Esc |

## Character attire presets

1. Campus Casual '91
2. Denim Street '88
3. Varsity Night '90
4. Smart Date '93
5. Leather Weekend '89

## Important visual note

V2 still uses **procedurally assembled placeholder geometry** for the bodies, faces, clothes and architecture. It is deliberately much more human and era-specific than V1, but it is **not yet photorealistic**. A genuinely realistic Japanese face requires a licensed/original rigged humanoid mesh, facial textures, hair cards, skin shader, blendshapes and animation assets. The code is structured so those assets can replace the procedural `Visual` object later without rewriting game logic.

The project is no longer macOS-only. Development should stay comfortable on Apple Silicon by using a scalable pipeline: 1080p/60 as the gameplay target, pooled crowds, LODs, occlusion, baked lighting where appropriate, texture budgets, and a Quality/Balanced/Performance tier. For production-quality visuals, the preferred direction is URP for balancing quality with macOS/Windows/Linux portability.

Open the project in Unity 6, choose **Lynx City → Open Main Scene**, then press Play. Desktop builds are under **Lynx City → Build → Desktop**.
