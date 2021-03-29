using System.Collections;
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
}
