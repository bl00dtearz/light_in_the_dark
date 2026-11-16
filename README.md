# Light in the Dark

A 2D mobile troll platformer for Android/iOS (landscape).
The level is dark: only a circle of light around the hero is visible.
Traps flash for a split second - remember them and get through.

- Engine: Unity 6.3 LTS (6000.3.24f1), URP 2D, uGUI, Input System, TextMeshPro
- Course: Game Development Fundamentals, Narxoz University
- Author: bl00dtearz
- Design document: [Docs/GDD.pdf](Docs/GDD.pdf)

## How to play
Collect all 8 light orbs, then walk into the open door. One touch of a trap ends the run.
Phone: left/right buttons bottom-left, JUMP bottom-right, pause top-right. Editor: A/D + Space.

## Scenes
`Assets/Scenes/MainMenu` (start here) and `Assets/Scenes/Level1`.
## Sounds and music
All sounds and both music loops were synthesized by code for this project (own work, made with the help of an AI agent; no third-party audio). License: same as the project.

| File | What | Source · license |
|---|---|---|
| `Audio/SFX/jump.wav` | hero jump | synthesized, own work |
| `Audio/SFX/orb_pickup.wav` | light orb collected | synthesized, own work |
| `Audio/SFX/death.wav` | hero hit / death | synthesized, own work |
| `Audio/SFX/lose.wav` | level restarts after a death | synthesized, own work |
| `Audio/SFX/door_open.wav` | exit door opens | synthesized, own work |
| `Audio/SFX/win.wav` | hero enters the open door | synthesized, own work |
| `Audio/SFX/floor_crack.wav` | fake floor cracks | synthesized, own work |
| `Audio/SFX/shade_alert.wav` | Shade wind-up (growl) | synthesized, own work |
| `Audio/SFX/watcher_aim.wav` | Watcher wind-up (charging hum) | synthesized, own work |
| `Audio/SFX/watcher_shoot.wav` | Watcher shot | synthesized, own work |
| `Audio/SFX/rusher_windup.wav` | Rusher wind-up (snorts) | synthesized, own work |
| `Audio/SFX/rusher_crash.wav` | Rusher hits a wall | synthesized, own work |
| `Audio/SFX/ui_click.wav` | button click | synthesized, own work |
| `Audio/Music/menu_loop.wav` | menu music, calm loop (66 bpm) | synthesized, own work |
| `Audio/Music/level_loop.wav` | level music, tense loop (112 bpm) | synthesized, own work |
