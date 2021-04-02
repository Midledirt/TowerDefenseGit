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
        else if (defender.defenderIsAlive && defender.thisDefenderIsEngagedAsMainTarget == false && defender.thisDefenderIsEngagedAsNoneTarget == false)
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
        if(_creep == null)
        {
            defenderHasANewPotentialTarget = false;
            defender.defenderIsAlreadyMovingTowardsTarget = false;
            return;
        }
        if(_creep._CreepHealth.CurrentHealth <= 0f)
        {
            defenderHasANewPotentialTarget = false;
            defender.defenderIsAlreadyMovingTowardsTarget = false;
            //print("Target died before I got to it...");
            defender.LookForNewTarget();
            return;
        }
        if((transform.position - _creep.transform.position).magnitude > engagementDistance)
        {
            moveTowardsTarget(_creep.transform.position);
            //Rotate towards target
        }
        else if((transform.position - _creep.transform.position).magnitude <= engagementDistance)
        {
            defender.defenderIsAlreadyMovingTowardsTarget = false;

            scrCreepEngagementHandler potentialTargetEngagementHandler = _creep.GetComponent<scrCreepEngagementHandler>();
            if (potentialTargetEngagementHandler.CurrentTarget == defender) //Check if we are the current target //WORKS
            {
                defenderHasANewPotentialTarget = false; //Prevents this code from looping
                defender.SetDefenderIsEngagedAsMainTargetTrue(); //What keeps the defender from searching from approaching ever new targets
                potentialTargetEngagementHandler.SetThisCreepIsEngaged();
                defenderAnimator.PlayAttackAnimation(); //Attack
            }
            else if(defender.thisDefenderIsEngagedAsNoneTarget)
            {
                defenderHasANewPotentialTarget = false; //Prevents this code from looping
                defenderAnimator.PlayAttackAnimation(); //Attack
            }
            else
            {
                defenderHasANewPotentialTarget = false; //Prevents this code from looping
                defender.ChceckForOtherTargets(_creep);
            }
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
        defender.ResetDefenderIsEngagedAsMainOrNoneTarget(); //Make the defender break out of combat
        defenderHasANewPotentialTarget = false;
    }
}
