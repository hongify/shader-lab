# Shader Programming Project 2024-2: Interactive Grass System for Unity (GPU Instancing)

![Main Preview](Documentation/main_image.png)

A real-time interactive grass system developed for a university shader programming course. This project focuses on offloading complex grass physics and dense rendering workloads to the GPU to maintain high performance in Unity.

---

### GPU & Physics Implementation

The system is built on **GPU Indirect Instancing**, allowing for the drawing of thousands of uniquely transformed grass blades in a single draw call. Real-time interactions, such as grass trampling and bending, are handled via **Compute Shaders**.

<p align="center">
  <img src="Documentation/image2.png" width="48%" />
  <img src="Documentation/image3.png" width="48%" />
</p>

- **Efficient Rendering**: By bypassing the traditional GameObject-based approach, CPU overhead is minimized.
- **Dynamic Interaction**: The positions of interactive entities are passed to the GPU every frame for distance-based displacement.
- **Buffer Management**: A `ComputeBuffer` stores the state of each grass clump, updated in parallel on the GPU.

### Stylized Technical Rendering

The visual presentation uses a custom **Toon Surface Shader** that works in tandem with the GPU buffer.
- **Vertex Deformation**: The shader reads interaction data directly from the structured buffer to apply vertex offsets.
- **NdotL Lighting**: Implements a step-based light model to achieve a clean, illustrated aesthetic.
- **Procedural Variation**: Individual grass blades are randomized based on noise values to avoid repetitive patterns.

![Rendering Results](Documentation/image4.png)

---
*2024-2 University Course Work / Unity 2021.3+*
