# Astro Putt - VR Space Golf Game

<p align="center">
  <img src="Documentation/logo_minimalistic.png" />
</p>

🎓 **Bachelor Thesis Project** - This is my Final project for obtaining the Computer Engineering degree at Universitat Oberta de Catalunya

📋 **[Download Full Thesis Document (PDF) (in Spanish)](Documentation/project.pdf)**

## 🎮 Game Description

**Astro Putt** transports us to a future where golf is played on a cosmic scale. It is a virtual reality golf game in which physics plays a fundamental role. The player stands on orbital platforms and must hit a spherical device to launch it towards targets scattered throughout space. Each body encountered along the trajectory can exert its own gravitational force, which can modify the movement of the sphere in unexpected ways. Therefore, success depends on mastering orbital mechanics and not just executing the perfect shot.

The game aims to be both an entertainment experience and an interactive demonstration of physics. To achieve this, it realistically incorporates Newton's laws of gravitational attraction, allowing players to leverage gravity assist maneuvers to accelerate the sphere or, conversely, suffer the consequences of getting too close to a massive body. The player can try to study the most promising trajectories before launching, but the final result will depend on their precision and ability to adapt to the environment.

Astro Putt combines science and gameplay to transform the laws of the universe into an immersive and accessible experience. An invitation to explore gravity not only as a force, but as a cosmic challenge full of possibilities.

### Key Features
- **Realistic Gravitational Physics**: Experience Newton's laws of gravitational attraction in action
- **VR Immersion**: Full controller tracking and physics-based interaction using Meta Quest 2
- **Orbital Mechanics**: Master gravity assist maneuvers and orbital trajectories
- **Space Platforms**: Play from orbital platforms with cosmic targets
- **Interactive Tutorial**: Learn unique gravitational mechanics through guided gameplay
- **Score Tracking**: Save your best scores and hole counts across multiple courses
- **Educational Value**: Interactive demonstration of real physics principles

### Gameplay Mechanics
- **Gravitational Golf**: Hit spherical devices towards space targets using gravity
- **Orbital Precision**: Account for massive bodies affecting your sphere's movement
- **Gravity Assists**: Use gravitational forces strategically to reach distant targets
- **Cosmic Challenges**: Navigate increasingly complex gravitational environments

## 📸 Screenshots

![Lobby](Documentation/Screenshots/title.png)

![Tutorial level](Documentation/Screenshots/escenario.png)

![Gameplay](Documentation/Gameplay/segundo_disparo_bien.gif)

![Course 1](Documentation/Screenshots/landscape.png)

*More Screenshots and gameplay videos can be found in the [Documentation](Documentation/) directory*

## 🏗️ Project Structure

```
Assets/
├── Scenes/          # Main game scenes (Lobby, Tutorial, Courses)
├── Scripts/         # Custom C# gameplay scripts
├── Models/          # 3D models and prefabs
├── Materials/       # Unity materials and physics materials
├── Audio/           # Sound effects and music
└── UI/              # User interface elements

Documentation/
├── Gameplay/       # Videos demonstrating gameplay
├── Screenshots/    # Game screenshots
└── project.pdf     # Complete thesis document
```

## 🚀 Technical Specifications

- **Unity Version**: Unity 6.2 (Unity 6000.0.28f1 or later recommended)
- **Target Platform**: Meta Quest 2
- **XR Framework**: Unity XR Interaction Toolkit 3.3.0
- **Physics**: Custom gravity system with rigidbody interactions
- **Input**: Controller-based interaction

### Platform Support
- **Meta Quest 2**: Full VR experience with controller tracking (Recommended)
- **Windows (PC)**: Desktop version using XR Device Simulator for development and testing

*Note: The Windows build uses Unity's XR Device Simulator and is primarily intended for development purposes. The full experience is designed for Meta Quest 2.*

## 📦 Installation & Releases

### Quick Start (Meta Quest 2)
1. Download the latest `.apk` from the releases section
2. Enable Developer Mode on your Meta Quest 2
3. Sideload the APK using SideQuest or ADB
4. Launch "Astro Putt" from your Unknown Sources

## 🛠️ Development Setup

### Prerequisites
- Unity 6.2 (Unity 6000.0.28f1 or later)
- Meta Quest 2 with Developer Mode enabled
- Windows 10/11 or macOS for development
- Android Build Support module for Unity

### Quick Development Setup
```bash
# Clone the repository
git clone [your-repository-url]
cd "Astro Putt"

# Open in Unity Hub
# Import as Unity project (Unity 6.2)
# Ensure Android build target is installed
```


## ⚖️ Legal & Assets

### Third-Party Resources Disclaimer
This project incorporates various free-to-use assets from the Unity Asset Store and other sources. All assets used are either:
- Free assets with appropriate usage rights for educational/personal projects
- Unity's built-in resources and sample projects
- Self-created content

**Important**: While this project is open source, the third-party assets included may have their own licensing restrictions. These assets are provided for educational use and may not be suitable for commercial redistribution. Please refer to the full thesis document for complete asset attribution and source links.

### License
- **Source Code:** The C# scripts and project configuration files created for this project are licensed under the MIT License (see LICENSE file). You are free to use, modify, and distribute this code.

- **Assets & Third-Party Libraries:** All 3D models, textures, sound effects, and third-party Unity packages (including the XR Interaction Toolkit) remain the property of their original creators or are licensed under their specific terms. These assets are included for demonstration/educational purposes only and are not covered by the MIT License.

If you wish to use these assets in your own project, please acquire them directly from the original sources.




*Developed as part of the Computer Engineering program at Universitat Oberta de Catalunya • 2026*