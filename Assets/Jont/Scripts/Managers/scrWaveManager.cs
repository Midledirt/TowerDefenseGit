using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scrWaveManager : MonoBehaviour
{
    [Header("Assign spawners")]
    [Tooltip("Each spawner is a new path for creeps. Only ONE is necessary for a single level")]
    public List<scrWaveSpawnerSO> spawners;
    private scrLevelManager levelManager;
    public int CurrentWave { get; private set; }


    private void Awake()
    {
        //Get the LevelManager Instance
        levelManager = scrLevelManager.Instance.GetComponent<scrLevelManager>();

        foreach (scrWaveSpawnerSO _spawner in spawners)
        {
            _spawner.ResetWave();
        }
    }
    private void Update()
    {
        foreach (scrWaveSpawnerSO _spawner in spawners)
        {
            _spawner.UpdateWaveAndGroups();
            levelManager.CurrentWaveForUI = _spawner.currentWave; //Update the UI
        }
    }

    public void GameStarted(object sender, EventArgs e)
    {
        //currentWave = 1; //Starts the game at wave 1 This is where the fun starts. Make this "currentWave" war take from a number which you also use to
        //update the wave in the UI
        foreach (scrWaveSpawnerSO _spawner in spawners)
        {
            _spawner.gameHasStarted = true;
            _spawner.currentWave = 1;
        }
    }

    private void OnEnable()
    {
        //Subscribe to the event
        levelManager.OnGameStart += GameStarted;
    }

    private void OnDisable()
    {
        //Desubscribe from the event
        levelManager.OnGameStart -= GameStarted;
    }
}
