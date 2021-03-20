using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Defender : MonoBehaviour
{
    //scrCreepHealth defenderHealth;
    [Tooltip("Drag the body of the defender itself into this slot")]
    [SerializeField] private GameObject defenderBody;
    //[SerializeField] private float respawnTimer = 2f;
    public bool IsEngagedWithCreep { get; private set; } //Used by movement script
    public Creep DefenderCreepTarget { get; set; } //Can be set by defender movement!
    private scrCreepHealth targetHealth;
    public Vector3 currentCreepTargetPos { get; private set; }
    private scrAnimationEventHandler animEventHandler;
    public scrDefenderTowerTargets defenderTowerTargets;
    private scrDefenderMovement defenderMovement;
    private void Awake()
    {
        defenderMovement = GetComponent<scrDefenderMovement>();
        IsEngagedWithCreep = false;
        animEventHandler = GetComponentInChildren<scrAnimationEventHandler>(); //Get the instance
    }
    private void Update()
    {
        //EngageTarget(SelectingTarget()); //Engage the selected target
        //I think the issue is that this method is always making the defender move towards target 0. There must be a way to remedy this
    }
    public void SetNewTarget()
    {
        EngageTarget(SelectingTarget());
    }
    public void SetIsEngagedWithCreepToFalse()
    {
        IsEngagedWithCreep = false;
    }

    //Engages the target
    private void EngageTarget(Creep _target)
    {
        if(_target == null) //Makes the defender ignore a null
        {
            IsEngagedWithCreep = false; //This bool is used by the "scrDefenderMovement" script, to decide what to move towards
            return;
        }
        if(_target._CreepHealth.CurrentHealth <= 0f) //If its not null, but it has 0 health...
        {
            IsEngagedWithCreep = false; //This bool is used by the "scrDefenderMovement" script, to decide what to move towards
            return;
        }
        currentCreepTargetPos = _target.transform.position; //Used by "scrDefenderMovement" for moving towards the target
        scrCreepEngagementHandler _targetEngagementHandler = _target.GetComponent<scrCreepEngagementHandler>();
        //THE FOLLOWING 3 FUNCTIONS FEEL KINDA REDUNDANT, PERHAPS THERE IS A WAY TO SIMPLIFY THIS INTO ONE FUNCTION(OR ACTION) THAT SENDS SEVERAL REFERENCES?
        _targetEngagementHandler.ToggleEngagement(_target); //Used for animation
        _target.CreepIsTargetedByDefender(true); //Stops creep movement
        _target.CreepIsInCombatWithTarget(this.gameObject); //Sends a reference of this gameobject to a list used to toggle creep attack
        //_target.CreepIsInCombatWithTarget(this.gameObject); //Makes the target respond and fight back against "this"
        IsEngagedWithCreep = true;
        if (targetHealth.CurrentHealth <= 0) //Makes the defender ignore a dead creep
        {
            _target.CreepIsTargetedByDefender(false);
            _targetEngagementHandler.ToggleEngagementEnd(_target);
            IsEngagedWithCreep = false; //This bool is used by the "scrDefenderMovement" script, to decide what to move towards
            return;
        }
    }
    //Manage the creep target
    private Creep SelectingTarget()
    {
        if (defenderTowerTargets.DefenderCreepList.Count <= 0)
        {
            DefenderCreepTarget = null;
            return DefenderCreepTarget; //Returns null, there are no creeps close
        }
        if (defenderTowerTargets.DefenderCreepList[0].GetComponent<scrCreepHealth>().CurrentHealth > 0f)
        {
            DefenderCreepTarget = defenderTowerTargets.DefenderCreepList[0]; //Assign the first target as the default target
        }
        float defenderCreepTargetDistanceTraveled = defenderTowerTargets.DefenderCreepList[0].DistanceTravelled;
        targetHealth = defenderTowerTargets.DefenderCreepList[0].GetComponent<scrCreepHealth>();
        if(targetHealth.CurrentHealth <= 0) //Checks that the current target has died
        {
            defenderTowerTargets.DefenderCreepList.Remove(defenderTowerTargets.DefenderCreepList[0]);
            for (int i = 0; i < defenderTowerTargets.DefenderCreepList.Count; i++)
            {
                if (defenderTowerTargets.DefenderCreepList[i].DistanceTravelled > defenderCreepTargetDistanceTraveled)
                {
                    DefenderCreepTarget = defenderTowerTargets.DefenderCreepList[i];
                    defenderCreepTargetDistanceTraveled = defenderTowerTargets.DefenderCreepList[i].DistanceTravelled;
                    targetHealth = defenderTowerTargets.DefenderCreepList[i].GetComponent<scrCreepHealth>();
                }
                else if(defenderTowerTargets.DefenderCreepList.Count < 0)
                {
                    return null;
                }
            }
            return DefenderCreepTarget; //Returns the current living target
        }
        return DefenderCreepTarget;
    }
    public void UpdateCurrentDefenderTarget(Creep _newTarget)
    {
        DefenderCreepTarget = _newTarget;
        //currentCreepTargetPos = _newTarget.transform.position; //Do not think this is necessary here
        EngageTarget(_newTarget);
    }
    private void EnemyKilled(Creep _creep) //This is used to make sure the reference of the current creep is lost when it dies
    {
        if(defenderTowerTargets.DefenderCreepList.Contains(_creep)) //Make sure to remove it from the creep list, even if it is not the current target
        {
            //Debug.Log("I died, not the target");
            defenderTowerTargets.DefenderCreepList.Remove(_creep);
        }
        if(DefenderCreepTarget == _creep)
        {
            //Debug.Log("I died, current target");
            DefenderCreepTarget = null;
            defenderMovement.DefenderAlreadyHasATarget = false;
            IsEngagedWithCreep = false;
            SetNewTarget(); //Search for a new target emediately
        }
    }
    public scrDefenderTowerTargets AssignDefenderTowerTargets(scrDefenderTowerTargets _defenderTowerTargets)
    {
        //print("Local defender targets assigned to defender");
        return defenderTowerTargets = _defenderTowerTargets; //Gets the reference
    }
    private void DealDamageToEnemy(float _damage)
    {
        if(DefenderCreepTarget != null) //Check for null reference
        {
            DefenderCreepTarget.GetComponent<scrCreepHealth>().DealDamage(_damage); //Deal damage to the creep
        }
    }
    private void RemoveDefenderTarget(Creep _defenderTarget)
    {
        if (DefenderCreepTarget == _defenderTarget)
        {
            DefenderCreepTarget = null; //Loose this reference if this creep just moved outside of range
        }
    }
    private void OnEnable()
    {
        scrDefenderTowerTargets.LooseDefenderTarget += RemoveDefenderTarget;
        animEventHandler.OnDealingDamage += DealDamageToEnemy;
        scrCreepHealth.OnEnemyKilled += EnemyKilled;
    }
    private void OnDisable()
    {
        scrDefenderTowerTargets.LooseDefenderTarget -= RemoveDefenderTarget;
        animEventHandler.OnDealingDamage -= DealDamageToEnemy;
        scrCreepHealth.OnEnemyKilled -= EnemyKilled;
    }
}
