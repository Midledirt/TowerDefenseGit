using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTower : MonoBehaviour
{
    [SerializeField] private float attackRange = 3f;

    public Creep CurrentCreepTarget { get; set; }

    private bool _gameStarted;
    private List<Creep> _creeps;

    private void Start()
    {
        _gameStarted = true;
        _creeps = new List<Creep>(); //Initialize the list
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
        CurrentCreepTarget = _creeps[0]; //IMPORTANT This is what makes us choose the first enemy in the list!
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
        //float angle = Vector3.SignedAngle(transform.forward, targetPos, transform.up); //Tutorial code, have issues with it

        //transform.Rotate(0f, angle, 0f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creep")) //Filther for tags
        {
            Creep newCreep = other.GetComponent<Creep>(); //Get the new reference
            _creeps.Add(newCreep);
        }
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Creep"))
        {
            Creep monitoredCreep = other.GetComponent<Creep>();
            if (monitoredCreep == null) //Check if the creep has dissapeared
            {
                LooseCurrentCreepTarget(monitoredCreep.gameObject); //Remove this from the list
            }
        }
    }
    */
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