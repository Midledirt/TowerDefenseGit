using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTower : MonoBehaviour
{
    [SerializeField] private float attackRange = 3f;
    public scrUppgradeTurrets TowerUpgrade { get; set; } //This may be the wrong script! :O UPDATE: It works, so it must be right
    public Creep CurrentCreepTarget { get; set; }
    //public float AttackRange => attackRange; //This is used in the "scrTowerNode" script. It is used to make sprite that illustrates the
    //attack range scale with the actual attack range //IMPORTANT: DOES NOT WORK RIGHT NOW

    private bool _gameStarted;
    private List<Creep> _creeps;

    private void Start()
    {
        _gameStarted = true;
        _creeps = new List<Creep>(); //Initialize the list

        TowerUpgrade = GetComponent<scrUppgradeTurrets>();    
    }

    private void Update()
    {
        GetCurrentCreepTarget();
        RotateTowardsTarget();


        if (CurrentCreepTarget != null) //My own addition YES! With this, my game works as intended (so far) :)
        {
            if (CurrentCreepTarget._CreepHealth.currentHealth <= 0) 
            {
                _creeps.Remove(CurrentCreepTarget);
            }
        }
    }

    private void GetCurrentCreepTarget()
    {
        if (_creeps.Count <= 0)
        {
            CurrentCreepTarget = null;
            return; //No point in continuing with the logic of this function. Exit
        }
        //Old simple way
        CurrentCreepTarget = _creeps[0]; //IMPORTANT This is what makes us choose the first enemy in the list!
        //New better way, tho this may be a source of errors
        float currentCreepDistance = _creeps[0].DistanceTravelled;
        for (int i = 1; i < _creeps.Count; i ++) //Starting by one, makes it so that this list wont return any if we only have one target
        {
            if (_creeps[i].DistanceTravelled > currentCreepDistance) //Checks if any of the creeps have a larget currentCreepDistance
            {
                //Debug.Log("current creep target has been passed");
                CurrentCreepTarget = _creeps[i]; //Set it as new target
                currentCreepDistance = _creeps[i].DistanceTravelled; //Update the currentCreepDistance
            }

        }
    }

    private void RotateTowardsTarget()
    {
        if(CurrentCreepTarget == null)
        {
            return; // Stop the code
        }

        Vector3 targetPos = CurrentCreepTarget.transform.position - transform.position;
        targetPos.y = 0; //This is where the magic happens. It disables the y rotation 
        var rotation = Quaternion.LookRotation(targetPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creep")) //Filther for tags
        {
            Creep newCreep = other.GetComponent<Creep>(); //Get the new reference
            _creeps.Add(newCreep);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Creep"))
        {
            Creep creep = other.GetComponent<Creep>();
            if (_creeps.Contains(creep))//Checks if this SPESIFIC reference was already in the list (I think)
            {
                _creeps.Remove(creep); //Remove this creep from the list
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_gameStarted)
        {
            GetComponent<SphereCollider>().radius = attackRange * 2.5f; //I am not sure why the sphere collider radius is smaller that the wiresphere
            //Radius, but if i multiply it with 2.5f, its very close to the same size...
        }

        Gizmos.DrawWireSphere(transform.position, attackRange); //Visualize the attack range
    }
}