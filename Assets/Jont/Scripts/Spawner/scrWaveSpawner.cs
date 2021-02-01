using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class scrWaveSpawner : MonoBehaviour
{
    [SerializeField] private scrGroupSO[] groupArray;
    [SerializeField] private float timer;
    [Tooltip("Assign the middle part of this spawners path")]
    [SerializeField] private PathCreator middlePath;
    [Tooltip("Assign the left part of this spawners path")]
    [SerializeField] private PathCreator leftPath;
    [Tooltip("Assign the right part of this spawners path")]
    [SerializeField] private PathCreator rightPath;


    private void Awake()
    {
        foreach (scrGroupSO group in groupArray)
        {
            group.ResetVars(); //Sets the timers back to the value set in the inspector. Necessary, bacause SOs will save changes made in play mode after
                               //play mode has ended

            InitializeGroups(group); //Sets up the creeps and assignes paths
        }
    }

    private void InitializeGroups(scrGroupSO group)
    {
        PathCreator _creepPath = new PathCreator();
        if (group.GroupPath == 0)
        {
            Debug.Log("Spawning at the left path");
            _creepPath = middlePath;
            group.InitializeCreeps(_creepPath);
            return;
        }
        else if (group.GroupPath == 1)
        {
            Debug.Log("Spawning at the midle path");
            _creepPath = leftPath;
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

    private void Update() 
    {
        foreach (scrGroupSO group in groupArray)
        {
            group.IncrementWaveTimer();
            if (group.waveTimer <= 0)
            {
                group.SpawnCreep();  //Spawn creeps 
            }
        }
    }
}