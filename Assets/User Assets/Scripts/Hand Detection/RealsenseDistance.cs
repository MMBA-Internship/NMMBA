using Intel.RealSense;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class RealsenseDistance : MonoBehaviour
{
	private Pipeline pipeline;

	// Depth range
	public float minDepth = 0.2f;
	public float maxDepth = 2f;

	// Detection region (top of camera)
	[Range(0f, 1f)]
	[SerializeField]
	private float topRegionStart = 0.45f;
	[Range(0f, 1f)]
	[SerializeField]
	private float topRegionEnd = 0.55f;

	private bool handDetected = false;

	[SerializeField] private float requiredDetectionTime = 0.2f;
	[SerializeField] private float detectionTime = 0f;
	[SerializeField] private float accumulatedTime = 0f;

	void Start()
	{
		TryConnect();
		GameEvents.On3DCameraSettingsSaved += UpdateSettings;
		GameEvents.OnTry3DCameraConnect += TryConnect;
	}

	private void TryConnect()
	{
		pipeline = new Pipeline();

		try
		{
			var config = new Config();
			config.EnableStream(Stream.Depth, 640, 480, Format.Z16, 30);
			pipeline.Start(config);
			enabled = true;
		}
		catch (Exception e)
		{
			Debug.LogError("RealSense camera not found or failed to start: " + e.Message);
			GameEvents.Raise3DCameraConnectionError(e);
			pipeline = null;

			// Don't run update
			enabled = false;
		}
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