using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the logic necesarry to find out if this creep is engaged or not
/// </summary>
public class scrCreepEngagementHandler : MonoBehaviour
{
    private Creep thisCreep;
    private List<Defender> currentDefenderTargetsForThisCreep;
    public bool ThisCreepIsEngaged { get; private set; }
    public Defender CurrentTarget { get; private set; }
    private void Awake()
    {
        thisCreep = GetComponent<Creep>(); //Get the creep instance on this game object
    }
    private void Start()
    {
        currentDefenderTargetsForThisCreep = new List<Defender>();
        ThisCreepIsEngaged = false;
    }
    private void Update()
    {
        CreepEngageTarget();
    }
    public void SetThisCreepIsEngaged()
    {
        if(ThisCreepIsEngaged == false)
        {
            ThisCreepIsEngaged = true;
        }
    }
    public void AddDefenderToCreepTargetsList(Defender _newDefenderTarget)
    {
        if(_newDefenderTarget != null)
        {
            currentDefenderTargetsForThisCreep.Add(_newDefenderTarget); //Adds the new defender to local list
        }
    }
    private void CreepEngageTarget()
    {
        if(currentDefenderTargetsForThisCreep.Count <= 0) //Return if there are no current targets
        {
            ThisCreepIsEngaged = false;
            return;
        }
        else if(currentDefenderTargetsForThisCreep.Count > 0)
        {
            //print("I am engaged in combat!");
            thisCreep.StopMovement(); //1.Stop
            //2.Face target
            //3.Attack target
            CheckForDefenderDeath(); //4.Handle target death
        }
    }
    private void CheckForDefenderDeath()
    {
        CurrentTarget = currentDefenderTargetsForThisCreep[0];
        if(CurrentTarget.defenderIsAlive == false)
        {
            currentDefenderTargetsForThisCreep.Remove(CurrentTarget); //Remove it from the list
            CurrentTarget = null;
        }
    }
}
