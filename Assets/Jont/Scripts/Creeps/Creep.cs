using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation; //Needed!


public class Creep : MonoBehaviour
{
    //This is EXTREMELY interesting. It is covered at Part 14, around 3.50 timestamp. 
    //REQUIRES "System"
    public static Action<Creep> OnEndReaced;

    //From "PathFollower"
    float distanceTravelled;
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction; //This one needs to be assigned


    [SerializeField] private float movementSpeed = 3f;

    public float MovementSpeed { get; set; } //For modifying the property movementspeed in other scripts

    public scrCreepHealth _CreepHealth { get; set; }

    private scrCreepHealth CreepHealth;

    private void Start()
    {
        pathCreator = FindObjectOfType<PathCreator>(); //This may work for now, but there might be issues when I make more paths in the same level...
        //The reason why is that THIS script in placed on a PREFAB (does not exist in the scene before start). As such, it cannot refere to scene objects
        //like the path, by simply dragging something from the inspector

        if (pathCreator != null)
        {
            transform.position = pathCreator.path.GetPoint(0); //Spawns the creep at the first point
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }

        //currentWaypointIndex = 0;
        _CreepHealth = GetComponent<scrCreepHealth>();

        MovementSpeed = movementSpeed; //For modifying the property movementspeed in other scripts

        CreepHealth = GetComponent<scrCreepHealth>();
        //creepPossition = transform.position; //Stores the position of the transform
    }

    private void Update()
    {
        if (pathCreator != null)
        {
            distanceTravelled += movementSpeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }

        if (distanceTravelled >= pathCreator.path.length)
        {
            //Debug.Log("Reached the end"); CONFIRMED THAT THIS TURNS TRUE WHEN THE OBJECT REACHES THE END
            EndPointReached();
        }
    }

    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    public void StopMovement()
    {
        MovementSpeed = 0f;
    }

    public void ResumeMovement()
    {
        MovementSpeed = movementSpeed;
    }

    private void EndPointReached()
    {
        //Spawner bug: 
        //The reason why my waves are a little weird, is that the "OnEndReached" SHOULD only be called once per creep. However, it is called
        //multiple times, my guess is that it happens because it is called in the update frame, and happens (potentially) several times before
        //the "creep" is disabled 

        //This is where we fire the event (see the comment at the beginning of this script)
        if (OnEndReaced != null)
        {
            OnEndReaced.Invoke(this);
        }
        //This following code will do the same thing easier, but the code might be confusing, so I keep the old method active
        //OnEndReaced?.Invoke();

        CreepHealth.ResetHealth(); // Resets the amount of health the creep has as it reaches its end position
        ObjectPooler.ReturnToPool(gameObject);
    }
}
