using Intel.RealSense;
using UnityEngine;

public class RealsenseDistance : MonoBehaviour
{
    private Pipeline pipeline;

    // Depth range
    public float minDepth = 0.3f;
    public float maxDepth = 2f;

    // Depth  Unity Y mapping
    public float minY = 0f;  // closest hand  lower Y
    public float maxY = 4f;  // farthest hand  higher Y

    // Detection region (top of camera)
    [Range(0f, 1f)]
    public float topRegionStart = 0.0f;
    [Range(0f, 1f)]
    public float topRegionEnd = 0.25f;

    // Hand marker
    public GameObject handMarkerPrefab;
    private GameObject handMarkerInstance;
    public float handColliderRadius = 0.05f;

    // Output position
    public Vector3 handPosition;

    void Start()
    {
        pipeline = new Pipeline();

        var config = new Config();
        config.EnableStream(Stream.Depth, 640, 480, Format.Z16, 30);
        pipeline.Start(config);

        // Always spawn marker
        if (handMarkerPrefab != null)
        {
            handMarkerInstance = Instantiate(handMarkerPrefab);
            var sphere = handMarkerInstance.GetComponent<SphereCollider>();
            if (sphere != null)
                sphere.radius = handColliderRadius;
        }
    }

    void Update()
    {
        if (pipeline == null || handMarkerInstance == null)
            return;

        if (!pipeline.PollForFrames(out FrameSet frames))
            return;

        using (frames)
        {
            var depthFrame = frames.DepthFrame;
            if (depthFrame == null) return;

            using (depthFrame)
            {
                var profile = depthFrame.Profile.As<VideoStreamProfile>();
                if (profile == null) return;

                var intrinsics = profile.GetIntrinsics();
                int width = depthFrame.Width;
                int height = depthFrame.Height;

                int startY = (int)(height * topRegionStart);
                int endY = (int)(height * topRegionEnd);

                float sumDepth = 0f;
                float sumX = 0f;
                int count = 0;

                int step = 4;

                // --- Sample top region ---
                for (int y = startY; y < endY; y += step)
                {
                    for (int x = 0; x < width; x += step)
                    {
                        float d = depthFrame.GetDistance(x, y);
                        if (d <= 0) continue;

                        if (d >= minDepth && d <= maxDepth)
                        {
                            sumDepth += d;

                            float X = (x - intrinsics.ppx) / intrinsics.fx * d;
                            sumX += X;

                            count++;
                        }
                    }
                }

                float mappedY;
                float mappedX;

                if (count > 10)
                {
                    float avgDepth = sumDepth / count;
                    float avgX = sumX / count;

                    // Depth  Y mapping (further away = higher Y)
                    float t = (avgDepth - minDepth) / (maxDepth - minDepth);
                    mappedY = Mathf.Lerp(minY, maxY, t);  // flipped

                    mappedX = -avgX;
                }
                else
                {
                    // No detection: fallback
                    mappedY = minY;
                    mappedX = 0f;
                }

                handPosition = new Vector3(mappedX * 10, mappedY * 10, 0f);
                handMarkerInstance.transform.position = handPosition;

                Debug.Log($"Depth avg: {sumDepth / Mathf.Max(1, count):F2} to Y: {mappedY:F2} | X: {mappedX:F2}");
            }
        }
    }

    void OnDestroy()
    {
        if (pipeline != null)
        {
            pipeline.Stop();
            pipeline.Dispose();
            pipeline = null;
        }
    }
}