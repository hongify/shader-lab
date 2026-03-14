using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrassTrampleController : MonoBehaviour
{
    public TMP_Text text; 
    float logTimer;

    struct GrassClump
    {
        public Vector3 position;
        public float trample;
        public float noise;

        public GrassClump(Vector3 pos)
        {
            position.x = pos.x;
            position.y = pos.y;
            position.z = pos.z;
            noise = Random.Range(0.5f, 1);
            if (Random.value < 0.5f) noise = -noise;
            trample = 0;
        }
    }

    int SIZE_GRASS_CLUMP = 5 * sizeof(float);

    public Mesh mesh;
    public Material material;
    public ComputeShader computeShader;
    [Range(0, 1)] public float density;
    [Range(0.1f, 3)] public float scale;

    public Transform[] tramplers; 
    [Range(0.1f, 2)] public float trampleRadius = 0.5f;
    public Vector3 tramplerOffset; 

    ComputeBuffer clumpsBuffer;
    ComputeBuffer argsBuffer;
    GrassClump[] clumpsArray;
    uint[] argsArray = new uint[] { 0, 0, 0, 0, 0 };
    Bounds bounds;

    int tramplePosID;
    int groupSize;
    int kernelUpdateGrass;

    int grassCount; 
    float deltaTime = 0.0f; 

    void Start()
    {
        bounds = new Bounds(Vector3.zero, new Vector3(30, 30, 30));
        InitShader();
    }

    void InitShader()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Bounds bounds = mf.sharedMesh.bounds;
        Vector2 size = new Vector2(bounds.extents.x * transform.localScale.x, bounds.extents.z * transform.localScale.z);

        Vector2 clumps = size;
        Vector3 vec = transform.localScale / 0.1f * density;
        clumps.x *= vec.x;
        clumps.y *= vec.z;

        int total = (int)clumps.x * (int)clumps.y;

        kernelUpdateGrass = computeShader.FindKernel("UpdateGrass");

        uint threadGroupSize;
        computeShader.GetKernelThreadGroupSizes(kernelUpdateGrass, out threadGroupSize, out _, out _);
        groupSize = Mathf.CeilToInt((float)total / (float)threadGroupSize);
        grassCount = groupSize * (int)threadGroupSize;

        clumpsArray = new GrassClump[grassCount];

        for (int i = 0; i < grassCount; i++)
        {
            Vector3 pos = new Vector3(Random.value * size.x * 2 - size.x, 0, Random.value * size.y * 2 - size.y);
            clumpsArray[i] = new GrassClump(pos);
        }

        clumpsBuffer = new ComputeBuffer(grassCount, SIZE_GRASS_CLUMP);
        clumpsBuffer.SetData(clumpsArray);

        computeShader.SetBuffer(kernelUpdateGrass, "clumpsBuffer", clumpsBuffer);
        computeShader.SetFloat("trampleRadius", trampleRadius);
  
        tramplePosID = Shader.PropertyToID("tramplePos");

        argsArray[0] = mesh.GetIndexCount(0);
        argsArray[1] = (uint)grassCount;
        argsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(argsArray);

        material.SetBuffer("clumpsBuffer", clumpsBuffer);
        material.SetFloat("_Scale", scale);
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        foreach (Transform trampler in tramplers)
        {
            if (trampler != null)
            {
                Vector4 tramplerPos = trampler.position + tramplerOffset; 
                computeShader.SetVector(tramplePosID, tramplerPos);
                computeShader.Dispatch(kernelUpdateGrass, groupSize, 1, 1);
            }
        }

        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);

        logTimer += Time.deltaTime;
        if (logTimer >= 0.5f)
        {
            text.text = $"Grass Count: {grassCount}, FPS: {fps:F2}";
            logTimer = 0.0f;
        }
    }

    private void OnDestroy()
    {
        clumpsBuffer.Release();
        argsBuffer.Release();
    }
}
