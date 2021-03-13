using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    scrCreepHealth defenderHealth;
    [Tooltip("Drag the body of the defender itself into this slot")]
    [SerializeField] private GameObject defenderBody;
    [SerializeField] private float respawnTimer = 2f;
    public bool IsEngagedWithCreep { get; private set; } //Used by movement script
    private List<Creep> _creepList;
    private Creep defenderCreepTarget;
    private scrCreepHealth targetHealth;
    public Vector3 currentCreepTargetPos { get; private set; }

    private void Awake()
    {
        _creepList = new List<Creep>();
        defenderHealth = GetComponent<scrCreepHealth>(); //Gets the instance
        IsEngagedWithCreep = false;
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
                if(defenderCreepTarget == theCreep)
                {
                    defenderCreepTarget = null; //Loose this reference if this creep just moved outside of range
                }
                _creepList.Remove(theCreep);
            }
        }
    }
    //Engages the target
    private void EngageTarget(Creep _target)
    {
        if(_target == null) //Makes the defender ignore a null reference
        {
            IsEngagedWithCreep = false; //This bool is used by the "scrDefenderMovement" script, to decide what to move towards
            return;
        }
        currentCreepTargetPos = _target.transform.position; //Used by "scrDefenderMovement" for moving towards the target
        _target.CreepIsTargetedByDefender(true); //Stops creep movement
        IsEngagedWithCreep = true;
        if (targetHealth.currentHealth <= 0) //Makes the defender ignore a dead creep
        {
            _target.CreepIsTargetedByDefender(false);
            IsEngagedWithCreep = false; //This bool is used by the "scrDefenderMovement" script, to decide what to move towards
            return;
        }
        //Move towards the target
    }
    //Manage the creep target
    private Creep SelectingTarget()
    {
        if(_creepList.Count <= 0)
        {
            defenderCreepTarget = null;
            return defenderCreepTarget; //Returns null, there are no creeps close
        }
        defenderCreepTarget = _creepList[0]; //Assign the first target as the default target
        float defenderCreepTargetDistanceTraveled = _creepList[0].DistanceTravelled;
        targetHealth = _creepList[0].GetComponent<scrCreepHealth>();
        if(targetHealth.currentHealth <= 0) //Checks that the current target has died
        {
            for (int i = 1; i < _creepList.Count; i++)
            {
                if (_creepList[i].DistanceTravelled > defenderCreepTargetDistanceTraveled)
                {
                    defenderCreepTarget = _creepList[i];
                    defenderCreepTargetDistanceTraveled = _creepList[i].DistanceTravelled;
                    targetHealth = _creepList[i].GetComponent<scrCreepHealth>();
                }
            }
            return defenderCreepTarget; //Returns the current living target
        }
        return defenderCreepTarget;

    }
    private void EnemyKilled(Creep _creep) //This is used to make sure the reference of the current creep is lost when it dies
    {
        if(defenderCreepTarget == _creep)
        {
            defenderCreepTarget = null;
        }
    }
    private void DefenderKilled(Defender _defender)
    {
        Debug.Log("I died"); 
        defenderBody.SetActive(false); //Set the gameobject to unactive
        RespawnDefender(respawnTimer);
        StartCoroutine(RespawnDefender(respawnTimer));
        //Reset the defender position.
        //Respawn after timer
    }
    private IEnumerator RespawnDefender(float _respawnTimer)
    {
        yield return new WaitForSeconds(_respawnTimer);
        defenderHealth.ResetHealth(); //Reset its health
        defenderBody.SetActive(true);
    }
    
    private void OnEnable()
    {
        scrCreepHealth.OnEnemyKilled += EnemyKilled;
        scrCreepHealth.OnDefenderKilled += DefenderKilled;
    }
    private void OnDisable()
    {
        scrCreepHealth.OnEnemyKilled -= EnemyKilled;
        scrCreepHealth.OnDefenderKilled -= DefenderKilled;
    }
}
