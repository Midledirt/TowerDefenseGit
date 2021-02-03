using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scrLevelManager : Singleton<scrLevelManager> //This has also been turned into a singleton later, due to the need for a reference in the scrUIManager
                                                          //script. (by the tutorial, not be)
{
    [SerializeField] private GameObject callWaveButton;
    [SerializeField] private GameObject startButton; //Find a better way of referencing this later
    public event EventHandler OnGameStart;
    [SerializeField] private int lives = 10;
    public int TotalLives { get; set; }
    public int CurrentWaveForUI { get; set; }
    public int CurrentWave { get; set; } //Increment this when any specific wave is over, so that a wave var can be displayed at the screen

    private void Start()
    {
        TotalLives = lives;
        CurrentWaveForUI = 0;
        CurrentWave = 0;
    }
    public void StartTheGame() //MAKE THIS INTO AN EVENT LATER, SO THAT MANY DIFFERENT CLASSES CAN REACT TO IT!
    {
        OnGameStart?.Invoke(this, EventArgs.Empty);
        CurrentWave = 1; //Start the first wave
        CurrentWaveForUI = 1;
        startButton.SetActive(false);
        Debug.Log("The game has started");
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
    public void CallWave()
    {
        //CurrentWave = Should equal wave number
        callWaveButton.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //Subscribe to the event
        Creep.OnEndReaced += ReduceLives;
    }
    private void OnDisable()
    {
        //Desubscribe from the event
        Creep.OnEndReaced -= ReduceLives;
    }
}
