# DECISIONS — Light in the Dark

What I asked the AI agent (Claude Code), what it got wrong, what I changed and why.

## Week 10 — Three enemy types (Assignment 10)

2026-11-09  Asked the agent for a plan before any code: 3 enemies, their scripts, look and logic. My rules: every state is an Animator clip, scripts stay simple, a new Level2, branch feature/enemies + PR, small commits, DECISIONS.md.
2026-11-09  Agent built Shade (Patrol/Alert/Chase/Lost), Watcher (Sleep/Aim/Shoot/Cooldown) and Rusher (Idle/WindUp/Charge/Stunned/Return) + shared EnemyVision (distance, angle, Raycast on Ground). Shade could run off its platform while chasing — chase is clamped between its two waypoints.
2026-11-10  Bugs it made: Level2 was recreated and got a new GUID, Build Settings still had the old one — fixed before the commit. Rusher looked right in the scene (it flipped only at runtime) — flipX set in the scene.
2026-11-10  Its automated play test showed the Rusher kills you on the way back if you stand in its path. Kept on purpose: touching any enemy = death, one rule for all.
2026-11-11 

## Week 12 — Sound Pass (Assignment 12)

2026-11-16  Asked the agent for a plan first: sound list for my game (13 events), mixer, two tracks, settings. I chose: sounds synthesized by code, Music + SFX sliders as the assignment says, PlayerPrefs for volumes (week 11 save system is not done yet).
2026-11-16  Agent made one AudioManager (DontDestroyOnLoad, crossfade, PlaySfx with pitch 0.92–1.08) and built the GameMix mixer by script. The win sound was cut by the scene load — ExitDoor now waits 1 s and the hero goes into the door.
2026-11-16  No sound at all in the game. The agent tested in batch mode, where Unity has no audio output, so it missed that no scene had an AudioListener (missing since week 9). I found it by playing, added Audio Listener to Main Camera in all 3 scenes.
2026-11-16  Sound slider at 0 still played button clicks in the menu: the UI group went straight to Master. Moved UI under SFX, now the Sound slider mutes clicks too.
2026-11-16  Listened on the phone speaker: the jump and the button click got annoying after many presses — too sharp and loud. Remade both: jump square 260–720 Hz → soft triangle 220–440 Hz, about 9 dB quieter; click → short quiet sine tick, about 10 dB quieter.

