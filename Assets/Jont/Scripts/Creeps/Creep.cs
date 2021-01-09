using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep : MonoBehaviour
{
    //This is EXTREMELY interesting. It is covered at Part 14, around 3.50 timestamp. 
    //REQUIRES "System"
    public static Action<Creep> OnEndReaced;
    private Vector3 creepPossition;
    private Vector3 direction;

    [SerializeField] private float movementSpeed = 3f;

    public float MovementSpeed { get; set; } //For modifying the property movementspeed in other scripts

    public scrWaypoint myWaypoint { get; set; }

    public scrCreepHealth _CreepHealth { get; set; }

    public Vector3 currentPointPossition => myWaypoint.GetWaypointPosition(currentWaypointIndex);

    private int currentWaypointIndex;
    private scrCreepHealth CreepHealth;

    private void Start()
    {
        currentWaypointIndex = 0;
        _CreepHealth = GetComponent<scrCreepHealth>();

        MovementSpeed = movementSpeed; //For modifying the property movementspeed in other scripts

        CreepHealth = GetComponent<scrCreepHealth>();
        creepPossition = transform.position; //Stores the position of the transform
    }

    private void Update()
    {
        Move();
        Rotate();
        if (CurrentPointPositionReached())
        {
            //Increment the waypoint index
            UpdateCurrentPointIndex();
        }
    }

    public void StopMovement()
    {
        MovementSpeed = 0f;
    }

    public void ResumeMovement()
    {
        MovementSpeed = movementSpeed;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentPointPossition, MovementSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        direction = currentPointPossition - creepPossition; //Get the direction we are facing
        transform.rotation = Quaternion.LookRotation(direction); //apply it to the "lookrotation"
        //transform.Rotate(direction); 
    }

    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - currentPointPossition).magnitude;
        //Check if we have moved to the next points possition
        if (distanceToNextPointPosition < 0.1f)
        {
            creepPossition = transform.position; //Update the possition of the creep
            return true;
        }

        return false;
    }

    //Make sure to only update the waypoint index for as long as there are more waypoints in the list
    private void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = myWaypoint.Points.Length - 1;
        if (currentWaypointIndex < lastWaypointIndex)
        {
            currentWaypointIndex++;
        }
        else // if we have reached the final waypoint
        {
            EndPointReached();
        }
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

        CreepHealth.ResetHealth(); // Resets the amount of health the creep has as it reaches its end position
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void ResetCreep()
    {
        currentWaypointIndex = 0; //Sets the waypoint back to 0, so this enemy won`t go straight for the end checkpoint when it is
        //Respawned
    }
}
