struct Boid
{
    float3 position;
    float3 velocity;
};

StructuredBuffer<Boid> boids;

void Instancing_float(
        float3 Position, 
        float3 Normal, 
        float3 Scale,
        float ID, 
        out float3 WorldPosition, 
        out float3 WorldNormal)
{
    Boid b = boids[(uint) ID];

    // Ensure velocity is non-zero
    float3 forward = normalize(b.velocity + 1e-5);
    float3 up = float3(0, 1, 0);

    // Build orthonormal basis
    float3 right = normalize(cross(up, forward));
    up = normalize(cross(forward, right));

    float3x3 rot = float3x3(right, up, forward);

    // Rotate & translate vertex
    Position *= Scale;
    float3 localPos = mul(Position, rot);
    float3 newPos = localPos + b.position;

    // Rotate normal
    float3 localNormal = normalize(mul(Normal, rot));

    WorldPosition = newPos;
    WorldNormal = localNormal;

}

