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
        if(defender.IsEngagedWithCreep == false)
        {
            defenderAnimator.StopAttackAnimation(); //Stop animation
            moveTowardsTarget(rallyPointPos);
            float _minDistanceToRallyPointPos = 0.05f;
            if((transform.position - rallyPointPos).magnitude <= _minDistanceToRallyPointPos)
            {
                DefenderIsCurrentlyMovingTowardsNewPossition = false;
            }
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
            else if((transform.position - defender.currentCreepTargetPos).magnitude <= engagementDistance && defender.DefenderCreepTarget != null) //Check if this defender is the first target
            {
                //I need to make a check for wether THIS defender is the creeps first target
                //Is the creep in fight?
                if (defender.DefenderCreepTarget.CreepGotItsFirstTarget == false)//No
                {
                    //ASSIGN THIS AS THE FIRST CREEP TARGET
                    defender.DefenderCreepTarget.CreepGotItsFirstTarget = true; //This is the first target - Engage.
                    defender.DefenderCreepTarget.AssignCreepsCurrentDefenderTarget(this.gameObject); //Assign as creepTarget
                    DefenderAlreadyHasATarget = true;
                    return;
                }
                else if(defender.DefenderCreepTarget.CreepGotItsFirstTarget == true) //Yes
                {
                    //Are there other targets in the tower engagement list?
                    if (defender.defenderTowerTargets.DefenderCreepList.Count > 0 && DefenderAlreadyHasATarget == false) //Yes
                    {
                        for(int i = 0; i < defender.defenderTowerTargets.DefenderCreepList.Count; i++)
                        {
                            //The problem might be that this list makes the defenders constantly go for the next target. Perhaps i can break out of it?
                            if(defender.defenderTowerTargets.DefenderCreepList[i].CreepGotItsFirstTarget == false)
                            {
                                //print("Defender selecting new target");
                                defender.UpdateCurrentDefenderTarget(defender.defenderTowerTargets.DefenderCreepList[i]);
                                defender.defenderTowerTargets.DefenderCreepList[i].CreepGotItsFirstTarget = true;
                                defender.defenderTowerTargets.DefenderCreepList[i].AssignCreepsCurrentDefenderTarget(this.gameObject);
                                DefenderAlreadyHasATarget = true;
                                break;
                            }
                            if(i == defender.defenderTowerTargets.DefenderCreepList.Count)
                            {
                                //print("Defender selecting last target in list");
                                defender.UpdateCurrentDefenderTarget(defender.defenderTowerTargets.DefenderCreepList[i]);
                                if(defender.defenderTowerTargets.DefenderCreepList[i].CreepGotItsFirstTarget == false) //Need 2 check this here because it might
                                    //not be true
                                {
                                    defender.defenderTowerTargets.DefenderCreepList[i].CreepGotItsFirstTarget = true;
                                }
                                defender.defenderTowerTargets.DefenderCreepList[i].AssignCreepsCurrentDefenderTarget(this.gameObject);
                                DefenderAlreadyHasATarget = true;
                                break;
                            }
                        }
                    }
                    else if (defender.defenderTowerTargets.DefenderCreepList.Count <= 0)//No
                    {
                        if(defender.defenderTowerTargets.DefenderCreepList[0] == null)
                        {
                            return;
                        }
                        print("There are no other targets to engage, engaging the first target in the list");
                        defender.DefenderCreepTarget.CreepGotItsFirstTarget = true; //This is the first target - Engage.
                        defender.DefenderCreepTarget.AssignCreepsCurrentDefenderTarget(this.gameObject); //Assign as creepTarget
                        DefenderAlreadyHasATarget = true;
                        return; //Engage.
                    }
                }

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
        DefenderIsCurrentlyMovingTowardsNewPossition = true;
        defender.SetIsEngagedWithCreepToFalse();
        DefenderAlreadyHasATarget = false;
        rallyPointPos = rallyPointPossition;
        //the creep needs to loose referense!
        if(defender.DefenderCreepTarget != null)
        {
            defender.DefenderCreepTarget.CreepEngagementHandler.SetEngagementToFalse();
        }
    }
}
