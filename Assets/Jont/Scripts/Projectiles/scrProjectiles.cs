using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This class handles the movement of the mage projectile! Since other projectiles derrives from this, ive decided to call it "scrProjectiles".
/// </summary>
public class scrProjectiles : MonoBehaviour
{
    //Delete the following BEFORE launch
    public static Action<Creep, float> OnCreepHit; //This is only used for bug testing (displaying damage numbers)
    //Delete what is abowe, BEFORE launch
    [Header("Stats")]
    [Tooltip("Set how fast the projectile will move")]
    [SerializeField] private float projectileMovementSpeed = 10;
    public float ProjectileMovementSpeed { get; private set; }
    [Tooltip("Defines how close to the target the projectiles needs to be in order to hit it. The larger this radius is, the easier and ealier the collision")]
    [Range(0.1f, 1f)]
    [SerializeField] private float minDistanceToDealDamage = 0.1f;
    private float MinDistanceToDealDamage;
    [Tooltip("How high the arch for this projectile is.")]
    [SerializeField] private float topProjectileHight = 10f;
    [Tooltip("Decides if this projectile will home in on enemies, or be aimed")]
    [SerializeField] private bool homingProjectile = false;
    [Tooltip("If true, this projectile will deal splash damage without requiring upgrades")]
    [SerializeField] private bool dealsSplashDamageByDefault;
    public bool DealsSplashDamage { get; private set; }
    private scrSplashDamage localSplashDamage;
    public scrTowerProjectileLoader TurretOwner { get; set; } //Define a "Owner". `This is weird, covered in episode 26, around 3.30
    public float Damage { get; set; } //This damage is assigned from the tower via the scrTowerProjectileLoader Script
    private Creep _creepTarget;
    private Vector3 targetPos;
    [HideInInspector] public bool projectileIsFired; //Used to dissable "homing" when this projectile is launched

    private float t;
    private Vector3 aPos;
    private Vector3 bPos;
    private Vector3 cPos;
    private Transform ABPos;
    private Transform BCPos;

    private void Awake()
    {
        ABPos = transform.Find("ABPos");
        BCPos = transform.Find("BCPos");
        t = 0f;
        //Assign stats
        ProjectileMovementSpeed = projectileMovementSpeed;
        MinDistanceToDealDamage = minDistanceToDealDamage;
        projectileIsFired = false;
        DealsSplashDamage = dealsSplashDamageByDefault;
        localSplashDamage = GetComponent<scrSplashDamage>(); //Gets the reference
    }
    private void Update()
    {
        if (_creepTarget != null && homingProjectile)
        {
            MovePorjectileWithHoming();
            //RotateProjectile();
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
        transform.position = Vector3.MoveTowards(transform.position, _creepTarget.transform.position, ProjectileMovementSpeed * Time.deltaTime); //Move the projectile
        float distanceToTarget = (_creepTarget.transform.position - transform.position).magnitude; //IMPORTANT AND USEFULL. THIS IS HOW YOU CAN GET A DISTANCE
        //BETWEEN TWO "Vector3" POINTS AS A "float"!
        if (distanceToTarget < MinDistanceToDealDamage) //Check if the projectile is close
        {
            OnCreepHit?.Invoke(_creepTarget, Damage);//DELETE this later, as it is ONLY used for displaying DAMAGE NUMBERS

            _creepTarget._CreepHealth.DealDamage(Damage); //Fires the deal damage function in the creephealth reference
            //This is also why you sometimes want to declare FUNCTIONS with parameters. I would NOT have been able to specify the damage
            //from my damage var in THIS script, if the "DealDamage(float damage)" function took no "input".
            TurretOwner.ResetTurretProjectile();
            ObjectPooler.SetObjectToInactive(gameObject); //Return this projectile to the pool
        }
    }
    private void MoveProjectileWithoutHoming()
    {
        if (t < 1f)
        {
            t += (Time.deltaTime * ProjectileMovementSpeed) / 10;
        }
        aPos = TurretOwner.transform.position + Vector3.up * 2.5f; //Needs improvement
        bPos = ((TurretOwner.transform.position + Vector3.up * topProjectileHight) + targetPos) / 2;
        cPos = targetPos;
        //New movement

        ABPos.position = Vector3.Lerp(aPos, bPos, t); //Move from A to B

        BCPos.position = Vector3.Lerp(bPos, cPos, t); //Move from B to C 

        transform.position = Vector3.Lerp(ABPos.position, BCPos.position, t); //Move the projectile

        float distanceToTarget = (cPos - transform.position).magnitude; //IMPORTANT AND USEFULL. THIS IS HOW YOU CAN GET A DISTANCE
        ////BETWEEN TWO "Vector3" POINTS AS A "float"!
        if (distanceToTarget < MinDistanceToDealDamage) //Check if the projectile is close
        {
            projectileIsFired = false;
            TurretOwner.ResetTurretProjectile();
            ObjectPooler.SetObjectToInactive(gameObject); //Return this projectile to the pool
            if(!DealsSplashDamage)
            {
                _creepTarget._CreepHealth.DealDamage(Damage); //Fires the deal damage function in the creephealth reference
                OnCreepHit?.Invoke(_creepTarget, Damage);//DELETE this later, as it is ONLY used for displaying DAMAGE NUMBERS
            }
            if(DealsSplashDamage)
            {
                localSplashDamage.DealSplashDamage(transform.position, Damage); //Deal damage at location
            }
            t = 0f;
        }
        
    }
    public Vector3 SetPossitionsForNonHoming(Vector3 _possition)
    {
        //print("Got following possition for target nonHomingPoint: " + _possition);
        return targetPos = _possition; //Updates the possition
    }
    private void RotateProjectile() //This may not work, and may not be needed for my functionallity. //NEEDS MORE WORK
    {
        Vector3 _riseDirection = bPos - transform.position; //For pointing upwards initially
        Vector3 _fallDirection = targetPos - transform.position; //For pointing downwards
        Quaternion _initialLookDirection = Quaternion.LookRotation(_riseDirection);
        Quaternion _fallLookDirection = Quaternion.LookRotation(_fallDirection);


        if (!projectileIsFired) //Projectile faces upwards
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _initialLookDirection, 100f * Time.deltaTime);
        }
        else if (projectileIsFired) //Projectile faces downwards
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _fallLookDirection, 5f * Time.deltaTime);
        }
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
