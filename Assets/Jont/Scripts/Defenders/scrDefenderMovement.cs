using System.Collections;
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
    public bool DefenderAlreadyHasATarget { get; set; } //This is set to false on the defender dies event, as well as by the creeps when they die
    public bool DefenderIsCurrentlyMovingTowardsNewPossition { get; set; }
    private void Awake()
    {
        defender = GetComponent<Defender>(); //Get the instance
        defenderAnimator = GetComponent<scrDefenderAnimation>(); //Get the instance;
        DefenderAlreadyHasATarget = false;
        DefenderIsCurrentlyMovingTowardsNewPossition = true;
    }
    private void Update()
    {
        defenderAnimator.StopAttackAnimation(); //Stop animation
        moveTowardsTarget(rallyPointPos);
        float _minDistanceToRallyPointPos = 0.05f;
        if ((transform.position - rallyPointPos).magnitude <= _minDistanceToRallyPointPos)
        {
            DefenderIsCurrentlyMovingTowardsNewPossition = false;
        }
        if (transform.position != rallyPointPos)
        {
            rotateTowardsTarget(rallyPointPos);
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
        DefenderIsCurrentlyMovingTowardsNewPossition = true;
        DefenderAlreadyHasATarget = false;
        rallyPointPos = rallyPointPossition;
    }
}
