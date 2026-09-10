# Run Lynx City V2.1 on macOS

1. Install Unity 6.0 LTS with **Mac Build Support (IL2CPP)** in Unity Hub.
2. Add the project folder in Unity Hub.
3. Let Unity import/compile the project.
4. In the Unity menu choose **Lynx City > V2.1 > Open Character Lab**.
5. Press Play.

## Controls
- WASD: move
- Shift: run
- Mouse: orbit camera
- Mouse wheel: zoom
- C: wardrobe
- Left / Right or 1–5: change outfit
- Q / E while wardrobe is open: rotate Lynx
- Esc: release/lock cursor

## Build the native app
Choose **Lynx City > macOS > Build Native Apple Silicon**.

The resulting app is written to `Builds/macOS-AppleSilicon/Lynx City.app`.

The build menu intentionally configures Apple Silicon ARM64 + Metal + IL2CPP Release. It does not create Windows or Linux builds.
