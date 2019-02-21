# 2D Platformer Character Controller for Unity

Provides more precise and snappier controls for a platformer game than the default Unity character controller.

The repository contains all required assets for a demo scene to demonstrate the controller. The important files are in the `Assets/Scripts/` folder.

## Features
  - Raycast based collision detection
  - Support for one-way platforms with the ability to drop through
  - Reactive jump: Jump input is still recognized right before and after being grounded
  - Ability to control jump height by releasing the jump button mid jump

## Setting up
  1. Add the `MovementController.cs` and `Player.cs` scripts to your player game object.
  2. Make sure the player has a **Box Collider 2D** attached to it.
  3. Set up the collision masks inside the inspector.
  4. Start tweaking the parameters!
### One-way platforms
  1. Attach **Edge Collider 2D** to the platform game object.
  2. Tag the platform as `Platform`.

  Pressing "Down" on a platform will make the player fall through it.

## Sources:
  - [Creating 2D Games in Unity 4.5 playlist by 3DBuzz](https://www.youtube.com/playlist?list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n)
  - [The hobbyist coder #1: 2D platformer controller](https://www.gamasutra.com/blogs/YoannPignole/20131010/202080/The_hobbyist_coder_1_2D_platformer_controller.php)
  - [Platformer controls: how to avoid limpness and rigidity feelings](https://www.gamasutra.com/blogs/YoannPignole/20140103/207987/Platformer_controls_how_to_avoid_limpness_and_rigidity_feelings.php)
