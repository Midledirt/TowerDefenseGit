using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[CreateAssetMenu(menuName = "ScriptableObject/WaveSpawner")]
public class scrWaveSpawnerSO : ScriptableObject
{
    [SerializeField] private scrWaveSO[] waveArray;
    [SerializeField] private scrGroupSO[] groupArray;
    [Tooltip("Assign the middle part of this spawners path")]
    [SerializeField] private PathCreator middlePath;
    [Tooltip("Assign the left part of this spawners path")]
    [SerializeField] private PathCreator leftPath;
    [Tooltip("Assign the right part of this spawners path")]
    [SerializeField] private PathCreator rightPath;
    [HideInInspector] public int currentWave;
    [HideInInspector] public bool gameHasStarted;

    public void ResetWave() //Run in Awake
    {
        currentWave = 0;
        gameHasStarted = false;
        foreach (scrWaveSO wave in waveArray)
        {
            wave.ResetWaveSOVars();
        }
        foreach (scrGroupSO group in groupArray)
        {
            group.ResetVars(); //Sets the timers back to the value set in the inspector. Necessary, bacause SOs will save changes made in play mode after
                               //play mode has ended
            InitializeGroups(group); //Sets up the creeps and assignes paths
        }
    }
    public void UpdateWaveAndGroups() //Run in Update
    {
        foreach (scrWaveSO wave in waveArray)
        {
            if (gameHasStarted)
            {
                if (!wave.WaveHasBeenCalled)
                {
                    wave.IncrementWaveTimer(); //This works for now, but if i can switch this for events instad... then the code can be called only once
                    if (wave.waveTimer <= 0f)
                    {
                        currentWave = wave.SetWaveNumber(); //problem, this will set the current wave to 1 and then to 2 every frame afther they have come true
                        Debug.Log("The wave was just set to: " + currentWave);
                    }
                }
            }
        }
        foreach (scrGroupSO group in groupArray)
        {
            if (currentWave >= group.GroupWavePlacement) //Only increment the group timer if its wave has spawned
            {
                group.IncrementGroupTimer(); //Makes the group count down
                if (group.groupTimer <= 0)
                {
                    group.SpawnCreep();  //Spawn creeps 
                }
            }
        }
    }

    private void InitializeGroups(scrGroupSO group)
    {
        PathCreator _creepPath;
        if (group.GroupPath == 0)
        {
            Debug.Log("Spawning at the left path");
            _creepPath = leftPath;
            group.InitializeCreeps(_creepPath);
            return;
        }
        else if (group.GroupPath == 1)
        {
            Debug.Log("Spawning at the midle path");
            _creepPath = middlePath;
            group.InitializeCreeps(_creepPath);
            return;
        }
        else if (group.GroupPath == 2)
        {
            Debug.Log("Spawning at the right path");
            _creepPath = rightPath;
            group.InitializeCreeps(_creepPath);
            return;
        }
    }
}