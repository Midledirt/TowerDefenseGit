﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// When im done handling the targeting logic, i`ll need to write logic for defenders tackling new defenders if their current target died.
/// This should probably take the "distance along path" var into consideration
/// 
/// Currently, any defender that is not a MAINTARGET will always look for new targets each frame...
/// </summary>
public class Defender : MonoBehaviour
{
    [Tooltip("Drag the body of the defender itself into this slot")]
    [SerializeField] private GameObject defenderBody;
    private scrAnimationEventHandler animEventHandler;
    private scrDefenderTowerTargets defenderTowerTargets;
    private scrDefenderMovement defenderMovement;

    public bool thisDefenderIsEngagedAsMainTarget { get; private set; }
    public bool thisDefenderIsEngagedAsNoneTarget { get; private set; } //Make defender fight creep, there are more defenders than creep
    public bool defenderIsAlive { get; private set; }
    public Creep CurrentCreepTarget { get; set; } //Increase security if this code works
    public bool defenderIsAlreadyMovingTowardsTarget { get; set; } //Increase security if this code works

    private void Awake()
    {
        defenderMovement = GetComponent<scrDefenderMovement>();
        animEventHandler = GetComponentInChildren<scrAnimationEventHandler>(); //Get the instance
    }
    private void Start()
    {
        thisDefenderIsEngagedAsMainTarget = false;
        thisDefenderIsEngagedAsNoneTarget = false;
        defenderIsAlive = true; //Used to make sure certain code is not run whilst the defender is respawning
        defenderIsAlreadyMovingTowardsTarget = false;
    }
    private void Update()
    {
        //If defender is engaged as main or none target
            //Attack
    }
    public scrDefenderTowerTargets AssignDefenderTowerTargets(scrDefenderTowerTargets _defenderTowerTargets)
    {
        return defenderTowerTargets = _defenderTowerTargets; //Gets the reference
    }
    public void NewCreepTargetInCollider(Creep _newCreep)
    {
        if(thisDefenderIsEngagedAsMainTarget == false && defenderIsAlive && defenderIsAlreadyMovingTowardsTarget == false) //If we are not already engaged && currently alive && we are not already moving towards a target
        {
            NewTargetToCheckOut(_newCreep);
        }
    }
    private void NewTargetToCheckOut(Creep _creepTarget) //This does not work yet...
    {
        if(CurrentCreepTarget != null) //Update the list for the current (if any) creep target
        {
            CurrentCreepTarget.GetComponent<scrCreepEngagementHandler>().RemoveDefenderFromList(this);
        }
        defenderIsAlreadyMovingTowardsTarget = true;
        //Assign target as current target:
        CurrentCreepTarget = _creepTarget;
        //1.Find out if this target is already targeted by another defender, if so, prioritize a new target if possible
        scrCreepEngagementHandler _creepEngagementHandler = CurrentCreepTarget.GetComponent<scrCreepEngagementHandler>(); //Gets the engagement handler

        //2.Move towards target
        defenderMovement.MakeDefenderMoveTowardsTarget(); //This will set the thisDefenderIsEngagedAsMainTarget to true, if the creep has no current targets
        //3.Tell target that it is targeted
        _creepEngagementHandler.AddDefenderToCreepTargetsList(this); //Adds itself to the creep target list
    }
    public void ChceckForOtherTargets(Creep _creepTarget) //Called from defender movement
    {
        scrCreepEngagementHandler _creepEngagementHandler = _creepTarget.GetComponent<scrCreepEngagementHandler>(); //Gets the engagement handler
        if (defenderTowerTargets.DefenderCreepList.Count <= 1) 
        {
            if(thisDefenderIsEngagedAsNoneTarget)
            {
                return; //No need to run this code several times
            }
            print("Attacking only other option");
            SetDefenderIsEngagedAsNoneTarget(); //1. Engage anyway //This must be called before movement
                                                //2.Move towards target
            defenderMovement.MakeDefenderMoveTowardsTarget(); //This will set the thisDefenderIsEngagedAsMainTarget to true, if the creep has no current targets
                                                              //3.Tell target that it is targeted
            _creepEngagementHandler.AddDefenderToCreepTargetsList(this); //Adds itself to the creep target list

            return; //Do not continue down this code, we do not need to look for new targets
        }
        else if (defenderTowerTargets.DefenderCreepList.Count > 1)
        {
            //5. Other targets? -> Look for other targets       
            for (int i = 0; i < defenderTowerTargets.DefenderCreepList.Count; i ++)
            {
                print(defenderTowerTargets.DefenderCreepList.Count + " is the value for defenderTowerTargets Count");
                print(i + " is the value for i...");
                scrCreepEngagementHandler _newCreepEngagementHandler = defenderTowerTargets.DefenderCreepList[i].GetComponent<scrCreepEngagementHandler>();
                if (_newCreepEngagementHandler.ThisCreepIsEngaged == false || _newCreepEngagementHandler.CurrentTarget == this)
                {
                    print("I am now the main target for new creep"); //This probably works, but wont get called for the current setup of the test level
                                                                     //This will only happen if we have a new creep that entered whilst several defenders were moving towards another target 
                    defenderIsAlreadyMovingTowardsTarget = true;
                    CurrentCreepTarget = defenderTowerTargets.DefenderCreepList[i];
                    defenderMovement.MakeDefenderMoveTowardsTarget(); //This will set the thisDefenderIsEngagedAsMainTarget to true, if the creep has no current targets
                    _newCreepEngagementHandler.AddDefenderToCreepTargetsList(this);
                    //thisDefenderIsEngagedAsMainTarget = true;
                    return;
                }
                //6. Engage other targets (this CANNOT run a loop that can restart THIS loop)
                if (i >= defenderTowerTargets.DefenderCreepList.Count - 1) //No more creeps in list (need to implement - 1 because i wont increase IF DOING SO MAKES IT = TO THE COUNT. (not how i thought this worked...))
                {
                    if (_newCreepEngagementHandler.CurrentTarget != this)
                    {
                        print("No more unengaged targets code run");
                        defenderIsAlreadyMovingTowardsTarget = true;
                        CurrentCreepTarget = defenderTowerTargets.DefenderCreepList[i]; //Target is the last entry
                        SetDefenderIsEngagedAsNoneTarget(); //1. Engage anyway //This must be called before movement
                                                            //2.Move towards target
                        defenderMovement.MakeDefenderMoveTowardsTarget(); //This will set the thisDefenderIsEngagedAsMainTarget to true, if the creep has no current targets
                                                                          //3. Add defender to list
                        _creepEngagementHandler.AddDefenderToCreepTargetsList(this); //Adds itself to the creep target list

                        return; //Do not continue down this code, we do not need to look for new targets
                    }
                }
            }
        }
    }

    public void ResetIsAlive() //Called when the defender respawns. Handled by the animator script on this gameobject
    {
        defenderIsAlive = true; //Tested to work!
    }
    private void WhenIDie(Defender _defender)
    {
        if(this == _defender)
        {
            defenderIsAlive = false;
            thisDefenderIsEngagedAsMainTarget = false; //Reset engagement on death
            thisDefenderIsEngagedAsNoneTarget = false;
            print("I died");
        }
    }
    private void WhenTargetDies(Creep _creep) //Reset engagement on target death
    {
        if(CurrentCreepTarget != null)
        {
            if (CurrentCreepTarget == _creep)
            {
                CurrentCreepTarget = null;
                thisDefenderIsEngagedAsMainTarget = false;
                thisDefenderIsEngagedAsNoneTarget = false;
            }
        }
    }
    public void SetDefenderIsEngagedAsMainTargetTrue()
    {
        thisDefenderIsEngagedAsMainTarget = true;
        thisDefenderIsEngagedAsNoneTarget = false;
    }
    public void SetDefenderIsEngagedAsNoneTarget()
    {
        thisDefenderIsEngagedAsNoneTarget = true;
        thisDefenderIsEngagedAsMainTarget = false;
    }
    public void ResetDefenderIsEngagedAsMainOrNoneTarget() //Called when rally point is updated/Changed from the scrDefenderMovement class 
    {
        thisDefenderIsEngagedAsMainTarget = false;
        thisDefenderIsEngagedAsNoneTarget = false;
        if(CurrentCreepTarget != null)
        {
            CurrentCreepTarget.GetComponent<scrCreepEngagementHandler>().RemoveDefenderFromList(this); //Remove this defender from creep targets list
            CurrentCreepTarget = null;
        }
    }
    private void OnEnable()
    {
        scrUnitHealth.OnDefenderKilled += WhenIDie;
        scrUnitHealth.OnEnemyKilled += WhenTargetDies;
    }
    private void OnDisable()
    {
        scrUnitHealth.OnDefenderKilled -= WhenIDie;
        scrUnitHealth.OnEnemyKilled -= WhenTargetDies;
    }
}
