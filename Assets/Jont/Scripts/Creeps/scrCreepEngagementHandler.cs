﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the logic necesarry to find out if this creep is engaged or not
/// </summary>
public class scrCreepEngagementHandler : MonoBehaviour
{
    private Creep thisCreep;
    public bool ThisCreepIsEngaged { get; private set; }
    private void Awake()
    {
        thisCreep = GetComponent<Creep>(); //Get the creep instance on this game object
    }
    public bool ToggleEngagement(Creep _target)
    {
        if(thisCreep == _target)
        {
            return ThisCreepIsEngaged = true; //This is true as long as the target for the defender is THIS creep
        }
        return ThisCreepIsEngaged = false;  //This is false if the target for the defender is THIS creep
    }
}
