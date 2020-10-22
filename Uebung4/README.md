# Excercise 4
TODO
## Current state of the release
With the abgabe_ue2 release, this project features an unpolished level with a parallex background. Animations are partialy done, but the character movement and attack animations are done.
The attacks themselve are also finished and are a combination of a melee, as well as a ranged attack. At the moment, only the ranged attack inflicts damage.
This may change in later builds. The level has bounds and the creation of the floor was realised with Sprite Shapes, to make level generation easier.

## Installation
### Executeable
Unzip the rar and execute the Uebung2_3.exe
### Source
For using the source as a Unity project, follow these steps:
* Download the source ZIP and extract it
* Add the project via the Unity Hub
* Profit

## Splines
There are two types of splines defined within the `MovementPath.cs`:
* Linear splines
* Loop splines
This script is assigned to the parent spline, which has multiple spline points.
Afterwards the script `FollowPath.cs` is used on the game object which should follow the spline path. One parameter is the game object which uses the `MovementPath` script.
There are two ways for the game object to follow the path:
* Linear movement
* Lerp towards

### Linear
The spline starts with the first spline point and ends at the last. When reaching the end, the spline movement reverses and goes back the same way until it reaches the first,
where it reverses its movement again. This process continues idenfinetly, or until the game object is destroyed.
### Loop
The spline starts with the first spline point and upon reachin the last one, the next spline node is set directly as the first and forms a loop.
### Linear movement
The game object moves at a constant pace towards each spline point
### Lerp towards
The game object is fast at first, but looses speed towards reaching the spline point.

## Folder structure
The folder structure of the unity project within the `Assets` folder is explained in the following subchapters, whith each explaining the contents of a folder.
### Animation
Stores the Animation controller and animations, and sometimes also the sprites used to build the animation.
### Materials
Custom or external materials used in the project
### Prefabs
Contains prefabs used in the project, for example the player character or bullets
### Scense
Contains the used scense, which will likely be restricted to one
### Scripts
Contains scripts sorted to their used case within folders.
### Sprites
Stores all sprites for the project. This may contain unused sprites, which may be needed at a later time in the project.
### Tiles
Some areas of the level are created via tilemaps and tiles, which are stored here.
