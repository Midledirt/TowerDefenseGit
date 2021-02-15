using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the movement of the mage projectile! Since other projectiles derrives from this, ive decided to call it "scrProjecties".
/// </summary>
public class scrProjecties : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Assign the projectile type with the stats you want for this prefab. This must be done for BOTH tower and projectile prefab!")]
    public TowerProjectileTypeSO stats;     //Inherit stats from SO
    [Tooltip("How fast this projectile moves")]
    private float movementSpeed;
    [Tooltip("Defines how close to the target the projectiles needs to be in order to hit it. The larger this radius is, the easier and ealier the collision")]
    [Range(0.1f, 1f)]
    private float MinDistanceToDealDamage;
    public bool homingProjectile = false;

    public scrTowerProjectileLoader TurretOwner { get; set; } //Define a "Owner". `This is weird, covered in episode 26, around 3.30
    public float Damage { get; set; } //This damage is assigned from the tower via the scrTowerProjectileLoader Script

    private Creep _creepTarget;
    private Vector3 targetPos;
    [HideInInspector] public bool projectileIsFired; //Used to dissable "homing" when this projectile is launched

    private void Awake()
    {
        //Reset stats
        stats.ResetStats(); //IMPORTANT: Other scripts will acess the properties in stats. They do not need to run this method, HOWEVER, they cannot acces 
        //the stats in AWAKE. Instead, do it in START. This is done to make sure that the stats are initialized before they are called, as other classes AWAKE
        //might run before this one.
        //Assign stats
        movementSpeed = stats.MovementSpeed;
        MinDistanceToDealDamage = stats.MinDistanceToDamage;
        projectileIsFired = false;
    }
    private void Update()
    //Explanation of virtual and override:
    /*
     * virtual, override, abstract are all used when extending a base class in object oriented programming. They let you redefine how a function or property works when extending a class. I'll use the terms base class and extended class in my explaination.

virtual and abstract are used in the base class. virtual means, use the extended class's version if it has one otherwise use this base class's version. abstract means the extended class MUST have a new version.

override is the extended classes way of saying, "Hey, I'm making a new version of this function/property! Don't use the base class's version."

Eg: I have a abstract base "Potion" class that has an abstract Use() function. If I make a "Health Potion" class that extends "Potion" then I can override Use() to heal the player. 
     */
    {
        if (_creepTarget != null && homingProjectile)
        {
            MovePorjectileWithHoming();
            RotateProjectile();
        }
        else if (_creepTarget != null && !homingProjectile)
        {
            projectileIsFired = true;
            MoveProjectileWithoutHoming();
            RotateProjectile();
        }
    }

    private void MovePorjectileWithHoming()
    {
        transform.position = Vector3.MoveTowards(transform.position, _creepTarget.transform.position, movementSpeed * Time.deltaTime); //Move the projectile
        float distanceToTarget = (_creepTarget.transform.position - transform.position).magnitude; //IMPORTANT AND USEFULL. THIS IS HOW YOU CAN GET A DISTANCE
        //BETWEEN TWO "Vector3" POINTS AS A "float"!
        if (distanceToTarget < MinDistanceToDealDamage) //Check if the projectile is close
        {
            _creepTarget._CreepHealth.DealDamage(Damage); //Fires the deal damage function in the creephealth reference
            //This is also why you sometimes want to declare FUNCTIONS with parameters. I would NOT have been able to specify the damage
            //from my damage var in THIS script, if the "DealDamage(float damage)" function took no "input".
            TurretOwner.ResetTurretProjectile();
            ObjectPooler.MoveToDeathPool(gameObject); //Return this projectile to the pool
        }
    }
    private void MoveProjectileWithoutHoming()
    {
        //Coppied for now 
        transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime); //Move the projectile
        float distanceToTarget = (targetPos - transform.position).magnitude; //IMPORTANT AND USEFULL. THIS IS HOW YOU CAN GET A DISTANCE
        ////BETWEEN TWO "Vector3" POINTS AS A "float"!
        if (distanceToTarget < MinDistanceToDealDamage) //Check if the projectile is close
        {
            projectileIsFired = false;
            _creepTarget._CreepHealth.DealDamage(Damage); //Fires the deal damage function in the creephealth reference
            //This is also why you sometimes want to declare FUNCTIONS with parameters. I would NOT have been able to specify the damage
            //from my damage var in THIS script, if the "DealDamage(float damage)" function took no "input".
            TurretOwner.ResetTurretProjectile();
            ObjectPooler.MoveToDeathPool(gameObject); //Return this projectile to the pool
        }
        
    }
    public Vector3 SetHitPossitionForNonHoming(Vector3 _possition)
    {
        return targetPos = _possition; //Updates the possition
    }
    private void RotateProjectile() //This may not work, and may not be needed for my functionallity.
    {
        Vector3 _direction = targetPos - transform.position;
        Quaternion _lookDirection = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookDirection, 50f * Time.deltaTime);
    }
    public void SetTarget(Creep creep)
    {
        _creepTarget = creep;
    }
    public void ResetProjectile()
    {
        _creepTarget = null;
        transform.localRotation = Quaternion.identity;
    }
}
