using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scrLevelManager : Singleton<scrLevelManager> //This has also been turned into a singleton later, due to the need for a reference in the scrUIManager
    //script. (by the tutorial, not be)
{
    [SerializeField] private scrWaveSpawner waveSpawner; //This is only temporary, as this only allow us to use one wave spawner. The logic for handling the waves and updating
    //them needs to handled by its own class, or by this, as this is a singleton class.
    [SerializeField] private int lives = 10;

    public int TotalLives { get; set; }
    public int CurrentWaveForUI { get; set; }

    private void Start()
    {
        TotalLives = lives;
        CurrentWaveForUI = 0;
    }

    private void ReduceLives(Creep creep)
    {
        TotalLives--;
        if (TotalLives <= 0)
        {
            TotalLives = 0;
            //Game over
        }
    }

    private void GameStarted(object sender, System.EventArgs e)
    {
        //You might want to run some code here? Keeping this usefull event around for now
    }

    private void NewWave(object sender, System.EventArgs e)
    {
        CurrentWaveForUI += 1;
    }

    /*private void WaveCompleted()
    {
        CurrentWaveForUI++;
    }*/

    private void OnEnable()
    {
        //Subscribe to the event
        Creep.OnEndReaced += ReduceLives;
        waveSpawner.OnGameStart += GameStarted;
        waveSpawner.OnNewWave += NewWave;
        //scrOldSpawnerDeleteLater.OnWaveCompleted += WaveCompleted;
    }

    private void OnDisable()
    {
        //Desubscribe from the event
        Creep.OnEndReaced -= ReduceLives;
        waveSpawner.OnGameStart -= GameStarted;
        waveSpawner.OnNewWave -= NewWave;
        //scrOldSpawnerDeleteLater.OnWaveCompleted -= WaveCompleted;
    }
}
