<div align="center" style="text-align: center;">
    <img src="https://github.com/Ecole-des-Nouvelles-Images/2025-Escape-VR-TheGreenHouse/blob/dev/Assets/UI/S_Logo.png" alt="Game Logo" height="300px"/>
    <h1>The GreenHouse</h1>
</div>
<div align="center" style="text-align: center;">
    <img alt="Static Badge" src="https://img.shields.io/badge/Unity%20-%206000.0.34f1%20-%20%231182C3?logo=unity&logoColor=%23FFFFFF&logoSize=auto&link=https%3A%2F%2Funity.com%2F"/>
    <img alt="GitHub Downloads (all assets, latest release)" src="https://img.shields.io/github/downloads/Ecole-des-Nouvelles-Images/2025-Escape-VR-TheGreenHouse/latest/total?sort=semver&logo=gitlfs&logoColor=%23FFFFFF&logoSize=auto&label=Release"/>
    <img alt="GitHub License" src="https://img.shields.io/github/license/Ecole-des-Nouvelles-Images/2025-Escape-VR-TheGreenHouse?label=License"/>
    <img alt="GitHub repo size" src="https://img.shields.io/github/repo-size/Ecole-des-Nouvelles-Images/2025-Escape-VR-TheGreenHouse?label=Size&color=lightgrey"/>
    <br/><br/>
    <p>
        A VR Escape-game where you find yourself trapped in an abandoned house.<br/>
        Find a way to free yourself from strange plants while gathering info and clues about this mysterious place and its manifestly absent owner.
    </p>
</div>



## Table of Contents

1. [Introduction](#introduction)
2. [Team](#team)
3. [Gameplay](#gameplay)
4. [Release & Install](#release-and-requirements)
5. [Development](#development)
6. [Special Thanks](#special-thanks)

---



## Introduction

**The GreenHouse** is an Escape-Game developed by a small team of **five students** from the [*Ecole des Nouvelles Images*](https://www.nouvellesimages.xyz/) during **2 months**.

This project was created as part of the **third year of the *Game Development* curriculum**, focusing on low-end devices and VR gameplay.

The development process tried to mimic the industry standards with dedicated **preproduction** (3 weeks), **production** (4 weeks) and **post-production** (1 week) phases.

---



## Team

This game was brought to life by :

- **Cyrielle ESCHALIER**: *Lead Game Artist, Product Owner*
- **Camilia FILHON**: *Game Artist*
- **Charlie BOYER**: *Technical Developer, SCRUM Master*
- **Michaël ELIE**: *Gameplay Developer*
- **Noah MILIANI**: *Gameplay Developer*

<div style="text-align: center;">
    <img src="https://github.com/Ecole-des-Nouvelles-Images/Unity-Template/blob/main/MetaData/Greenhouse-Team.png" alt="Team photo" width="100%"/>
</div>

---



## Gameplay

Incarnating a character trapped as soon as it enters an abandoned house, the player has no choice but to explore the various rooms and progress through strange vegetation.
Puzzles, riddles, and objects are scattered throughout the house, and the player must find a way to escape.

The game features four different rooms and some corridors :

| Rooms              | Description                                                  |
| ------------------ | ------------------------------------------------------------ |
| **The Hall**       | An introductory room where the player learns the basic mechanics of the game and familiarizes itself with the virtual environment. |
| **The Lounge**     | The heart of the house where the player can find clues and interact with various objects to progress in the game. |
| **The Greenhouse** | A room filled with plants and tools, where the player can experience many interactions with the environment. |
| **[REDACTED]**     | The culmination of the player's experience and game's lore where the player may find the solution to escape, if it manages to solve a complex puzzle. |

![Gameplay Screenshot](https://github.com/Ecole-des-Nouvelles-Images/Unity-Template/blob/main/MetaData/gameplay-screenshot.png)

---



## Release and requirements

This game has been developed for the <b><u>Meta Quest 3 headset only</u></b>.
It was designed to be played in <b><u>standalone mode with controllers</u></b>.

However, the game is built upon [**OpenXR**](https://www.khronos.org/openxr/) and the Unity engine' s XR Interaction Toolkit.
As such, the game <u>_**should**_</u> be compatible with most Android-based VR headsets that handle `.apk` files.

> [!WARNING]
> Other devices haven't been tested, and performances may vary greatly.
> The game is not guaranteed to run at all on other headsets. This is especially true for older headsets that don't support <u>*32-bit vertex index meshes*</u>, doesn't run the <u>*Vulkan graphic API*</u> or run on <u>*Android 9 Pie and older*</u>.



## Installation

This game is, at this time, unavailable from the Meta Quest Store, which would have been the preferred and easiest method.

>  [!WARNING] 
> The steps below describe the rather complex process to follow if you still desire to play the game in it's current state. It's assume using a computer that could connect your headset.
> This should be reserved for experienced users only and involve tweaking your headset's parameters.

___

You can find the latest release of the game on the [**Release page**](https://github.com/Ecole-des-Nouvelles-Images/2025-Escape-VR-TheGreenHouse/releases) or use this convenient button to download the archive directly <a href="https://github.com/Ecole-des-Nouvelles-Images/https://github.com/Ecole-des-Nouvelles-Images/2025-Escape-VR-TheGreenHouse"><img alt="Static Badge" src="https://img.shields.io/badge/Download-.zip-%20%23510ED8?style=plastic"></a>

Meta Quest users are required to create or associate a **Meta developer account** and then enable the **Developer Mode** on their headset in order to access advanced features. Once enabled, you can leverage the [Meta Developer Hub](https://developers.meta.com/horizon/downloads/package/oculus-developer-hub-win) to upload the `.apk` and install it on your device.

Alternatively, you can also use third-party tools like <a href="https://sidequestvr.com/setup-howto">SideQuest</a> for detailed instructions and tutored access.

> [!CAUTION]
>
> All users must remember the implications of the <em><u>developer mode</u></em> and the associated risks when installing applications from unknown sources or devices. <span style="color: darkred;">**Always proceed with care and remember to disable the <u>*developer mode*</u> when unnecessary.**</span>

___


For other headsets outside Meta Quest platform, that would satisfy the requirements, also download the latest version <a href="https://github.com/Ecole-des-Nouvelles-Images/https://github.com/Ecole-des-Nouvelles-Images/2025-Escape-VR-TheGreenHouse"><img alt="Static Badge" src="https://img.shields.io/badge/Download-.zip-%20%23510ED8?style=plastic"></a> and refer to your device's official manual or curated tooling in order to upload and install the game's `.apk`.



## Development

This project is **finished** as part of our curriculum and can be considered as an extended prototype.

However, despite the end of active production, this project might receive updates that could include :

- Bugs resolution and quality-of-life enhancements
- Performance and stability improvements
- Leverage hands tracking and provides controller-less, alternative gameplay.
- Unfinished content such as the **[REDACTED]** room, which is currently not accessible in the game.
- Changes to the environments and overhauls of puzzles mechanics.

If you encounter any bugs or want to share suggestions, feel free to open an **[issue ticket](https://github.com/Ecole-des-Nouvelles-Images/2025-Escape-VR-TheGreenHouse/issues)**.



## Special Thanks

We would like to thank all teachers of the [*Ecole des Nouvelles Images*](https://www.nouvellesimages.xyz/), notably :

- **Frédéric BAST** & **Jérôme CROSS**: respectively Technical and Art directors, for their mentoring and testing.
- **Tommy HA PHUOC**: Game Designer, for his support and feedback on the general gameplay, game-feel, and QA.
- **Yvan BLADET**: seasoned Developer, for his support and direct contributions to the project's shaders and VFXs.
