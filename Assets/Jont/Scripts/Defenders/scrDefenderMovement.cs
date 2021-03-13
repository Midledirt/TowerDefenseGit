using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDefenderMovement : MonoBehaviour
{
    [Tooltip("Set how fast the defenders move")]
    [SerializeField] private float defenderMovementSpeed;
    private float defenderRotationSpeed = 5f; //How fast defenders rotate to face targets
    private Vector3 rallyPointPos;
    private Defender defender;
    private void Awake()
    {
        defender = GetComponent<Defender>(); //Get the instance
    }
    private void Update()
    {
        if(defender.IsEngagedWithCreep == false)
        {
            moveTowardsTarget(rallyPointPos);
            if (transform.position != rallyPointPos)
            {
                rotateTowardsTarget(rallyPointPos);
            }
        }
        if(defender.IsEngagedWithCreep && defender.currentCreepTargetPos != null)
        {
            moveTowardsTarget(defender.currentCreepTargetPos);
            rotateTowardsTarget(defender.currentCreepTargetPos);
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
