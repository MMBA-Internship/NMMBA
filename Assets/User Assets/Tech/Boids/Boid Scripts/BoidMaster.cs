using UnityEngine;

public class BoidMaster : MonoBehaviour
{
    [SerializeField] private ComputeShader boidComputeShader;
    [SerializeField] private int boidAmount;

    [SerializeField] private Vector3 boundsExtents = new Vector3(20, 10, 20);
    [SerializeField] private Vector3 boundsCullingExtents = new Vector3(20, 10, 20);
    [SerializeField] private float boundsRadius;
    [SerializeField] private Vector3 boundsCenter = Vector3.zero;
    [SerializeField] private Vector3 boundsCullingCenter = Vector3.zero;

    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform avoidTarget;

    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float avoidanceRadius;
    [SerializeField] private float cohesionRadius;
    [SerializeField] private float alignmentRadius;

    [Range(0, 10)]
    [SerializeField] private float boundsStrength;
    [Range(0, 10)]
    [SerializeField] private float followStrength;
    [Range(0, 10)]
    [SerializeField] private float avoidStrength;
    [Range(0, 10)]
    [SerializeField] private float avoidanceStrength;
    [Range(0, 10)]
    [SerializeField] private float cohesionStrength;
    [Range(0, 10)]
    [SerializeField] private float alignmentStrength;

    private ComputeBuffer boidComputeBuffer;
    private BoidData[] cpuData;

    private float seed;
    private bool boidsInitialized;

    private int check = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boidComputeShader = Instantiate(boidComputeShader);

        seed = Random.Range(0, 10000);
        CreateBuffer();
        SpawnDispatch();
    }

    // Update is called once per frame
    void Update()
    {
        MoveDispatch();
    }

    private void OnDestroy()
    {
        boidComputeBuffer.Dispose();
    }

    void CreateBuffer()
    {
        boidComputeBuffer = new ComputeBuffer(boidAmount, (sizeof(float) * 6));
    }

    void SetBuffer(string kernel)
    {
        boidComputeShader.SetBuffer(boidComputeShader.FindKernel(kernel), "Boids", boidComputeBuffer);
    }

    void SetSpawnData(string kernel)
    {
        SetBuffer(kernel);

        boidComputeShader.SetFloat("time", Time.time);

        boidComputeShader.SetInt("boidAmount", boidAmount);

        boidComputeShader.SetVector("boundsExtents", boundsExtents);
        boidComputeShader.SetVector("boundsCenter", boundsCenter);

        boidComputeShader.SetFloat("seed", seed);
    }

    void SetMoveData(string kernel)
    {
        SetBuffer(kernel);

        boidComputeShader.SetFloat("deltaTime", Time.deltaTime);
        boidComputeShader.SetFloat("time", Time.time);

        boidComputeShader.SetInt("boidAmount", boidAmount);

        boidComputeShader.SetVector("boundsExtents", boundsExtents);
        boidComputeShader.SetFloat("boundsRadius", boundsRadius);
        boidComputeShader.SetVector("boundsCenter", boundsCenter);

        boidComputeShader.SetVector("followTarget", followTarget.position);
        boidComputeShader.SetFloat("followStrength", followStrength);

        boidComputeShader.SetVector("avoidTarget", avoidTarget.position);
        boidComputeShader.SetFloat("avoidStrength", avoidStrength);

        boidComputeShader.SetFloat("forwardSpeed", forwardSpeed);
        boidComputeShader.SetFloat("rotationSpeed", rotationSpeed);

        boidComputeShader.SetFloat("avoidanceRadius", avoidanceRadius);
        boidComputeShader.SetFloat("cohesionRadius", cohesionRadius);
        boidComputeShader.SetFloat("alignmentRadius", alignmentRadius);

        boidComputeShader.SetFloat("boundsStrength", boundsStrength);
        boidComputeShader.SetFloat("avoidanceStrength", avoidanceStrength);
        boidComputeShader.SetFloat("cohesionStrength", cohesionStrength);
        boidComputeShader.SetFloat("alignmentStrength", alignmentStrength);

        boidComputeShader.SetFloat("seed", seed);
    }

    void GetData()
    {
        if (cpuData == null)
            cpuData = new BoidData[boidAmount];

        if (cpuData != null)
            boidsInitialized = true;

        //boidComputeBuffer.GetData(cpuData);
    }

    void SpawnDispatch()
    {
        SetSpawnData("Spawn");

        int numThreads = 128;
        int groups = Mathf.CeilToInt((float)boidAmount / (float)numThreads);

        boidComputeShader.Dispatch(boidComputeShader.FindKernel("Spawn"), groups, 1, 1);
    }

    void MoveDispatch()
    {
        SetMoveData("Move");

        int numThreads = 128;
        int groups = Mathf.CeilToInt((float)boidAmount / (float)numThreads);
        
        boidComputeShader.Dispatch(boidComputeShader.FindKernel("Move"), groups, 1, 1);
    }

    public int GetBoidAmount() { return boidAmount; }
    public ComputeBuffer GetBoidComputeBuffer() { return boidComputeBuffer; }
    public Vector3 GetBoundsCenter() { return boundsCullingCenter; }
    public Vector3 GetBoundsExtents() { return boundsCullingExtents; }
}


struct BoidData
{
    public Vector3 pos;
    public Vector3 velocity;
}