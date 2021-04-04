using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the logic necesarry to find out if this creep is engaged or not
/// </summary>
public class scrCreepEngagementHandler : MonoBehaviour
{
    scrAnimationEventHandler creepAnimationEventHandler;
    private Creep thisCreep;
    public List<DefenderEngagementHandler> currentDefenderTargetsForThisCreep { get; private set; }
    public bool ThisCreepIsEngaged { get; private set; }
    public DefenderEngagementHandler CurrentTarget { get; private set; }
    //I MUST set a specific reference to whatever defender this creep IS IN combat with. So that this creep can return to walking if that defender dies or is
    //moved

    private void Awake()
    {
        thisCreep = GetComponent<Creep>(); //Get the creep instance on this game object
        creepAnimationEventHandler = GetComponentInChildren<scrAnimationEventHandler>(); //Gets the reference
    }
    private void Start()
    {
        currentDefenderTargetsForThisCreep = new List<DefenderEngagementHandler>();
        ThisCreepIsEngaged = false;
        CurrentTarget = null; //starts with no target
    }
    private void Update()
    {
        CreepEngageTarget();
    }
    private void RotateTowardsTarget(Vector3 _target)
    {
        if(CurrentTarget != null)
        {
            transform.LookAt(_target);
        }
    }
    public void SetThisCreepIsEngaged()
    {
        if(ThisCreepIsEngaged == false)
        {
            //print("This creep is engaged");
            ThisCreepIsEngaged = true;
        }
    }
    public void AddDefenderToCreepTargetsList(DefenderEngagementHandler _newDefenderTarget)
    {
        if(_newDefenderTarget != null)
        {
            currentDefenderTargetsForThisCreep.Add(_newDefenderTarget); //Adds the new defender to local list
        }
    }
    private void CreepEngageTarget()
    {
        if (currentDefenderTargetsForThisCreep.Count <= 0) //Return if there are no current targets
        {
            ThisCreepIsEngaged = false;
            CurrentTarget = null;
            ReturnToPath();
            return;
        }
        if(currentDefenderTargetsForThisCreep.Count > 0)
        {
            thisCreep.StopMovement();
            if (CurrentTarget != null)
            {
                RotateTowardsTarget(CurrentTarget.transform.position);
            }
            CheckForDefenderDeath(); //4.Handle target death.
        }
    }
    private void DamageDefender(float _damage)
    {
        if(CurrentTarget != null && CurrentTarget.defenderIsAlive)
        {
            CurrentTarget.DefenderHealth.DealDamage(_damage); //Damage the defender
        }
    }
    private void CheckForDefenderDeath()
    {
        if(currentDefenderTargetsForThisCreep.Count <= 0)
        {
            currentDefenderTargetsForThisCreep.Clear();
            CurrentTarget = null;
            return;
        }
        if(currentDefenderTargetsForThisCreep[0].defenderIsAlive || currentDefenderTargetsForThisCreep[0] != null)
        {
            CurrentTarget = currentDefenderTargetsForThisCreep[0];
        }
        if (!CurrentTarget.defenderIsAlive) //Find new targets when current target dies
        {
            print("DefenderEngagementHandler died, removing defender");
            CurrentTarget.DefenderRemovedByCreep(CurrentTarget);
            currentDefenderTargetsForThisCreep.Remove(CurrentTarget); //Remove it from the list
            if(currentDefenderTargetsForThisCreep.Count <= 0)
            {
                print("No more defenders, returning to path.");
                CurrentTarget = null; //Set this to the next potential defender instead
                currentDefenderTargetsForThisCreep.Clear(); //Just to be sure
                ReturnToPath();
            }
            else //Get the next defender //
            {
                print("More defenders in list...");
                for(int i = 0; i < currentDefenderTargetsForThisCreep.Count; i++)
                {
                    if(currentDefenderTargetsForThisCreep[i].defenderIsAlive && !currentDefenderTargetsForThisCreep[i].thisDefenderIsEngagedAsMainTarget 
                        && !currentDefenderTargetsForThisCreep[i].defenderIsAlreadyMovingTowardsTarget)
                    {
                        print("Assigning new target");
                        if(CurrentTarget != null)
                        {
                            if(CurrentTarget.DefenderTowerTargetsReference.DefenderCreepList.Contains(thisCreep))
                            {
                                CurrentTarget = currentDefenderTargetsForThisCreep[i];
                                CurrentTarget.SetDefenderIsEngagedAsMainTargetTrue(thisCreep);
                            }
                        }
                    }
                    else if(i >= currentDefenderTargetsForThisCreep.Count)
                    {
                        print("...Actually, no more viable defenders after all");
                        currentDefenderTargetsForThisCreep.Clear();
                        CurrentTarget = null;
                    }
                }
            }
        }
    }
    private void ReturnToPath()
    {
        if(CurrentTarget != null || currentDefenderTargetsForThisCreep.Count > 0) //Checks that we have no targets.
        {
            return;
        }
        thisCreep.ResumeMovement();
    }
    public void RemoveDefenderFromList(DefenderEngagementHandler _defender) //Removes the defender from target list if it is in the target list
    {
        if(currentDefenderTargetsForThisCreep.Contains(_defender))
        {
            currentDefenderTargetsForThisCreep.Remove(_defender);
        }
    }
    private void DefenderRallyPointMoved(Creep _creep, List<DefenderEngagementHandler> _defenders) //Remove defenders from list
    {
        if(thisCreep == _creep) //If we are affected
        {
            //print("Rally point moved"); //works
            foreach(DefenderEngagementHandler defender in _defenders)
            {
                if(currentDefenderTargetsForThisCreep.Contains(defender))
                {
                    currentDefenderTargetsForThisCreep.Remove(defender); //This works!
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
        creepAnimationEventHandler.OnDealingDamage += DamageDefender;
    }
    private void OnDisable()
    {
        scrDefenderTowerTargets.LooseDefenderTarget -= DefenderRallyPointMoved;
        creepAnimationEventHandler.OnDealingDamage -= DamageDefender;
    }
}
