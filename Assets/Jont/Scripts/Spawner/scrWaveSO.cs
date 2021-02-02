using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "ScriptableObject/WaveSO")]
public class scrWaveSO : ScriptableObject
{
    [Header("Setup for wave")]
    public string waveName;
    [Tooltip("Referenced by the wave spawner, sets the current number to equal this")]
    public int waveNumber;
    [Tooltip("Sets how many seconds after game start this wave starts spawning creeps. When spawning a single wave from multiple spawners," +
        "set the same waves to the same timer. If you don`t want the actual creeps to spawn at the exact same time for each road, then set " +
        "their group timers to different times instead")]
    [SerializeField] private int setWaveTimer;
    [HideInInspector] public float waveTimer;
    public bool WaveHasBeenCalled { get; private set; }

    public void IncrementWaveTimer()
    {
        if (waveTimer >= 0f)
        {
            waveTimer -= Time.deltaTime;
        }
        else if (waveTimer < 0f)
        {
            //Debug.Log("I have counted down to 0");
            WaveHasBeenCalled = true;
        }
    }
    public void ResetWaveSOVars()
    {
        waveTimer = setWaveTimer;
        WaveHasBeenCalled = false;
    }

    public int SetWaveNumber()
    {
        //setCurrentWave = waveNumber;
        return waveNumber; //Returns the number of this wave, which is set in the inspector
    }
}
