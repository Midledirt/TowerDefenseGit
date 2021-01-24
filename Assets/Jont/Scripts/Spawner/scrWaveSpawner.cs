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
            wave.InitializeCreeps(); // This should initialize the creep instances in awake, per wave
        }
    }

    private void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                foreach (scrWaveSO wave in waveArray)
                {
                    wave.SetCreepActive(); //Run this function for each wave. //YES! THIS WORKS
                }
            }
        }
        
    }
}
