using System;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("Race Settings")]
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private int lastCheckpointIndex = -1;
    [SerializeField] private bool isCircut = false;
    [SerializeField] private int totalLaps = 1;

    private int currentLap = 1;


    private bool raceStarted = false;
    private bool raceFinished = false;

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
    #endregion


    #region Checkpoint Management

    public void CheckPointReached(int checkpointIndex)
    {
        if ((!raceStarted && checkpointIndex != 0) || raceFinished) return;
        if (checkpointIndex == lastCheckpointIndex + 1)
        {
            //UpdateCheckPoint();
        }

    }

    private void UpdateCheckPoint(int checkpointIndex)
    {
        if (checkpointIndex == 0)
        {
            if (!raceStarted)
            {
                StartRace();
            }
            else if (isCircut && lastCheckpointIndex == checkpoints.Length - 1 && raceStarted)
            {
                OnLapFinish();
            }
        }
        else if (!isCircut && checkpointIndex == checkpoints.Length - 1)
        {
            OnLapFinish();
        }
        lastCheckpointIndex = checkpointIndex;

    }



    #endregion

    #region Race Management 

    private void OnLapFinish()
    {
        currentLap++;
        if (currentLap > totalLaps)
        {
            EndRace();
        }
    }


    private void StartRace()
    {
        raceStarted = true;
        raceFinished = false;
    }


    private void EndRace()
    {
        raceFinished = true;
        raceStarted = false;
    }

    #endregion

}
