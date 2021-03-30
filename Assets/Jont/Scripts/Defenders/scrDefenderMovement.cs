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
    public bool DefenderIsCurrentlyMovingTowardsNewPossition { get; set; }
    private bool defenderHasANewPotentialTarget;
    private void Awake()
    {
        defender = GetComponent<Defender>(); //Get the instance
        defenderAnimator = GetComponent<scrDefenderAnimation>(); //Get the instance;
        DefenderIsCurrentlyMovingTowardsNewPossition = true;
        defenderHasANewPotentialTarget = false;
    }
    private void Update()
    {
        if (defenderHasANewPotentialTarget && defender.defenderIsAlive) //Potential target set in the defender script
        {
            SetDefenderApproachTarget(defender.CurrentCreepTarget);
        }
        else if (defender.defenderIsAlive && defender.thisDefenderIsEngagedAsMainTarget == false)
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
    }
    public void MakeDefenderMoveTowardsTarget() //Potential target set in the defender script
    {
        defenderHasANewPotentialTarget = true;
    }
    public void SetDefenderApproachTarget(Creep _creep)
    {
        if((transform.position - _creep.transform.position).magnitude > engagementDistance)
        {
            moveTowardsTarget(_creep.transform.position);
            //Rotate towards target
        }
        else if((transform.position - _creep.transform.position).magnitude <= engagementDistance)
        {
            defenderHasANewPotentialTarget = false;
            scrCreepEngagementHandler potentialTargetEngagementHandler = _creep.GetComponent<scrCreepEngagementHandler>();
            if (potentialTargetEngagementHandler.CurrentTarget == null)
            {
                defender.SetDefenderIsEngagedAsMainTargetTrue(); //What keeps the defender from searching from approaching ever new targets
                potentialTargetEngagementHandler.SetThisCreepIsEngaged();
            }
            //else if() Make it possible to attack, even if we are not the main target (in case there are no other targets)
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
        rallyPointPos = rallyPointPossition;
        defender.ResetDefenderIsEngagedAsMainTarget(); //Make the defender break out of combat
        defenderHasANewPotentialTarget = false;
    }
}
