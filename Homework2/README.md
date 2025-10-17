# IMR Lab 02 - VR Basketball Game

## Project Overview
A concise VR basketball experience built for the **Introduction to Mixed Realities** course (Lab 02).  
Implements VR throwing, scoring, and visual feedback using **OpenXR** and **XR Interaction Toolkit**.

## What We Built
- OpenXR + XR Interaction Toolkit setup  
- XR Device Simulator for testing without a headset  
- Physics-based ball throwing mechanics  
- Physics materials (Unity Physics Materials) used to tune ball bounciness and friction  
- Event-driven scoring system and TextMeshPro UI  
- Confetti particle effect on score  
- Ball spawner with two spawn points (press `1` or `2`)

## Key Scripts
- `ScoreManager.cs` — manages score and fires events  
- `ScoreUI.cs` — updates TextMeshPro UI  
- `BasketScoreTrigger.cs` — detects valid basket scores  
- `ConfettiOnScore.cs` — plays particles on score  
- `ThrowTracker.cs` — tracks ball release position and hold state  
- `BallSpawner.cs` — spawns objects relative to player orientation

## How to Play
- **In Editor (XR Device Simulator):**  
  Use mouse + modifier keys to simulate headset and hands; grip to grab and throw.  
- **In VR:**  
  Use controller grip to grab and throw.  
- Press `1` or `2` to spawn the corresponding object at its spawn point.

## Setup
1. Clone the repository  
2. Open in Unity **2022.3.x (LTS)** or newer  
3. Install **OpenXR** and **XR Interaction Toolkit** packages  
4. Open the main scene and press **Play**

---

**Course:** Introduction to Mixed Realities — Lab 02  
**Academic Year:** 2025–2026  

---

## 🎥 Demo Video
[![Watch the Demo](https://img.youtube.com/vi/2SmNPFZkWkM/0.jpg)](https://youtu.be/2SmNPFZkWkM)
