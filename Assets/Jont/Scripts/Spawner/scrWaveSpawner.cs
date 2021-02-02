using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
/// <summary>
/// For future me:
/// Whilst functional, I do not think that the 
/// </summary>
public class scrWaveSpawner : MonoBehaviour
{
    [SerializeField] private scrWaveSO[] waveArray;
    [SerializeField] private scrGroupSO[] groupArray;
    [SerializeField] private float timer;
    [Tooltip("Assign the middle part of this spawners path")]
    [SerializeField] private PathCreator middlePath;
    [Tooltip("Assign the left part of this spawners path")]
    [SerializeField] private PathCreator leftPath;
    [Tooltip("Assign the right part of this spawners path")]
    [SerializeField] private PathCreator rightPath;
    [SerializeField] private GameObject startButton; //Find a better way of referencing this later
    [SerializeField] private GameObject callWaveButton;
    private bool gameHasStarted; //This might be outdated when i make the gamestart into an actual event
    public event EventHandler OnGameStart;
    public event EventHandler OnNewWave;
    public int CurrentWave { get; set; } //Increment this when any specific wave is over, so that a wave var can be displayed at the screen

    private void Awake()
    {
        gameHasStarted = false;
        CurrentWave = 0;
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
    private void Update() 
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
                        CurrentWave = wave.SetWaveNumber(); //problem, this will set the current wave to 1 and then to 2 every frame afther they have come true
                        OnNewWave?.Invoke(this, EventArgs.Empty);
                        Debug.Log("The wave was just set to: " + CurrentWave);
                    }
                }
            }
        }

        //This needs to be bound up to a condition checking that the group can be "called"
        foreach (scrGroupSO group in groupArray)
        {
            if (CurrentWave >= group.GroupWavePlacement) //Only increment the group timer if its wave has spawned
            {
                group.IncrementGroupTimer(); //Makes the group count down
                if (group.groupTimer <= 0)
                {
                    group.SpawnCreep();  //Spawn creeps 
                }
            }
        }
    }
    public void StartTheGame() //MAKE THIS INTO AN EVENT LATER, SO THAT MANY DIFFERENT CLASSES CAN REACT TO IT!
    {
        OnGameStart?.Invoke(this, EventArgs.Empty);
        CurrentWave = 1; //Start the first wave
        startButton.SetActive(false);
        gameHasStarted = true;
        Debug.Log("The game has started");
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
    //Used to set this wave as active, but also record how much time was skipped, if time was skipped, and then subtract that time from other waves
    public void CallWave()
    {
        //CurrentWave = Should equal wave number
        callWaveButton.gameObject.SetActive(false);
    }
}