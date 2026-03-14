# Shader Programming Project 2024-2

This is a university project I worked on during the 2024-2 semester. I wanted to try making an interactive grass system that can handle a lot of objects efficiently using shaders.

## Overview

- **GPU Instancing**: Instead of using individual GameObjects for each blade of grass, I used GPU Indirect Instancing. This lets me render thousands of grass blades at once without putting too much load on the CPU.
- **Interactable Grass**: I used a Compute Shader to make the grass bend when a character walks over it. It calculates the distance between the character and each clump of grass in real-time on the GPU.
- **Toon Look**: I made a simple toon shader to give the grass a stylized, illustrated feel. It uses simple light steps for the shadows.
- **Bezier Movement**: I also added a smooth movement script using Bezier curves so I could test how the grass reacts to characters moving around.

## Project Structure

- `Assets/Shaders/`: 
  - `GrassToon.shader`: The surface shader for the grass visual.
  - `GrassTrample.compute`: The compute shader that handles the trampling logic.
- `Assets/Scripts/Grass/`: Contains the main controller that manages the grass buffers and rendering.
- `Assets/Scripts/Movement/`: Basic scripts for camera/character movement and the automated Bezier pathing.

## Implementation Details

1. The `GrassTrampleController` script sets up the buffers and sends the initial grass positions to the GPU.
2. In every frame, it passes the trampler's position to the Compute Shader.
3. The Compute Shader updates the grass state (how much it's bent) in the buffer.
4. Finally, Unity draws everything using the data directly from that GPU buffer.

This project was a great way to learn about the interaction between C# scripts and GPU compute shaders.
