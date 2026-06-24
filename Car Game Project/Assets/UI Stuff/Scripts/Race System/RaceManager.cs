using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI currentLapTimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;
    [SerializeField] private TextMeshProUGUI overallRaceTimeText;
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private TextMeshProUGUI checkpointMissedText;

    [Header("Race Settings")]
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private int lastCheckpointIndex = -1;
    [SerializeField] private bool isCircuit = false;
    [SerializeField] private int totalLaps = 1;
    [SerializeField] private string endMenuSceneName = "EndMenu";

    private int currentLap = 1;

    private bool raceStarted = false;
    private bool raceFinished = false;

    private bool ifcheckpointMissed = false;

    [Header("Lap Timer")]
    private float currentLapTime = 0f;
    private float overallRaceTime = 0f;
    private float bestLapTime = Mathf.Infinity;

    #region Unity Functions 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (raceStarted && !raceFinished)
        {
            UpdateTimers();
        }
        UpdateUI();
    }
    #endregion

    #region Checkpoint Management

    public void CheckPointReached(int checkpointIndex)
    {
        if (raceFinished) return;

        // Start line trigger to kick off the race session
        if (!raceStarted && checkpointIndex == 0)
        {
            StartRace();
            lastCheckpointIndex = 0;
            HideCheckpointMissedText();
            return;
        }

        if (!raceStarted) return;

        // Check if completing a lap on a circuit track loop
        bool validLapFinish = isCircuit && lastCheckpointIndex == checkpoints.Length - 1 && checkpointIndex == 0;

        if (validLapFinish)
        {
            HideCheckpointMissedText();
            OnLapFinish();
        }
        else if (checkpointIndex == lastCheckpointIndex + 1)
        {
            HideCheckpointMissedText();
            lastCheckpointIndex = checkpointIndex;

            // Check if completing a lap on a straight point-to-point track layout
            if (!isCircuit && checkpointIndex == checkpoints.Length - 1)
            {
                OnLapFinish();
            }
        }
        else
        {
            // Ignore accidental double-triggers of the exact same checkpoint frame
            if (checkpointIndex != lastCheckpointIndex)
            {
                ShowCheckpointMissedText();
            }
        }
    }

    #endregion

    #region Race Management 

    private void OnLapFinish()
    {
        // Save individual lap times into memory data registers
        PlayerPrefs.SetFloat("Lap" + currentLap + "Time", currentLapTime);

        if (currentLapTime < bestLapTime)
        {
            bestLapTime = currentLapTime;
        }

        if (currentLap >= totalLaps)
        {
            PlayerPrefs.SetFloat("BestLapTime", bestLapTime);
            EndRace();
        }
        else
        {
            currentLap++;
            currentLapTime = 0f;
            lastCheckpointIndex = 0;
        }
    }

    private void StartRace()
    {
        raceStarted = true;
        raceFinished = false;
        currentLap = 1;
        currentLapTime = 0f;
        overallRaceTime = 0f;

        // Clear any previous game run times from player storage memory keys
        PlayerPrefs.DeleteKey("Lap1Time");
        PlayerPrefs.DeleteKey("Lap2Time");
        PlayerPrefs.DeleteKey("BestLapTime");
    }

    private void EndRace()
    {
        raceFinished = true;
        raceStarted = false;
        StartCoroutine(HandleRaceEndSequence());
    }

    private IEnumerator HandleRaceEndSequence()
    {
        // Freeze game physics and updates completely
        Time.timeScale = 0f;

        // Wait for exactly half a second in real-world human time
        yield return new WaitForSecondsRealtime(0.5f);

        // Reset time tracking speed step back to normal speeds before scene shifts
        Time.timeScale = 1f;

        SceneManager.LoadScene(endMenuSceneName);
    }

    private void UpdateTimers()
    {
        currentLapTime += Time.deltaTime;
        overallRaceTime += Time.deltaTime;
    }

    private void UpdateUI()
    {
        currentLapTimeText.text = FormatTime(currentLapTime);
        overallRaceTimeText.text = FormatTime(overallRaceTime);

        if (raceFinished)
        {
            lapText.text = "Race Finished!";
        }
        else
        {
            lapText.text = "Lap: " + currentLap + "/" + totalLaps;
        }

        bestLapTimeText.text = FormatTime(bestLapTime);

        UpdateCheckPointMissedText();
    }

    private void UpdateCheckPointMissedText()
    {
        if (ifcheckpointMissed)
        {
            float alpha = Mathf.PingPong(Time.time * 2, 1);
            Color newColor = checkpointMissedText.color;
            newColor.a = alpha;
            checkpointMissedText.color = newColor;
        }
    }

    private void ShowCheckpointMissedText()
    {
        if (!ifcheckpointMissed)
        {
            checkpointMissedText.gameObject.SetActive(true);
            ifcheckpointMissed = true;
        }
    }

    private void HideCheckpointMissedText()
    {
        if (ifcheckpointMissed)
        {
            checkpointMissedText.gameObject.SetActive(false);
            ifcheckpointMissed = false;
        }
    }

    #endregion

    #region Utility Functions

    private string FormatTime(float time)
    {
        if (float.IsInfinity(time) || time < 0) return "--:--";
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }

    #endregion 
}