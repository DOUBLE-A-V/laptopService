# Laptop Service
Cozy simulator of servicing procedurally generated laptops
built with unity C#

## Gameplay
- Procedurally generated laptops with unique effects
- Tool system with individual ranges and abilities
- Customer story system
- Average playtime 15-20 mins

## Technical highlights
- **procedural generation**: generating defects on laptops. Difficult system of ranging defects by the rating of laptop service. Preventing generating defects on each other and outside of the laptop.
- **Tool system**: each tool has it's own perks, durability and range. For example rag just can't remove the scratches but easily removes water.
- **Post machine system**: post machine can accept done laptop or give you a laptop of the customer by the code from email that you type in via buttons on post machine.
- **Game state management**: full gameloop without any bugs, switching between places only when you can.

## How to run
1. Clone the repo
2. Open project in unity
3. Open scene in Assets/Scenes
4. Done!

## Tech stack
- Unity & Unity C#
- DOTween for smooth animations
- TMPEffects for beautiful TMP Text effects
itch.io: https://doubleav.itch.io/laptop-service
