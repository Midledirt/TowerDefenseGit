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
    //I MUST set a specific reference to whatever defender this creep IS IN combat with. So that this creep can return to walking if that defender dies or is
    //moved

    private void Awake()
    {
        thisCreep = GetComponent<Creep>(); //Get the creep instance on this game object
    }
    private void Start()
    {
        currentDefenderTargetsForThisCreep = new List<Defender>();
        ThisCreepIsEngaged = false;
        CurrentTarget = null; //starts with no target
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
            ReturnToPath();
            return;
        }
        else if(currentDefenderTargetsForThisCreep.Count > 0)
        {
            //print("I am engaged in combat!");
            thisCreep.StopMovement(); //1.Stop
            //2.Face target
            //3.Attack target
            CheckForDefenderDeath(); //4.Handle target death.
        }
    }
    private void CheckForDefenderDeath()
    {
        CurrentTarget = currentDefenderTargetsForThisCreep[0];
        if(CurrentTarget.defenderIsAlive == false)
        {
            currentDefenderTargetsForThisCreep.Remove(CurrentTarget); //Remove it from the list
            if(currentDefenderTargetsForThisCreep.Count <= 0)
            {
                CurrentTarget = null; //Set this to the next potential defender instead
                ReturnToPath();
            }
            else //Get the next defender //THIS MAY BE FAULTY, AS I DO NOT KNOW IF THE INDEX FOR THE LIST IS UPDATED WHEN AN ENTRY IS REMOVED
            {
                CurrentTarget = currentDefenderTargetsForThisCreep[0];
            }
        }
    }
    private void ReturnToPath()
    {
        if(CurrentTarget != null) //Checks that we have no targets.
        {
            return;
        }
        thisCreep.ResumeMovement();
    }
    public void RemoveDefenderFromList(Defender _defender) //Removes the defender from target list if it is in the target list
    {
        if(currentDefenderTargetsForThisCreep.Contains(_defender))
        {
            currentDefenderTargetsForThisCreep.Remove(_defender);
        }
    }
    private void DefenderRallyPointMoved(Creep _creep, List<Defender> _defenders) //Remove defenders from list
    {
        if(thisCreep == _creep) //If we are affected
        {
            //print("Rally point moved"); //works
            foreach(Defender defender in _defenders)
            {
                if(currentDefenderTargetsForThisCreep.Contains(defender))
                {
                    currentDefenderTargetsForThisCreep.Remove(defender); //This works!
                    if(CurrentTarget == defender)
                    {
                        CurrentTarget = null; //Reset the current target
                        ReturnToPath();
                    }
                }
                if(currentDefenderTargetsForThisCreep.Count <= 0) //This is necessary
                {
                    CurrentTarget = null; //Reset the current target
                    ReturnToPath();
                }
            }            
        }
    }
    private void OnEnable()
    {
        scrDefenderTowerTargets.LooseDefenderTarget += DefenderRallyPointMoved;
    }
    private void OnDisable()
    {
        scrDefenderTowerTargets.LooseDefenderTarget -= DefenderRallyPointMoved;
    }
}
