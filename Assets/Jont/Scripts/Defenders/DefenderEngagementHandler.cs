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
public class DefenderEngagementHandler : MonoBehaviour
{
    private scrDefenderAnimation defenderAnimator;
    private scrAnimationEventHandler animEventHandler;
    private scrDefenderTowerTargets defenderTowerTargets;

    public scrDefenderTowerTargets DefenderTowerTargetsReference { get; private set; }

    private scrDefenderMovement defenderMovement;
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
        DefenderTowerTargetsReference = _defenderTowerTargets; //Sets the reference for the creep
        return defenderTowerTargets = _defenderTowerTargets; //Gets the reference
    }
    public void DefenderEngageNewTargetAsNone(Creep _target)
    {
        if(_target == null || !defenderTowerTargets.DefenderCreepList.Contains(_target))
        {
            print("Was attempting to engage target, but it dissapeared...");
            CurrentCreepTarget = null;
            LookForNewTarget();
            return;
        }
        if (CurrentCreepTarget != null)
        {
            CurrentCreepTarget.GetComponent<scrCreepEngagementHandler>().RemoveDefenderFromList(this);
        }
        thisDefenderIsEngagedAsNoneTarget = true;
        thisDefenderIsEngagedAsMainTarget = false;
        InformCreepTarget(_target);
    }
    public void DefenderEngageNewTargetAsMain(DefenderEngagementHandler _defender, Creep _target)
    {
        if(_defender != this || _target == null || !defenderTowerTargets.DefenderCreepList.Contains(_target))
        {
            print("Was attempting to engage target, but it dissapeared...");
            CurrentCreepTarget = null;
            LookForNewTarget();
            return;
        }
        if(CurrentCreepTarget != null)
        {
            CurrentCreepTarget.GetComponent<scrCreepEngagementHandler>().RemoveDefenderFromList(this);
        }
        thisDefenderIsEngagedAsMainTarget = true;
        thisDefenderIsEngagedAsNoneTarget = false;
        InformCreepTarget(_target);
    }
    private void InformCreepTarget(Creep _target)
    {
        CurrentCreepTarget = _target;
        scrCreepEngagementHandler _currentTargetEngagementHandler = CurrentCreepTarget.GetComponent<scrCreepEngagementHandler>();
        _currentTargetEngagementHandler.AddDefenderToCreepTargetsList(this);
        _currentTargetEngagementHandler.SetThisCreepIsEngaged();
        defenderMovement.MakeDefenderMoveTowardsTarget();
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
        print("Looking for unengaged target");
        longestTargetDistance = 0f;
        Creep newTarget = null;
        foreach(Creep _target in defenderTowerTargets.DefenderCreepList)
        {
            scrCreepEngagementHandler _targetEngagementHandler = _target.GetComponent<scrCreepEngagementHandler>();
            if(_target.DistanceTravelled > longestTargetDistance && !_targetEngagementHandler.ThisCreepIsEngaged)
            {
                print("Found unengaged target");
                longestTargetDistance = _target.DistanceTravelled;
                newTarget = _target;
            }
        }
        return newTarget;
    }
    private Creep CheckForNewTargets()
    {
        print("Looking for any target");
        longestTargetDistance = 0f;
        Creep newTarget = null;
        foreach (Creep _target in defenderTowerTargets.DefenderCreepList)
        {
            if (_target.DistanceTravelled > longestTargetDistance)
            {
                print("Found a target");
                longestTargetDistance = _target.DistanceTravelled;
                newTarget = _target;
            }
        }
        return newTarget;
    }
    public void ResetIsAlive() //Called when the defender respawns. Handled by the animator script on this gameobject
    {
        defenderIsAlive = true; //Tested to work!
        CurrentCreepTarget = null;
        thisDefenderIsEngagedAsMainTarget = false;
        thisDefenderIsEngagedAsNoneTarget = false;
        LookForNewTarget(); //Look for a new target on respawn!
    }
    private void WhenDefenderDies(DefenderEngagementHandler _defender)
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
            //print("Some other defender died and I am free to help... I better help out!");
            LookForNewTarget();
        }
    }
    public void DefenderRemovedByCreep(DefenderEngagementHandler _defender)
    {
        if(this != _defender)
        {
            return;
        }
        thisDefenderIsEngagedAsMainTarget = false;
        thisDefenderIsEngagedAsNoneTarget = false;
        CurrentCreepTarget = null;
        defenderAnimator.StopAttackAnimation();
        if(defenderIsAlive)
        {
            LookForNewTarget();
        }
    }
    private void WhenTargetDies(Creep _creep) //Reset engagement on target death
    {
        if (CurrentCreepTarget != null)
        {
            if (CurrentCreepTarget == _creep)
            {
                thisDefenderIsEngagedAsMainTarget = false;
                thisDefenderIsEngagedAsNoneTarget = false;
                defenderAnimator.StopAttackAnimation();
                CurrentCreepTarget = null;
                LookForNewTarget(); //1. This one only runs if the defender just killed ITS target
            }
        }
    }
    private IEnumerator WaitForCreepCheck()
    {
        yield return new WaitForSeconds(0.1f * Time.deltaTime);
        Creep newPotentialTarget = CheckForUnengagedTargets(); //Are there any unengaged targets?
        if(newPotentialTarget != null)
        {
            DefenderEngageNewTargetAsMain(this, newPotentialTarget);
        }
        else if (newPotentialTarget == null)
        {
            DefenderEngageNewTargetAsNone(CheckForNewTargets()); //No? Check for unengaged ones
        }
    }
    public void SetDefenderIsEngagedAsMainTargetTrue(Creep creep)
    {
        if(creep != null)
        {
            CurrentCreepTarget = creep;
        }
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
