# SRFramework

Inspired by UE4 Gameplay Ability System.
Data-driven framework. All abilities (even character movement) can be created as abilities and managed as data.

The development intention is as follows.
   *Programmers : Function-oriented Development
   *Designers : Data-Driven Development

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
* 유니티의 스크립트와 UE4의 GAS가 제대로 합쳐진다면 개발 속도가 빨라질 것으로 예상된다.
* 나아가, 게임 제작중 만들어지는 데이터(아이템, 스킬, 스탯 등)를 에셋으로 만들수 있으므로 재사용 가능성을 확인해본다.

* 내부 규칙
   * 업데이트를 남용하지 않는다.
   * 기능은 간결해야한다. 이름으로 유추할 수 있어야한다.
   * 가능하면 Has-A로 만들도록 한다.

### Dependencies

* working on Unity 2020.1. Basically, trying to minimize dependencies.
* There are no additional packages or libraries needed yet.
* Backward compatibility is considered after the stable version.

### Installing

* Place the code in "your unity project/Asset" directory.

### Executing program

* Gameplay Mod
* 스킬이나 아이템 등에 적용 가능한 스탯의 묶음.
* ![Item_option_sample](https://user-images.githubusercontent.com/10418598/161435006-26ff52d0-275c-4ea1-9b3d-2bebfc038303.gif)

* Equipment
* a

* 이벤트 등록 가능한 태그

* 이벤트 등록 가능한 스탯

* 


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
