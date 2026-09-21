# Laptop Service

A cozy simulator of servicing procedurally generated laptops.
Built with Unity and C#.

## Gameplay
- Procedurally generated laptops with unique defects
- Tool system with individual ranges and abilities
- Customer story system
- Average playtime: 15–20 minutes

## Technical Highlights
- **Procedural generation**: defects are generated based on the laptop's service rating. The system prevents defects from overlapping or spawning outside the laptop.
- **Tool system**: each tool has unique capabilities, durability, and range. For example, a rag removes water but cannot remove scratches.
- **Post machine system**: the post machine accepts completed laptops and dispenses new ones based on a code entered via the machine's buttons.
- **Game state management**: complete game loop with state-based transitions between locations.

## How to Run
1. Clone the repository.
2. Open the project in Unity version.
3. Open the scene in Assets/Scenes.
4. Press Play.

## Tech Stack
- Unity, C#
- DOTween — smooth animations
- TMPEffects — text effects

## Links
- itch.io: https://doubleav.itch.io/laptop-service
