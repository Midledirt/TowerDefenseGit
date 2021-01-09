using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrLevelManager : MonoBehaviour
{
    [SerializeField] private int lives = 10;

    public int TotalLives { get; set; }

    private void Start()
    {
        TotalLives = lives;
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
