using Intel.RealSense;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class RealsenseDistance : MonoBehaviour
{
	private Pipeline pipeline;

	// Depth range
	// TODO: make configurable on startup of game
	public float minDepth = 0.2f;
	public float maxDepth = 2f;

	// Depth  Unity Y mapping
	//public float minY = 0f;  // closest hand  lower Y
	//public float maxY = 4f;  // farthest hand  higher Y

	// Detection region (top of camera)
	[Range(0f, 1f)]
	[SerializeField]
	private float topRegionStart = 0.45f;
	[Range(0f, 1f)]
	[SerializeField]
	private float topRegionEnd = 0.55f;

	// Hand marker
	//[SerializeField] private GameObject handMarkerPrefab;
	//private GameObject handMarkerInstance;

	// Output position
	//[SerializeField] private Vector3 handPosition;

	private bool handDetected = false;

	[SerializeField] private float requiredDetectionTime = 0.2f;
	[SerializeField] private float detectionTime = 0f;
	[SerializeField] private float accumulatedTime = 0f;

	void Start()
	{
		pipeline = new Pipeline();

		var config = new Config();
		config.EnableStream(Stream.Depth, 640, 480, Format.Z16, 30);
		pipeline.Start(config);

		// Always spawn marker
		//if (handMarkerPrefab != null)
		//{
		//    handMarkerInstance = Instantiate(handMarkerPrefab);
		//}

		GameEvents.On3DCameraSettingsSaved += UpdateSettings;
	}

	private void UpdateSettings(float maxDepth, float minDepth, float regionStart, float regionEnd)
	{
		this.maxDepth = maxDepth;
		this.minDepth = minDepth;
		this.topRegionStart = regionStart;
		this.topRegionEnd = regionEnd;
	}

	void Update()
	{
		accumulatedTime += Time.deltaTime;

		//if (pipeline == null || handMarkerInstance == null)
		//    return;

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

				//float mappedY;
				//float mappedX;

				if (count > 10)
				{
					detectionTime += accumulatedTime;
					accumulatedTime = 0f;
				}
				else
				{
					detectionTime -= accumulatedTime;
					accumulatedTime = 0f;
				}

				detectionTime = Mathf.Clamp(detectionTime, 0f, requiredDetectionTime);
				bool detectedThisFrame = detectionTime >= requiredDetectionTime;

				//if (detectedThisFrame)
				//{
				//    float avgDepth = sumDepth / count;
				//    float avgX = sumX / count;

				//    // Depth  Y mapping (further away = higher Y)
				//    float t = (avgDepth - minDepth) / (maxDepth - minDepth);
				//    mappedY = Mathf.Lerp(minY, maxY, t);  // flipped

				//    mappedX = -avgX;
				//}
				//else
				//{
				//    mappedY = minY;
				//    mappedX = 0f;
				//}

				//handPosition = new Vector3(mappedX * 10, mappedY * 10, 0f);
				//handMarkerInstance.transform.position = handPosition;

				//Debug.Log($"Depth avg: {sumDepth / Mathf.Max(1, count):F2} to Y: {mappedY:F2} | X: {mappedX:F2}");

				if (detectedThisFrame && !handDetected)
				{
					handDetected = true;
					GameEvents.RaiseHandEntered();
				}
				else if (!detectedThisFrame && handDetected && detectionTime == 0f)
				{
					handDetected = false;
					GameEvents.RaiseHandExited();
				}
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