using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{

    public TextMeshProUGUI fpsCounterText;
    [SerializeField] private float updatePeriod = 0.5f;
    [SerializeField] private int lowFpsThreshold = 60;

    private float fpsAvg = 0f;
    private float lastUpdated = 0f;

    void Update()
    {
        if (Time.time - lastUpdated > updatePeriod)
        {
            float fps = 1.0f / Time.unscaledDeltaTime;
            fpsAvg = (fpsAvg + fps) / 2;
            lastUpdated = Time.time;
        }
        string text = $"FPS Counter: {fpsAvg:0.}";

        fpsCounterText.text = text;

    }

}
