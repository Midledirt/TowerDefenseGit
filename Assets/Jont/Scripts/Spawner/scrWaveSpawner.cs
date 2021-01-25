using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrWaveSpawner : MonoBehaviour
{
    [SerializeField] private scrWaveSO[] waveArray;
    [SerializeField] private float timer;

    private void Awake()
    {
        foreach (scrWaveSO wave in waveArray)
        {
            wave.ResetTimers(); //Sets the timers back to the value set in the inspector. Necessary, bacause SOs will save changes made in play mode after
            //play mode has ended
            wave.InitializeCreeps(); // This should initialize the creep instances in awake, per wave
        }
    }

    private void Update()
    {
        foreach (scrWaveSO wave in waveArray)
        {
            wave.IncrementWaveTimer();
            if (wave.waveTimer <= 0)
            {
                wave.SpawnCreep();  //Spawn creeps
            }
        }
    }
}
