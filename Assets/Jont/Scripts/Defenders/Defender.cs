using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Defender : MonoBehaviour
{
    [Tooltip("Drag the body of the defender itself into this slot")]
    [SerializeField] private GameObject defenderBody;
    private scrAnimationEventHandler animEventHandler;
    private scrDefenderTowerTargets defenderTowerTargets;
    private scrDefenderMovement defenderMovement;

    public bool thisDefenderIsEngagedAsMainTarget { get; private set; }
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
        defenderIsAlive = true; //Used to make sure certain code is not run whilst the defender is respawning
        defenderIsAlreadyMovingTowardsTarget = false;
    }
    public scrDefenderTowerTargets AssignDefenderTowerTargets(scrDefenderTowerTargets _defenderTowerTargets)
    {
        //print("Local defender targets assigned to defender");
        return defenderTowerTargets = _defenderTowerTargets; //Gets the reference
    }
    public void NewCreepTargetInCollider(Creep _newCreep)
    {
        if(thisDefenderIsEngagedAsMainTarget == false && defenderIsAlive && defenderIsAlreadyMovingTowardsTarget == false) //If we are not already engaged && currently alive && we are not already moving towards a target
        {
            NewTargetToCheckOut(_newCreep);
        }
        //print("Spottet new enemy");
    }
    private void NewTargetToCheckOut(Creep _creepTarget) //This does not work yet...
    {
        defenderIsAlreadyMovingTowardsTarget = true;
        //Assign target as current target:
        CurrentCreepTarget = _creepTarget;
        //1.Find out if this target is already targeted by another defender, if so, prioritize a new target if possible
        scrCreepEngagementHandler _creepEngagementHandler = CurrentCreepTarget.GetComponent<scrCreepEngagementHandler>(); //Gets the engagement handler
        if(_creepEngagementHandler.CurrentTarget == null || _creepEngagementHandler.CurrentTarget == this) //Find out if the creep has a target already, or if "we" are it
        {
            //2.Move towards target
            defenderMovement.MakeDefenderMoveTowardsTarget(); //This will set the thisDefenderIsEngagedAsMainTarget to true, if the creep has no current targets

            //I THINK I KNOW THE SOLUTION. 
            //The problem seems to be that these defenders are all ASSINING THEMSELVES AS MAIN TARGET AT THE EXACT SAME TIME
            //The fix is: 
            //1. Run movement code.
            //2. Have a distance check
            //3. Then do the whole assign target thing. That way, it should never happen in the exact same frame for all defenders

            //3.Tell target that it is targeted
            _creepEngagementHandler.AddDefenderToCreepTargetsList(this); //Adds itself to the creep target list
            //_creepEngagementHandler.SetThisCreepIsEngaged(); //Sets the creep to engaged, if its not already true
        }
        else if(_creepEngagementHandler.CurrentTarget != null && _creepEngagementHandler.CurrentTarget != this.gameObject)
        {
            //4. No other targets? -> Engage
            if(defenderTowerTargets.DefenderCreepList.Count <= 1)
            {
                //2.Move towards target
                defenderMovement.MakeDefenderMoveTowardsTarget(); //This will set the thisDefenderIsEngagedAsMainTarget to true, if the creep has no current targets

                //3.Tell target that it is targeted
                //_creepEngagementHandler.SetThisCreepIsEngaged(); //Sets the creep to engaged, if its not already true
                _creepEngagementHandler.AddDefenderToCreepTargetsList(this); //Adds itself to the creep target list
                
                return; //Do not continue down this code, we do not need to look for new targets
            }
            else if(defenderTowerTargets.DefenderCreepList.Count > 1)
            {
                //5. Other targets? -> Look for other targets
                
                for (int i = 0; i < defenderTowerTargets.DefenderCreepList.Count; i++)
                {
                    //6. Engage other targets (this CANNOT run a loop that can restart THIS loop)
                    scrCreepEngagementHandler _newCreepEngagementHandler = defenderTowerTargets.DefenderCreepList[i].GetComponent<scrCreepEngagementHandler>();
                    if(_newCreepEngagementHandler.CurrentTarget == null || _newCreepEngagementHandler.CurrentTarget == this)
                    {
                        //2.Move towards target
                        defenderMovement.MakeDefenderMoveTowardsTarget(); //This will set the thisDefenderIsEngagedAsMainTarget to true, if the creep has no current targets

                        _newCreepEngagementHandler.AddDefenderToCreepTargetsList(this);
                        //_newCreepEngagementHandler.SetThisCreepIsEngaged();
                        //thisDefenderIsEngagedAsMainTarget = true;
                    }
                    else if(i >= defenderTowerTargets.DefenderCreepList.Count) //No more creeps in list
                    {
                        //2.Move towards target

                        
                        //3. Add defender to list
                        //_creepEngagementHandler.SetThisCreepIsEngaged(); //Sets the creep to engaged, if its not already true
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
            print("I died");
        }
    }
    private void WhenTargetDies(Creep _creep) //Reset engagement on target death
    {
        if(CurrentCreepTarget != null)
        {
            if (CurrentCreepTarget == _creep)
            {
                thisDefenderIsEngagedAsMainTarget = false;
            }
        }
    }
    public void SetDefenderIsEngagedAsMainTargetTrue()
    {
        thisDefenderIsEngagedAsMainTarget = true;
    }
    public void ResetDefenderIsEngagedAsMainTarget() //Called when rally point is updated/Changed from the scrDefenderMovement class 
    {
        thisDefenderIsEngagedAsMainTarget = false; 
        if(CurrentCreepTarget != null)
        {
            CurrentCreepTarget.GetComponent<scrCreepEngagementHandler>().RemoveDefenderFromList(this); //Remove this defender from creep targets list
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
