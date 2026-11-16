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

## Week 11 — Data and Saves (Assignment 11, done after week 12)

2026-11-17  Asked the agent for a plan first. I chose: one Game scene that builds levels from LevelData assets (as the assignment says), Level 3 with one new simple trap, best time per level as the best score. Agent moved every balance number into ScriptableObjects (PlayerStats, Shade/Watcher/Rusher stats, GameRules) and turned Level1/Level2 into Level_01/Level_02 assets — checked they match: 8 blocks, 8 orbs, 5 spikes / 7 blocks, 7 orbs, 3 enemies.
2026-11-17  Level 3: new NearSpikes flash only when the hero is close. The agent first put timed spikes right at the Shade's edge in front of a pit — jumping over them dropped you into the pit. Moved them between the two pillars. Near spikes range 2.5 → 3 units so there is time to react.
2026-11-17  Save system: JsonUtility + save.json, missing/empty/broken file → defaults, writing through save.tmp + File.Replace. Volumes moved from PlayerPrefs into save.json, because the assignment wants settings in the save (old volumes reset to 0.8 once). Tested a broken and an empty file — the game starts with defaults.
2026-11-17  Bugs it made: a "\n" in the level labels became a real line break inside a C# string (compile error) — fixed. The RESET PROGRESS text did not fit the button — font 52 → 44.
2026-11-17  Checked the menu myself: the Settings buttons were too big and RESET PROGRESS almost touched the Sound slider. Asked to make them 520×130 → 440×110 and move them down (Reset y −185 → −235, Back −330 → −365). README still described the old Level1 scene — updated it: level select, 3 levels, Game scene, data assets, save.json.

