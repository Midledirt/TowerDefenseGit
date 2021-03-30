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

    private bool thisDefenderIsEngaged;
    public bool defenderIsAlive { get; private set; }
    private Creep currentCreepTarget;

    private void Awake()
    {
        defenderMovement = GetComponent<scrDefenderMovement>();
        animEventHandler = GetComponentInChildren<scrAnimationEventHandler>(); //Get the instance
    }
    private void Start()
    {
        thisDefenderIsEngaged = false;
        defenderIsAlive = true; //Used to make sure certain code is not run whilst the defender is respawning
    }
    public scrDefenderTowerTargets AssignDefenderTowerTargets(scrDefenderTowerTargets _defenderTowerTargets)
    {
        //print("Local defender targets assigned to defender");
        return defenderTowerTargets = _defenderTowerTargets; //Gets the reference
    }
    public void NewCreepTargetInCollider(Creep _newCreep)
    {
        if(thisDefenderIsEngaged == false && defenderIsAlive) //If we are not already engaged && currently alive
        {
            thisDefenderIsEngaged = true;
            NewTargetToCheckOut(_newCreep);
        }
        //print("Spottet new enemy");
    }
    private void NewTargetToCheckOut(Creep _creepTarget) //This does not work yet...
    {
        //Assign target as current target:
        currentCreepTarget = _creepTarget;
        //1.Find out if this target is already targeted by another defender, if so, prioritize a new target if possible
        scrCreepEngagementHandler _creepEngagementHandler = currentCreepTarget.GetComponent<scrCreepEngagementHandler>(); //Gets the engagement handler
        if(_creepEngagementHandler.CurrentTarget == null || _creepEngagementHandler.CurrentTarget == this.gameObject) //Find out if the creep has a target already, or if "we" are it
        {
            //2.Move towards target


            //3.Tell target that it is targeted
            _creepEngagementHandler.SetThisCreepIsEngaged(); //Sets the creep to engaged, if its not already true
            _creepEngagementHandler.AddDefenderToCreepTargetsList(this); //Adds itself to the creep target list
            
            return; //Do not continue down this code, we do not need to look for new targets
        }
        else if(_creepEngagementHandler.CurrentTarget != null && _creepEngagementHandler.CurrentTarget != this.gameObject)
        {
            //4. No other targets? -> Engage
            if(defenderTowerTargets.DefenderCreepList.Count <= 1)
            {
                //3.Tell target that it is targeted
                _creepEngagementHandler.SetThisCreepIsEngaged(); //Sets the creep to engaged, if its not already true
                _creepEngagementHandler.AddDefenderToCreepTargetsList(this); //Adds itself to the creep target list
                
                return; //Do not continue down this code, we do not need to look for new targets
            }
            else if(defenderTowerTargets.DefenderCreepList.Count > 1)
            {
                //5. Other targets? -> Look for other targets
                for (int i = 0; i < defenderTowerTargets.DefenderCreepList.Count; i++)
                {
                    //6. Engage other targets
                    NewTargetToCheckOut(defenderTowerTargets.DefenderCreepList[i]); //This should make the defender look for any new targets. If they are 
                    //found, this loop should end automaticly due to the "RETURN" statements abowe.
                    
                    //Else, it should assign to the last defender in the list?
                    //If not, (id defenders just stand around waiting for more creeps) then we will need to MAKE the defender attack the last target...

                    //if(i == defenderTowerTargets.DefenderCreepList.Count) //If we are at the last creep in the list
                    //{
                        
                    //}
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
            thisDefenderIsEngaged = false; //Reset engagement on death
            print("I died");
        }
    }
    private void WhenTargetDies(Creep _creep) //Reset engagement on target death
    {
        if(currentCreepTarget != null)
        {
            if (currentCreepTarget == _creep)
            {
                thisDefenderIsEngaged = false;
            }
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
