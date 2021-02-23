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
    [Tooltip("Assign the projectile type with the stats you want for this prefab. This must be done for BOTH tower and projectile prefab!")]
    public TowerProjectileTypeSO statsVersion1;     //Inherit stats from SO
    public TowerProjectileTypeSO statsVersion2;     //Inherit stats from SO
    public TowerProjectileTypeSO statsVersion3;     //Inherit stats from SO
    private TowerProjectileTypeSO statsAssignedVersion; //Set in script
    [Tooltip("How fast this projectile moves")]
    private float movementSpeed; //This might not be needed anymore
    public float ProjectileMovementSpeed { get; private set; }
    [Tooltip("Defines how close to the target the projectiles needs to be in order to hit it. The larger this radius is, the easier and ealier the collision")]
    [Range(0.1f, 1f)]
    private float MinDistanceToDealDamage;
    [Tooltip("Decides if this projectile will home in on enemies, or be aimed")]
    [SerializeField] private bool homingProjectile = false;
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
        statsAssignedVersion = statsVersion1;
        ABPos = transform.Find("ABPos");
        BCPos = transform.Find("BCPos");
        t = 0f;
        ProjectileMovementSpeed = movementSpeed;
        //Reset stats
        statsAssignedVersion.ResetStats(); //IMPORTANT: Other scripts will acess the properties in stats. They do not need to run this method, HOWEVER, they cannot acces 
        //the stats in AWAKE. Instead, do it in START. This is done to make sure that the stats are initialized before they are called, as other classes AWAKE
        //might run before this one.
        //Assign stats
        ProjectileMovementSpeed = statsAssignedVersion.MovementSpeed;
        MinDistanceToDealDamage = statsAssignedVersion.MinDistanceToDamage;
        projectileIsFired = false;
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
            ObjectPooler.MoveToDeathPool(gameObject); //Return this projectile to the pool
        }
    }
    private void MoveProjectileWithoutHoming()
    {
        if (t < 1f)
        {
            t += (Time.deltaTime * ProjectileMovementSpeed) / 10;
        }
        aPos = TurretOwner.transform.position + Vector3.up * 2.5f; //Needs improvement
        bPos = ((TurretOwner.transform.position + Vector3.up * statsAssignedVersion.TopProjectileHight) + targetPos) / 2;
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
            ObjectPooler.MoveToDeathPool(gameObject); //Return this projectile to the pool
            _creepTarget._CreepHealth.DealDamage(Damage); //Fires the deal damage function in the creephealth reference
            t = 0f;
            OnCreepHit?.Invoke(_creepTarget, Damage);//DELETE this later, as it is ONLY used for displaying DAMAGE NUMBERS
        }
        
    }
    public Vector3 SetPossitionsForNonHoming(Vector3 _possition)
    {
        return targetPos = _possition; //Updates the possition
    }
    private void RotateProjectile() //This may not work, and may not be needed for my functionallity. //NEEDS MORE WORK
    {
        Vector3 _riseDirection = bPos - transform.position; //For pointing upwards initially
        Vector3 _fallDirection = targetPos - transform.position; //For pointing downwards
        Quaternion _initialLookDirection = Quaternion.LookRotation(_riseDirection);
        Quaternion _fallLookDirection = Quaternion.LookRotation(_fallDirection);


        if (!projectileIsFired) //Look upwards
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _initialLookDirection, 100f * Time.deltaTime);
        }
        else if (projectileIsFired)
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
    public void UpdateProjectileStats(int towerUpgradePath)
    {
        //Assign stats
        switch(towerUpgradePath)
        {
            case 0:
                statsAssignedVersion = statsVersion1; //Set the initial stat version
                return;
            case 1:
                statsAssignedVersion = statsVersion1; //If tower upgrade path 1 is chosen
                return;
            case 2:
                statsAssignedVersion = statsVersion2; //If tower upgrade path 2 is chosen
                return;
            case 3:
                statsAssignedVersion = statsVersion3; //If tower upgrade path 3 is chosen
                return;
        }
        //Update stats
        ProjectileMovementSpeed = statsAssignedVersion.MovementSpeed;
        MinDistanceToDealDamage = statsAssignedVersion.MinDistanceToDamage;
    }
}
