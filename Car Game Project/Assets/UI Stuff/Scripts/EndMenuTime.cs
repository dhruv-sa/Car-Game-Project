using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuDisplay : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI lap1TimeText;
    [SerializeField] private TextMeshProUGUI lap2TimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;

    void Start()
    {
        DisplayRaceResults();
    }

    private void DisplayRaceResults()
    {
        // Pull the stored float times from data memory (defaults to -1 if missing)
        float lap1Time = PlayerPrefs.GetFloat("Lap1Time", -1f);
        float lap2Time = PlayerPrefs.GetFloat("Lap2Time", -1f);
        float bestLapTime = PlayerPrefs.GetFloat("BestLapTime", -1f);

        // Format and print the values into your TextMeshPro components
        if (lap1TimeText != null) lap1TimeText.text = "Lap 1: " + FormatTime(lap1Time);
        if (lap2TimeText != null) lap2TimeText.text = "Lap 2: " + FormatTime(lap2Time);
        if (bestLapTimeText != null) bestLapTimeText.text = "Best Lap: " + FormatTime(bestLapTime);
    }

    private string FormatTime(float time)
    {
        if (time <= 0 || float.IsInfinity(time)) return "--:--";
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }

 
}