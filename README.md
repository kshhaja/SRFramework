# SRFramework

* Inspired by UE4 Gameplay Ability System.
* This is Data-driven framework. 
* All behaviours (even character movement) can be created as abilities and managed as data.

The development intention is as follows.
* Programmers : Function-oriented Development
* Designers : Data-Driven Development
   
The goal is to easily implement the main system of Path Of Exile.

## Description

Most of the code consists of ScriptableObject,
and the basic rule is to copy and use it so that the original data is not contaminated even if it is modified at runtime.

* Working on
   * Main Systems
      * GameplayStat
      * GameplayTag
      * GameplayMod

   * Runtime
      * Ability (Skill, Locomotion etc..)
      * Item (Usable, Equipment etc..)
      * PlayerCharacter based on AI movement
   
   * Managers
      * TextManager for L10N
      * InputManager for newer Unity Input system
      * other necessary managers

TODO
* Playable Animation Event System
* Dedicated Server

## Getting Started

* If Unity's script and UE4's GAS are properly combined, the development speed is expected to increase.
* Furthermore, data (items, skills, stats, etc.) created during game production can be made into assets, so check the reusability.

### Internal Rules

   * Do not abuse updates and printing "Log"
      * It is more efficient to create a centralized event-triggered method like Apple's Central Dispatch Queue, rather than each component using its own update to operate.
      * Log output consumes more resources than expected.
   * The function should be concise. You should be able to guess by name. (like Swift and objective-c)
   * If possible, make it Has-A.
      * As development progresses, features such as combat, movement, and abilities will be added to the character.
      * You are faced with a bizarre codeset, but schedules and other issues make it difficult to solve.
      * In order to minimize this problem, it is developed with Has-A rather than Is-A to facilitate modification by function.

### Dependencies

* InputSystem is the only dependency
   * The original input module had to inspect every frame, and the input-related codes were easy to fragment. (inputs on player, camera, ui, some other... etc)
   * The new input system operates on an event-to-input basis. You can implement neatly by using the auto-generated code.
   * Another plus is that key mapping is very easy.

* working on Unity 2020.1. Basically, trying to minimize dependencies.
* There are no external packages or libraries needed yet.
* Backward compatibility is considered after the stable version.

### Installing

* Place the code in "your unity project/Asset" directory.
* Install the input system via the package manager. ( Package Manager / Unity Registry / Input System )

### Executing program

* Event Registerable Stats
   
   * Invokes an event when the stat changes due to the use of an item or skill.   
   
* Event Registerable Tags

   * Invokes an event when a tag is gained or lost.
   * ![Tag_Collection_01](https://user-images.githubusercontent.com/10418598/162029294-bdd6a51f-0683-4739-95ff-9654ca613aab.gif)

* GameplayMod

   * A bundle of stats that can be applied to skills or items.
   * ![Item_option_sample](https://user-images.githubusercontent.com/10418598/161435006-26ff52d0-275c-4ea1-9b3d-2bebfc038303.gif)
   
   * The difference from the previous system is the point that forms a set of maximum and minimum values.
   * When performing actual damage execution, only a single literal of float or int is applied. At this time, if you evaluate the min max value that exists in the corresponding index, a random value within the range is obtained.
   
* Modifier & Execution
   
   * Modifier is a function that simply modifies the stat value of the target.
   * Execution can be customized with the formula defined in CalculationClass with the stats of Source and Target.

* ExecutionCalculation

   * Base CalculationClass does nothing.
   * If you want to create a custom damage formula, inherit ExecutionCalculationBase and implement Execute.
   * ![Execution_Setup_01](https://user-images.githubusercontent.com/10418598/162058403-b4970893-d879-41aa-adaf-eabb188e1043.gif)

* Ability
   
   * You can express all kinds of abilities like projectiles, melee attacks, buffs, and even locomotion.
   * For detailed specifications, please check UE4 GAS.
   * ![Ability_Sample_01](https://user-images.githubusercontent.com/10418598/162028814-7dff463d-efb7-4d6a-9931-2eee38cb38bf.gif)

* Equipment

   * You can easily edit the information of the equipment.
   * Easily attach and detach VFX or SFX playback, animation playback and other effects.
   * ![Equipment_Entire_Sample01](https://user-images.githubusercontent.com/10418598/162028859-0f27764e-2820-4259-a05c-02341e790cc9.gif)

* Test

   1. Set "current_health" stat by default.
   2. Subscribe onStatChanged for "current_health" stats.
   3. Register "Fireball" to Player's Primary Ability through SampleAbility Script.
   4. let's see what happens.
   * ![Play_Test_01](https://user-images.githubusercontent.com/10418598/162051456-aa982e39-93e0-4134-b291-fdfe3182eb14.gif)

## Help

* This is a personal project. (for now...)

## Authors

* Contact
    * [Sanghyuk Kim](mailto:kshhaja@gmail.com)

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
