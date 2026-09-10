# Run Lynx City on macOS

1. Install Unity Hub.
2. Install Unity **6.0 LTS**. This project was created with **6000.0.65f1**. If Hub offers a newer Unity 6.0.x LTS patch instead, it should be able to upgrade the project; make a backup before upgrading.
3. In Unity Hub, ensure the editor installation includes **Mac Build Support**.
4. Download/extract the project and select the folder named `LynxCity_Mac_Unity6` in Unity Hub using **Add/Open project from disk**.
5. Let Unity finish importing packages. The project uses the Unity Input System package.
6. Open `Assets/LynxCity/Scenes/LynxCity_Main.unity`. If it is not visible yet, use the editor menu **Lynx City > Open Main Scene**; the setup script creates it automatically.
7. Press the triangular **Play** button at the top of the Unity Editor.

## Keyboard controls
- WASD — move
- Left Shift — sprint
- Hold right mouse + move mouse — camera
- Left mouse — light combo
- Right mouse — heavy attack
- Space — dodge
- Q — special attack when the meter is high enough
- E — interact / talk / shop / taxi / date / karaoke

## Build a standalone Mac app
Use **Lynx City > Build macOS** or Unity's Build Profiles. On an M-series Mac, select Apple Silicon. The output helper writes `Builds/LynxCity.app`.
