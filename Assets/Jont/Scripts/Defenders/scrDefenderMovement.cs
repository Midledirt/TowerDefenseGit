using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDefenderMovement : MonoBehaviour
{
    [Tooltip("Set how fast the defenders move")]
    [Range(1f, 5f)]
    [SerializeField] private float setDefenderMovementSpeed;
    private float defenderMovementSpeed;
    private Vector3 rallyPointPos;
    private DefenderEngagementHandler defender;
    private float engagementDistance = .8f;
    private scrDefenderAnimation defenderAnimator; //This is only how I do this right now. It is probably better to make a dedicated class for attacking
    public bool DefenderIsCurrentlyMovingTowardsNewPossition { get; set; }
    private bool defenderHasANewPotentialTarget;
    
    private float t;
    private Vector3 aPos;
    private Vector3 bPos;
    private Vector3 cPos;
    private Transform ABPos;
    private Transform BCPos;
    Vector3 randomlyGeneratedPossition;
    private void Awake()
    {
        defender = GetComponent<DefenderEngagementHandler>(); //Get the instance
        defenderAnimator = GetComponent<scrDefenderAnimation>(); //Get the instance;
        DefenderIsCurrentlyMovingTowardsNewPossition = true;
        defenderHasANewPotentialTarget = false;
        t = 0f;
        aPos = transform.position;
        randomlyGeneratedPossition = GetRandomPossition(); //Potential problem: I assume this possition is relative to the parent object. If not...
        ABPos = transform.Find("ABPos");
        BCPos = transform.Find("BCPos");
        defenderMovementSpeed = setDefenderMovementSpeed;
    }
    private Vector3 GetRandomPossition()
    {
        Vector3 _possition = new Vector3(0, 0, 0);
        for(int i = 0; i < 100; i++) //Randomizes the value, attempting to return a value smaller than -1 or greater than 1. It will run 100 attempts
        {
            _possition.x = Random.Range(-2f, 2f);
            if(_possition.x < -1f || _possition.x > 1f)
            {
                print("Good possition found for x");
                break;
            }
        }
        for (int i = 0; i < 100; i++) //Randomizes the value, attempting to return a value smaller than -1 or greater than 1. It will run 100 attempts
        {
            _possition.z = Random.Range(-2f, 2f);
            if (_possition.z < -1f || _possition.z > 1f)
            {
                print("Good possition found for z");
                break;
            }
        }
        return _possition;
    }
    private void Update()
    {
        if (defenderHasANewPotentialTarget && defender.defenderIsAlive) //Potential target set in the defender script
        {
            defenderAnimator.StopIdleAnimation();
            SetDefenderApproachTarget(defender.CurrentCreepTarget);
            if(defender.CurrentCreepTarget != null)
            {
                rotateTowardsTarget(defender.CurrentCreepTarget.transform.position);
            }
        }
        else if (defender.defenderIsAlive && defender.thisDefenderIsEngagedAsMainTarget == false && defender.thisDefenderIsEngagedAsNoneTarget == false)
        {
            defenderAnimator.StopAttackAnimation(); //Stop animation
            moveTowardsTarget(rallyPointPos);
            float _minDistanceToRallyPointPos = 0.05f;
            if ((transform.position - rallyPointPos).magnitude <= _minDistanceToRallyPointPos)
            {
                DefenderIsCurrentlyMovingTowardsNewPossition = false;
                defenderAnimator.PlayIdleAnimation();
            }
            rotateTowardsTarget(rallyPointPos);
        }
    }
    public void MakeDefenderMoveTowardsTarget() //Potential target set in the defender script
    {
        aPos = transform.position;
        GetRandomPossition();
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
            t = 0f;
            defender.defenderIsAlreadyMovingTowardsTarget = false;
            defenderHasANewPotentialTarget = false; //Prevents this code from looping
            defenderAnimator.PlayAttackAnimation(); //Attack
        }
    }
    public void moveTowardsTarget(Vector3 _currentTargetPos)//Movement function for defenders
    {
        if(defender.thisDefenderIsEngagedAsNoneTarget && !defender.thisDefenderIsEngagedAsMainTarget)
        {
            if (t < 1f)
            {
                t += (Time.deltaTime * defenderMovementSpeed) / 4;
            }

            bPos = ((aPos + randomlyGeneratedPossition) + _currentTargetPos) / 2;
            cPos = _currentTargetPos;

            ABPos.position = Vector3.Lerp(aPos, bPos, t);
            BCPos.position = Vector3.Lerp(bPos, cPos, t);

            transform.position = Vector3.Lerp(ABPos.position, BCPos.position, t);
        }

        transform.position = Vector3.MoveTowards(transform.position, _currentTargetPos, defenderMovementSpeed * Time.deltaTime);
    }
    private void rotateTowardsTarget(Vector3 _targetPossition)
    {
        transform.LookAt(_targetPossition);
    }
    public void getRallyPointPos(Vector3 rallyPointPossition) //Assigned from other script
    {
        DefenderIsCurrentlyMovingTowardsNewPossition = true;
        rallyPointPos = rallyPointPossition;
        defender.ResetDefenderIsEngagedAsMainOrNoneTarget(); //Make the defender break out of combat
        defenderHasANewPotentialTarget = false;
        defenderAnimator.StopIdleAnimation();
    }
}
