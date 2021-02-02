using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "ScriptableObject/WaveSO")]
public class scrWaveSO : ScriptableObject
{
    [Header("Setup for wave")]
    public string waveName;
    [Tooltip("How much time it takes for this wave to spawn, after the game starts")]
    [SerializeField] private int SetWaveTimer;
    [HideInInspector] public float waveTimer;
    [Tooltip("Referenced by the wave spawner, sets the current number to equal this")]
    public int waveNumber;
    private int timeToShowCallWaveButton = 15;
    public bool WaveHasBeenCalled { get; private set; }

    public void CallButtonAppears(GameObject callWaveButton)
    {
        if (waveTimer <= timeToShowCallWaveButton && !callWaveButton.activeSelf) //Will this check that is is also not active?
        {
            callWaveButton.gameObject.SetActive(true);
        }
    }

    public void IncrementWaveTimer()
    {
        if (waveTimer >= 0f)
        {
            waveTimer -= Time.deltaTime;
        }
        else if (waveTimer < 0f)
        {
            WaveHasBeenCalled = true;
        }
    }
    public void ResetWaveSOVars()
    {
        waveTimer = SetWaveTimer;
        WaveHasBeenCalled = false;
    }

    public int SetWaveNumber()
    {
        //setCurrentWave = waveNumber;
        return waveNumber;
    }
}
