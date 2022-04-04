# SRFramework

Inspired by UE4 Gameplay Ability System.
Data-driven framework. All abilities (even character movement) can be created as abilities and managed as data.

The development intention is as follows.
* Programmers : Function-oriented Development
* Designers : Data-Driven Development
   
The goal is to easily implement the main system of Path Of Exile.

## Description

Most of the code consists of ScriptableObject,
and the basic rule is to copy and use it so that the original data is not contaminated even if it is modified at runtime.

Working on
* Stat System
* Mod System
* Ability System
* Tag System
* Equipment System
* Input System

TODO
* Playable Animation Event System
* Dedicated Server

## Getting Started
* If Unity's script and UE4's GAS are properly combined, the development speed is expected to increase.
* Furthermore, data (items, skills, stats, etc.) created during game production can be made into assets, so check the reusability.

### Internal Rules
   * Do not abuse updates.
   * The function should be concise. You should be able to guess by name. (like Swift and objective-c)
   * If possible, make it Has-A.

### Dependencies

* working on Unity 2020.1. Basically, trying to minimize dependencies.
* There are no additional packages or libraries needed yet.
* Backward compatibility is considered after the stable version.

### Installing

* Place the code in "your unity project/Asset" directory.

### Executing program

* Event Registerable Stats
   
   * Invokes an event when the stat changes due to the use of an item or skill.


* Gameplay Mod

   * A bundle of stats that can be applied to skills or items.
   ![Item_option_sample](https://user-images.githubusercontent.com/10418598/161435006-26ff52d0-275c-4ea1-9b3d-2bebfc038303.gif)
   

* Event Registerable Tags

   * Invokes an event when a tag is gained or lost.


* Ability
   
   * You can express all kinds of abilities like projectiles, melee attacks, buffs, and even locomotion.
   * For detailed specifications, please check UE4 GAS.


* Equipment

   * You can easily edit the information of the equipment.
   * Easily attach and detach VFX or SFX playback, animation playback and other effects.

## Help

* This is a personal project. (for now...)

## Authors

Contact
ex. [Sanghyuk Kim](mailto:kshhaja@gmail.com)

## Version History

* 0.1
    * Initial Commit

## License

This project is licensed under the MIT License - see the LICENSE.md file for details

## Acknowledgments

Inspiration, code snippets, etc.
* [fluid stats](https://github.com/ashblue/fluid-stats)
* [Unity GAS](https://github.com/sjai013/unity-gameplay-ability-system)
* [UE4 GAS Documentation](https://github.com/tranek/GASDocumentation)
