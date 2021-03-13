﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation; //Needed!


public class Creep : MonoBehaviour
{
    private CreepStatsSO stats;
    //This is EXTREMELY interesting. It is covered at Part 14, around 3.50 timestamp. 
    //REQUIRES "System"
    public static Action<Creep> OnEndReaced;

    //From "PathFollower"
    //float distanceTravelled;
    public float DistanceTravelled { get; set; }
    public PathCreator myPath; 
    public EndOfPathInstruction endOfPathInstruction; //This one needs to be assigned
    public float MovementSpeed { get; set; } //For modifying the property movementspeed in other scripts
    public scrCreepHealth _CreepHealth { get; private set; }
    [HideInInspector] public bool hasBeenSpawned; //Used to prevent this instance from being respawned by the spawner

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
        _CreepHealth = GetComponent<scrCreepHealth>();

        MovementSpeed = stats.movementSpeed; //For modifying the property movementspeed in other scripts
        //creepPossition = transform.position; //Stores the position of the transform
    }
    public PathCreator SetPath(PathCreator path)
    {
        //Debug.Log("I am assigned a path");
        return myPath = path.GetComponent<PathCreator>();
    }

    private void Update()
    {
        if (myPath != null)
        {
            DistanceTravelled += MovementSpeed * Time.deltaTime;
            transform.position = myPath.path.GetPointAtDistance(DistanceTravelled, endOfPathInstruction);
            transform.rotation = myPath.path.GetRotationAtDistance(DistanceTravelled, endOfPathInstruction);
            
            if (DistanceTravelled >= myPath.path.length)
            {
                //Debug.Log("Reached the end"); CONFIRMED THAT THIS TURNS TRUE WHEN THE OBJECT REACHES THE END
                EndPointReached();
            }
        }
    }
    public void CreepIsTargetedByDefender(bool isTargeted)
    {
        if (isTargeted)
        {
            StopMovement();
        }
        else
            ResumeMovement();

        //Play Idle animation from animator (IdleNotImplementedYet)
    }
    void OnPathChanged()
    {
        DistanceTravelled = myPath.path.GetClosestDistanceAlongPath(transform.position);
    }
    public void StopMovement()
    {
        //print("Movement is stopped");
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
