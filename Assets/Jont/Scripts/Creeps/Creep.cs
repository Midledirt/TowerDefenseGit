using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation; //Needed!
using Random = UnityEngine.Random;


public class Creep : MonoBehaviour
{
    private CreepStatsSO stats;
    //This is EXTREMELY interesting. It is covered at Part 14, around 3.50 timestamp. 
    //REQUIRES "System"
    public static Action<Creep> OnEndReaced;
    public float DistanceTravelled { get; set; }
    public PathCreator myPath; 
    public EndOfPathInstruction endOfPathInstruction; //This one needs to be assigned
    public float MovementSpeed { get; set; } //For modifying the property movementspeed in other scripts
    public scrUnitHealth _CreepHealth { get; private set; }
    [HideInInspector] public bool hasBeenSpawned; //Used to prevent this instance from being respawned by the spawner

    [Header("Randomize creep possitions")]
    [Tooltip("Larget numbers increases how far a creep MIGHT deviate from the original path possition")]
    [Range(0f, 1f)]
    [SerializeField] private float creepPossitionRandomizer;
    private Vector3 randomizedPossition;

    private void Awake()
    {
        stats = GetComponent<scrCreepTypeDefiner>().creepType;
        hasBeenSpawned = false;
    }
    private void Start()
    {
        if (myPath != null)
        {
            transform.position = myPath.path.GetPoint(0); //Spawns the creep at the first point
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            myPath.pathUpdated += OnPathChanged;
        }

        //currentWaypointIndex = 0;
        _CreepHealth = GetComponent<scrUnitHealth>();

        MovementSpeed = stats.movementSpeed; //For modifying the property movementspeed in other scripts
        //creepPossition = transform.position; //Stores the position of the transform
        randomizedPossition = RandomizeCreepPossition(); //Randomizes the possition for each creep on start
    }
    private void Update()
    {
        if (myPath != null)
        {
            DistanceTravelled += MovementSpeed * Time.deltaTime;
            transform.position = (myPath.path.GetPointAtDistance(DistanceTravelled, endOfPathInstruction) + randomizedPossition);
            transform.rotation = myPath.path.GetRotationAtDistance(DistanceTravelled, endOfPathInstruction);

            if (DistanceTravelled >= myPath.path.length)
            {
                //Debug.Log("Reached the end"); CONFIRMED THAT THIS TURNS TRUE WHEN THE OBJECT REACHES THE END
                EndPointReached();
            }
        }
    }
    private Vector3 RandomizeCreepPossition()
    {
        Vector3 _randomizedPossition = Vector3.zero;
        _randomizedPossition.x += Random.Range(-creepPossitionRandomizer, creepPossitionRandomizer);
        _randomizedPossition.z += Random.Range(-creepPossitionRandomizer, creepPossitionRandomizer);
        return _randomizedPossition;
    }
    public PathCreator SetPath(PathCreator path)
    {
        //Debug.Log("I am assigned a path");
        return myPath = path.GetComponent<PathCreator>();
    }
    void OnPathChanged()
    {
        DistanceTravelled = myPath.path.GetClosestDistanceAlongPath(transform.position);
    }
    public void StopMovement()
    {
        MovementSpeed = 0f;
    }
    public void ResumeMovement()
    {
        MovementSpeed = stats.movementSpeed;
    }
    private void EndPointReached()
    {
        //This is where we fire the event (see the comment at the beginning of this script)
        if (OnEndReaced != null)
        {
            OnEndReaced.Invoke(this);
        }
        //This following code will do the same thing easier, but the code might be confusing, so I keep the old method active
        //OnEndReaced?.Invoke();
        transform.position = myPath.path.GetPoint(0); //Resets the creep possition
        DistanceTravelled = 0f; //Reset the travel distance variable, also necessary so that they don`t "teleport" back into the goal
        _CreepHealth.ResetHealth(); // Resets the amount of health the creep has as it reaches its end position
        ObjectPooler.SetObjectToInactive(gameObject); //Only sets the gameobject to "not active".
    }
    public void ReturnPosition(Creep creep) //Is fired from... Check the reference above this method! :) (from the scrCreepAnimations)
    {
        creep.DistanceTravelled = 0f; //Reset the travel distance variable, also necessary so that they don`t "teleport" back into the goal
    }
}
