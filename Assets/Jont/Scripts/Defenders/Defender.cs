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
    private List<Creep> _creepList;
    public Creep DefenderCreepTarget { get; private set; }
    private scrCreepHealth targetHealth;
    public Vector3 currentCreepTargetPos { get; private set; }
    scrAnimationEventHandler animEventHandler;
    private void Awake()
    {
        _creepList = new List<Creep>();
        //defenderHealth = GetComponent<scrCreepHealth>(); //Gets the instance
        IsEngagedWithCreep = false;
        animEventHandler = GetComponentInChildren<scrAnimationEventHandler>(); //Get the instance
    }
    private void Update()
    {
        EngageTarget(SelectingTarget()); //Engage the selected target
    }
    //check for collisions with creep
    private void OnTriggerEnter(Collider other) //Adds the creep to a list
    {
        //print("Detected: " + other);
        if(other.CompareTag("Creep"))
        {
            Creep newCreep = other.GetComponent<Creep>();
            _creepList.Add(newCreep);
        }
    }
    private void OnTriggerExit(Collider other) //Removes the creep from the list
    {
        if(other.CompareTag("Creep"))
        {
            Creep theCreep = other.GetComponent<Creep>();
            if(_creepList.Contains(theCreep))
            {
                if(DefenderCreepTarget == theCreep)
                {
                    DefenderCreepTarget = null; //Loose this reference if this creep just moved outside of range
                }
                _creepList.Remove(theCreep);
            }
        }
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
        if (_creepList.Count <= 0)
        {
            DefenderCreepTarget = null;
            return DefenderCreepTarget; //Returns null, there are no creeps close
        }
        if (_creepList[0].GetComponent<scrCreepHealth>().CurrentHealth > 0f)
        {
            DefenderCreepTarget = _creepList[0]; //Assign the first target as the default target
        }
        float defenderCreepTargetDistanceTraveled = _creepList[0].DistanceTravelled;
        targetHealth = _creepList[0].GetComponent<scrCreepHealth>();
        if(targetHealth.CurrentHealth <= 0) //Checks that the current target has died
        {
            _creepList.Remove(_creepList[0]);
            for (int i = 0; i < _creepList.Count; i++)
            {
                if (_creepList[i].DistanceTravelled > defenderCreepTargetDistanceTraveled)
                {
                    DefenderCreepTarget = _creepList[i];
                    defenderCreepTargetDistanceTraveled = _creepList[i].DistanceTravelled;
                    targetHealth = _creepList[i].GetComponent<scrCreepHealth>();
                }
                else if(_creepList.Count < 0)
                {
                    return null;
                }
            }
            return DefenderCreepTarget; //Returns the current living target
        }
        return DefenderCreepTarget;

    }
    private void EnemyKilled(Creep _creep) //This is used to make sure the reference of the current creep is lost when it dies
    {
        if(_creepList.Contains(_creep)) //Make sure to remove it from the creep list, even if it is not the current target
        {
            //Debug.Log("I died, not the target");
            _creepList.Remove(_creep);
        }
        if(DefenderCreepTarget == _creep)
        {
            //Debug.Log("I died, current target");
            DefenderCreepTarget = null;
        }
    }
    private void DealDamageToEnemy(float _damage)
    {
        if(DefenderCreepTarget != null) //Check for null reference
        {
            DefenderCreepTarget.GetComponent<scrCreepHealth>().DealDamage(_damage); //Deal damage to the creep
        }
    }
    private void OnEnable()
    {
        animEventHandler.OnDealingDamage += DealDamageToEnemy;
        scrCreepHealth.OnEnemyKilled += EnemyKilled;
    }
    private void OnDisable()
    {
        animEventHandler.OnDealingDamage -= DealDamageToEnemy;
        scrCreepHealth.OnEnemyKilled -= EnemyKilled;
    }
}
