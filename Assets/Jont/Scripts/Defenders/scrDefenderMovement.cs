﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDefenderMovement : MonoBehaviour
{
    [Tooltip("Set how fast the defenders move")]
    [SerializeField] private float defenderMovementSpeed;
    private float defenderRotationSpeed = 10f; //How fast defenders rotate to face targets
    private Vector3 rallyPointPos;
    private Defender defender;
    private float engagementDistance = .8f;
    private scrDefenderAnimation defenderAnimator; //This is only how I do this right now. It is probably better to make a dedicated class for attacking
    private void Awake()
    {
        defender = GetComponent<Defender>(); //Get the instance
        defenderAnimator = GetComponent<scrDefenderAnimation>(); //Get the instance;
    }
    private void Update()
    {
        if(defender.IsEngagedWithCreep == false)
        {
            defenderAnimator.StopAttackAnimation(); //Stop animation
            moveTowardsTarget(rallyPointPos);
            if (transform.position != rallyPointPos)
            {
                rotateTowardsTarget(rallyPointPos);
            }
        }
        if(defender.IsEngagedWithCreep && defender.currentCreepTargetPos != null)
        {
            if((transform.position - defender.currentCreepTargetPos).magnitude > engagementDistance) //Move closer
            {
                moveTowardsTarget(defender.currentCreepTargetPos);
                rotateTowardsTarget(defender.currentCreepTargetPos);
            }
            //print("Close enough");
            defenderAnimator.PlayAttackAnimation();
            //Attack
            //Tell creep its under attack
        }

    }
    public void moveTowardsTarget(Vector3 _currentTargetPos)//Movement function for defenders
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTargetPos, defenderMovementSpeed * Time.deltaTime);
    }
    private void rotateTowardsTarget(Vector3 rallyPointPossition) //Simplify this code
    {
        Vector3 lookDirection = rallyPointPossition - transform.position;
        Quaternion direction = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, direction, defenderRotationSpeed * Time.deltaTime);
    }
    public void getRallyPointPos(Vector3 rallyPointPossition) //Assigned from other script
    {
        rallyPointPos = rallyPointPossition;
    }
}
