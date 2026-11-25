# The BlackWater Isle

A third-person survival game made in Unity as my semester 5 game development final project.

You explore a flooded, foggy island at night. Enemies chase and attack you when you get close. To escape, you find and activate the factory button that restores power to the lighthouse, then reach the lighthouse to win.

## Gameplay

- **Third-person movement** with walking, running and jumping (`PlayerMovement.cs`, CharacterController). Running drains a stamina bar that refills when you stop.
- **Orbit camera** controlled by the mouse (`ThirdPersonCamera.cs`).
- **Enemy AI** (`SimpleEnemyAI.cs`): idle, chase and attack states based on distance to the player. Enemies follow the ground with raycasts and deal damage on a cooldown.
- **Health system** (`PlayerHealth.cs`) with a UI slider and a death screen.
- **Objective**: walk up to the factory button and press **E** (`ButtonActivator.cs`). The lighthouse win zone (`WinTrigger.cs`) only counts once the button is activated. Progress is kept in `GameManager` (singleton) and `PlayerPrefs`.
- **Menus and UI** (`UIManager.cs`): main menu, story screen, pause (Esc), death and win panels, credits, and on-screen messages.

## Controls

| Action | Key |
|---|---|
| Move | WASD |
| Run | Left Shift |
| Jump | Space |
| Look | Mouse |
| Use button | E |
| Pause | Esc |

## Project structure

```
Assets/
  *.cs                       gameplay scripts (listed above)
  Scenes/MainMenu.unity      main menu
  Scenes/GameScene.unity     the island level
  Animations/, EnemyAnimations/   animation clips and the player animator controller
```

## Opening the project

Unity **2022.3.62f1** (any 2022.3 LTS should work).

This repository contains my code, scenes and settings only. The third-party art and audio the scenes use are **not included**, because their licences don't allow redistribution. Import them yourself before opening `GameScene`:

- **Flooded Grounds** (Unity Asset Store), the island environment → `Assets/Flooded_Grounds/`
- **Backrooms Forever DEMO** (Unity Asset Store) → `Assets/Backrooms Forever DEMO/`
- **Mixamo** characters *Remy* (player) and *Prisoner B Styperek* (enemy) → `Assets/TexturesRemy/`, `Assets/Animations/`, `Assets/EnemyAnimations/`
- Background music (not included)

Without them the scenes will open with missing models and textures, but all scripts compile.

## Tools

Unity 2022.3 (Built-in render pipeline), C#, TextMesh Pro, Mixamo.

A VR port of this game is in [VRBlackWaterIsle](https://github.com/Titiksha774/VRBlackWaterIsle).
