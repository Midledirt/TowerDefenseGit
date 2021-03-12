using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    scrCreepHealth defenderHealth;
    [Tooltip("Drag the body of the defender itself into this slot")]
    [SerializeField] private GameObject defenderBody;
    [SerializeField] private float respawnTimer = 2f;
    private bool isEngagedWithCreep;
    private List<Creep> _creepList;
    private Creep defenderCreepTarget;

    private void Awake()
    {
        _creepList = new List<Creep>();
        defenderHealth = GetComponent<scrCreepHealth>(); //Gets the instance
        isEngagedWithCreep = false;
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
                _creepList.Remove(theCreep);
            }
        }
    }
    //Engages the target
    private void EngageTarget(Creep _target)
    {
        if(_target == null)
        {
            return;
        }
        _target.StopMovement(); //Halts the target
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
        for(int i = 1; i < _creepList.Count; i++)
        {
            if(_creepList[i].DistanceTravelled > defenderCreepTargetDistanceTraveled)//YOU NEED TO ADD A CHECK THAT MAKES SURE THE CURRENT TARGET IS DEAD FIRST
            {
                defenderCreepTarget = _creepList[i];
                defenderCreepTargetDistanceTraveled = _creepList[i].DistanceTravelled;
            }
        }
        return defenderCreepTarget; //Returns the current living target
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
        scrCreepHealth.OnDefenderKilled += DefenderKilled;
    }
    private void OnDisable()
    {
        scrCreepHealth.OnDefenderKilled -= DefenderKilled;
    }
}
