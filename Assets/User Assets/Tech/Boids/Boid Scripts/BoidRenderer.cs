using UnityEngine;

public class BoidRenderer : MonoBehaviour
{
    [SerializeField] private BoidMaster boidMaster;

    [SerializeField] private Mesh boidMesh;
    [SerializeField] private Material boidMaterial;

    ComputeBuffer argsBuffer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
        args[0] = (uint)boidMesh.GetIndexCount(0);   // index count per instance
        args[1] = (uint)boidMaster.GetBoidAmount();               // instance count
        args[2] = (uint)boidMesh.GetIndexStart(0);   // start index location
        args[3] = (uint)boidMesh.GetBaseVertex(0);   // base vertex location

        argsBuffer =
            new ComputeBuffer(1,
                                args.Length * sizeof(uint),
                                ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);

    }

    // Update is called once per frame
    void Update()
    {
        boidMaterial.SetBuffer("boids", boidMaster.GetBoidComputeBuffer());

        Graphics.DrawMeshInstancedIndirect(
        boidMesh,
        0,
        boidMaterial,
        new Bounds(boidMaster.GetBoundsCenter(), boidMaster.GetBoundsExtents()),
        argsBuffer
        );
    }

    private void OnDestroy()
    {
        argsBuffer.Dispose();
    }
}
