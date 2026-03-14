# Shader Programming Project 2024-2

Interactive grass system in Unity, focused on efficient GPU computation and real-time interaction.

### Core Implementation
- **GPU Indirect Instancing**: High-performance rendering for dense grass fields. 
- **Compute Shader Physics**: Blade-character interaction and trample logic processed on the GPU.
- **Stylized Rendering**: Custom Toon Shader involving step-based lighting and vertex deformation.
- **Bezier Pathing**: Smooth character movement simulation using Cubic Bezier curves.

### Project Files
- `GrassTrampleController.cs`: Manages GPU buffers and indirect draw calls.
- `GrassTrample.compute`: Parallel computation of grass bending and restoration.
- `GrassToon.shader`: Handles the visual presentation and vertex offsets.
- `BezierMovement.cs`: Path-based movement utility for interaction testing.

---
*2024-2 University Course Work*
