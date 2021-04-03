using System.Collections;
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
    private scrDefenderAnimation defenderAnimator;
    private scrAnimationEventHandler animEventHandler;
    private scrDefenderTowerTargets defenderTowerTargets;
    private scrDefenderMovement defenderMovement;
    private scrCreepEngagementHandler creepEngagementHandler; 
    public scrUnitHealth DefenderHealth { get; private set; } //Used by the creep to deal damage

    public bool thisDefenderIsEngagedAsMainTarget { get; private set; }
    public bool thisDefenderIsEngagedAsNoneTarget { get; private set; } //Make defender fight creep, there are more defenders than creep
    public bool defenderIsAlive { get; private set; }
    public Creep CurrentCreepTarget { get; set; } //Increase security if this code works
    public bool defenderIsAlreadyMovingTowardsTarget { get; set; } //Increase security if this code works

    private float longestTargetDistance; //Used to find new creep targets for defenders when they have killed their current target

    private void Awake()
    {
        DefenderHealth = GetComponent<scrUnitHealth>(); //Gets the reference
        defenderAnimator = GetComponent<scrDefenderAnimation>(); //Gets the instance
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
            defenderIsAlreadyMovingTowardsTarget = true;
            SetDefenderIsEngagedAsNoneTarget();
            defenderMovement.MakeDefenderMoveTowardsTarget();
            return; //Do not continue down this code, we do not need to look for new targets
        }
        else if (defenderTowerTargets.DefenderCreepList.Count > 1)
        {
            //5. Other targets? -> Look for other targets       
            for (int i = 0; i < defenderTowerTargets.DefenderCreepList.Count; i ++)
            {
                scrCreepEngagementHandler _newCreepEngagementHandler = defenderTowerTargets.DefenderCreepList[i].GetComponent<scrCreepEngagementHandler>();
                if (_newCreepEngagementHandler.ThisCreepIsEngaged == false || _newCreepEngagementHandler.CurrentTarget == this)
                {
                    defenderIsAlreadyMovingTowardsTarget = true;
                    CurrentCreepTarget = defenderTowerTargets.DefenderCreepList[i];
                    defenderMovement.MakeDefenderMoveTowardsTarget(); 
                    _newCreepEngagementHandler.AddDefenderToCreepTargetsList(this);
                    return;
                }
                //6. Engage other targets (this CANNOT run a loop that can restart THIS loop)
                if (i >= defenderTowerTargets.DefenderCreepList.Count - 1) //No more creeps in list (need to implement - 1 because i wont increase IF DOING SO MAKES IT = TO THE COUNT. (not how i thought this worked...))
                {
                    if (_newCreepEngagementHandler.CurrentTarget != this && CurrentCreepTarget != defenderTowerTargets.DefenderCreepList[i])
                    {
                        //print("No more unengaged targets code run");
                        defenderIsAlreadyMovingTowardsTarget = true;
                        CurrentCreepTarget = defenderTowerTargets.DefenderCreepList[i]; //Target is the last entry
                        SetDefenderIsEngagedAsNoneTarget();
                        defenderMovement.MakeDefenderMoveTowardsTarget();
                        _creepEngagementHandler.AddDefenderToCreepTargetsList(this); //Adds itself to the creep target list
                        return; //Do not continue down this code, we do not need to look for new targets
                    }
                }
            }
        }
    }
    public void LookForNewTarget()
    {
        //print("Looking for new target...");
        longestTargetDistance = 0f; //Reset the longest target distance
        if(defenderTowerTargets.DefenderCreepList.Count <= 0 || thisDefenderIsEngagedAsMainTarget)
        {
            //print("There are no new targets...");
            return; //No targets, return
        }
        StartCoroutine(WaitForCreepCheck()); //This coroutine delays the check, so that the creep vars can update before the check
    }
    private Creep CheckForUnengagedTargets()
    {
        longestTargetDistance = 0f;
        Creep _potentialNewTarget = null;
        print("Running unengaged check...");
        for(int i = 0; i < defenderTowerTargets.DefenderCreepList.Count; i++)
        {
            if (defenderTowerTargets.DefenderCreepList[i].DistanceTravelled > longestTargetDistance && !defenderTowerTargets.DefenderCreepList[i].GetComponent<scrCreepEngagementHandler>().ThisCreepIsEngaged)
            {
                print("Found a new target pertaining to the standards...");
                longestTargetDistance = defenderTowerTargets.DefenderCreepList[i].DistanceTravelled;
                _potentialNewTarget = defenderTowerTargets.DefenderCreepList[i];
            }
            if (i >= defenderTowerTargets.DefenderCreepList.Count - 1) //does it NOT loop if this condidion is not checked?
            {
                print("Check complete...");
                if (_potentialNewTarget != null)
                {
                    print("Returning target");
                    return _potentialNewTarget;
                }
                else
                    print("Returning null");
                    return null;
            }
        }
        return null;
    }
    private void CheckForEngagedTargets()
    {
        longestTargetDistance = 0f;
        for (int i = 0; i < defenderTowerTargets.DefenderCreepList.Count; i++)
        {
            if (defenderTowerTargets.DefenderCreepList.Count <= 0)
            {
                //print("There are no new targets...");
                return; //No targets, return
            }
            Creep _potentialNewTarget = null;
            float newLength = defenderTowerTargets.DefenderCreepList[i].DistanceTravelled;
            if (newLength >= longestTargetDistance)
            {
                longestTargetDistance = newLength; //Update longest distance
                _potentialNewTarget = defenderTowerTargets.DefenderCreepList[i];
            }
            if (i >= defenderTowerTargets.DefenderCreepList.Count - 1) //If we are at the end of the list
            {
                if (_potentialNewTarget != null)
                {
                    print("...Found new already engaged target!");
                    NewTargetToCheckOut(_potentialNewTarget);
                    return;
                }
                else
                    print("Found no new targets...");
                    return;
            }

        }
        return;
    }
    public void ResetIsAlive() //Called when the defender respawns. Handled by the animator script on this gameobject
    {
        defenderIsAlive = true; //Tested to work!
        LookForNewTarget(); //Look for a new target on respawn!
    }
    private void WhenDefenderDies(Defender _defender)
    {
        if(this == _defender)
        {
            defenderIsAlive = false;
            thisDefenderIsEngagedAsMainTarget = false; //Reset engagement on death
            thisDefenderIsEngagedAsNoneTarget = false;
            defenderAnimator.StopAttackAnimation();
            //print("I died");
        }
        else if(this != _defender && !thisDefenderIsEngagedAsMainTarget && defenderIsAlive && !defenderIsAlreadyMovingTowardsTarget)
        {
            print("Some other defender died and I am free to help... I better help out!");
            LookForNewTarget();
        }
    }
    private void WhenTargetDies(Creep _creep) //Reset engagement on target death
    {
        if (CurrentCreepTarget != null)
        {
            if (CurrentCreepTarget == _creep)
            {
                CurrentCreepTarget = null;
                thisDefenderIsEngagedAsMainTarget = false;
                thisDefenderIsEngagedAsNoneTarget = false;
                defenderAnimator.StopAttackAnimation();
                LookForNewTarget(); //1. This one only runs if the defender just killed ITS target
            }
        }
    }
    private IEnumerator WaitForCreepCheck()
    {
        yield return new WaitForSeconds(0.2f);
        Creep newPotentialTarget = CheckForUnengagedTargets(); //Are there any unengaged targets?
        if(newPotentialTarget != null)
        {
            NewTargetToCheckOut(newPotentialTarget);
        }
        else if (newPotentialTarget == null)
        {
            CheckForEngagedTargets(); //No? Check for unengaged ones
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
    private void DealDamageToTarget(float _damage)
    {
        if(CurrentCreepTarget == null || CurrentCreepTarget._CreepHealth.CurrentHealth <= 0f)
        {
            return; //No alive target to damage
        }
        CurrentCreepTarget._CreepHealth.DealDamage(_damage);
    }
    private void OnEnable()
    {
        scrUnitHealth.OnDefenderKilled += WhenDefenderDies;
        scrUnitHealth.OnEnemyKilled += WhenTargetDies;
        animEventHandler.OnDealingDamage += DealDamageToTarget;
    }
    private void OnDisable()
    {
        scrUnitHealth.OnDefenderKilled -= WhenDefenderDies;
        scrUnitHealth.OnEnemyKilled -= WhenTargetDies;
        animEventHandler.OnDealingDamage -= DealDamageToTarget;
    }
}
