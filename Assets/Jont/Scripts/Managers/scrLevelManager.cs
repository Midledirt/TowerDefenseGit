using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrLevelManager : Singleton<scrLevelManager> //This has also been turned into a singleton later, due to the need for a reference in the scrUIManager
    //script. (by the tutorial, not be)
{
    [SerializeField] private int lives = 10;

    public int TotalLives { get; set; }
    public int CurrentWave { get; set; }

    private void Start()
    {
        TotalLives = lives;
        CurrentWave = 1;
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

    private void WaveCompleted()
    {
        CurrentWave++;
    }

    private void OnEnable()
    {
        //Subscribe to the event
        Creep.OnEndReaced += ReduceLives;
        scrSpawner.OnWaveCompleted += WaveCompleted;
    }

    private void OnDisable()
    {
        //Desubscribe from the event
        Creep.OnEndReaced -= ReduceLives;
        scrSpawner.OnWaveCompleted -= WaveCompleted;
    }
}
