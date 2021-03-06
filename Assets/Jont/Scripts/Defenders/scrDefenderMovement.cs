using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDefenderMovement : MonoBehaviour
{
    [Tooltip("Set how fast the defenders move")]
    [SerializeField] private float defenderMovementSpeed;
    private float defenderRotationSpeed = 5f; //How fast defenders rotate to face targets
    private Vector3 rallyPointPos;


    private void Update()
    {
        moveTowardsTarget(rallyPointPos);
        rotateTowardsTarget(rallyPointPos);
    }
    private void moveTowardsTarget(Vector3 rallyPointPossition)//Movement function for defenders
    {
        transform.position = Vector3.MoveTowards(transform.position, rallyPointPossition, defenderMovementSpeed * Time.deltaTime);
    }
    private void rotateTowardsTarget(Vector3 rallyPointPossition)
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
