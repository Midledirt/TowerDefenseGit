using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles the movement of the arrow projectile
/// This class derives from the scrMainProjectile. At least for now. They share a lot of common logic.
/// </summary>

public class scrArrow : MonoBehaviour // inherit from this as these will be very similar
{
    [Tooltip("How fast this projectile moves")]
    [SerializeField] protected float movementSpeed = 10f; //These will be "protected" instead of "private" because this will allow 
    [Tooltip("Defines how close to the target the projectiles needs to be in order to hit it. The larger this radius is, the easier and ealier the collision")]
    [Range(0.1f, 1f)]
    [SerializeField] private float MinDistanceToDealDamage = 0.1f;

    //public bool isMageBolt = true; //This is my way of making the "scrTurretProjectiles" class know if this is an arrow or a mage bolt

    public scrTowerArrowsProjectileLoader TurretOwner { get; set; } //Define a "Owner". `This is weird, covered in episode 26, around 3.30
    public float Damage { get; set; }

    protected Creep _creepTarget;

    protected virtual void Update()
    //Explanation of virtual and override:
    /*
     * virtual, override, abstract are all used when extending a base class in object oriented programming. They let you redefine how a function or property works when extending a class. I'll use the terms base class and extended class in my explaination.

virtual and abstract are used in the base class. virtual means, use the extended class's version if it has one otherwise use this base class's version. abstract means the extended class MUST have a new version.

override is the extended classes way of saying, "Hey, I'm making a new version of this function/property! Don't use the base class's version."

Eg: I have a abstract base "Potion" class that has an abstract Use() function. If I make a "Health Potion" class that extends "Potion" then I can override Use() to heal the player. 
     */
    {
        if (_creepTarget != null)
        {
            MovePorjectile();
            RotateProjectile(); //This may not be necessary
        }
    }

    protected virtual void MovePorjectile()
    {
        //This may cause problems...
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

    private void RotateProjectile() //This may not work, and may not be needed for my functionallity.
    {
        Vector3 targetPos = _creepTarget.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.up, targetPos, transform.forward); //This is not tested for 3D objects. Might not work
        transform.Rotate(0f, angle, 0f);
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
