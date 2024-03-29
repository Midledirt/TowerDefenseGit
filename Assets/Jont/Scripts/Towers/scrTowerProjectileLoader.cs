﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class takes the Objects in the pooler and runs the LoadProjectile function. Essentially, this is the fire mechanism of the tower
/// It is used (as is) for the "mage tower", and the "scrTowerArrows" also uses this class, as it derives from it.
/// </summary>
public class scrTowerProjectileLoader : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Assign the projectile type with the stats you want for this prefab. This must be done for BOTH tower and projectile prefab!")]
    private TowerProjectileTypeSO assignedStats;     //Inherit stats from abowe stats
    [SerializeField] protected Transform projectileSpawnPos;
    [Tooltip("This is effectivly the reload time for this tower")]
    [Range(0.1f, 10f)]
    private float delayBetweenAttacks;
    private float towerAimValue = 10f; //his value decides how far ahead the tower will aim in order to hit consistently. Defaults to 10.
    private scrTowerProjectileStatsRecorder projectileStatsTracker;
    public float Damage { get; set; }

    protected float _nextAttackTime;
    protected ObjectPooler _pooler;
    protected scrTowerTargeting Tower;
    protected scrProjectiles currentProjectileLoaded; //Lets keep this reference
    private Vector3 nonHomingHitPoint; //The point that towers without homing will aim at in order to hit the target
    [Tooltip("Decides how fast a creep must be moving before there is a chance for missfire")]
    [Range (1f, 10f)]
    [SerializeField] private float projectileMissfireTreshold;

    private void Awake()
    {
        projectileStatsTracker = GetComponent<scrTowerProjectileStatsRecorder>();
        _pooler = GetComponent<ObjectPooler>(); //Gets the specific instance of a pooler script attached to THIS GAMEOBJECT
        assignedStats = projectileStatsTracker.SetInitialStasts(); //Initializes the stats as that of the default verson
    }

    private void Start()
    {
        assignedStats.ResetStats();
        Tower = GetComponent<scrTowerTargeting>();

        //IMPORTANT: Other scripts will acess the properties in stats. They do not need to run this method, HOWEVER, they cannot acces 
        //the stats in AWAKE. Instead, do it in START. This is done to make sure that the stats are initialized before they are called, as other classes AWAKE
        //might run before this one.
        //Assign stats
        delayBetweenAttacks = assignedStats.DelayBetweenAttacks;
        Damage = assignedStats.ProjectileDamage;
        LoadProjectile();
    }
    private void Update()
    {
        if (Time.time > _nextAttackTime && Tower.CurrentCreepTarget != null) //Checks that enough time has gone since the game started for the turret to load
        {
            LoadProjectile();
            if (Tower.CurrentCreepTarget != null && currentProjectileLoaded != null && Tower.CurrentCreepTarget._CreepHealth.CurrentHealth > 0f)
            //Check that the turret this script is attached to has a target, and that we do have a projectile ready (I think), and that the target of
            //the mage turrets health is greater than 0
            {
                currentProjectileLoaded.transform.parent = null; //"Release" the projectile
                currentProjectileLoaded.SetTarget(Tower.CurrentCreepTarget);
                if (Tower.CurrentCreepTarget.MovementSpeed >= 3f) //For targeting most enemies
                {
                    //print("Firing normal projectile");
                    nonHomingHitPoint = (Tower.CurrentCreepTarget.myPath.path.GetPointAtDistance((Tower.CurrentCreepTarget.DistanceTravelled + ((Tower.CurrentCreepTarget.MovementSpeed) - (currentProjectileLoaded.GetComponent<scrProjectiles>().ProjectileMovementSpeed))) + towerAimValue, Tower.CurrentCreepTarget.endOfPathInstruction)) + Tower.CurrentCreepTarget.RandomizedPossition;
                    if (Tower.CurrentCreepTarget.MovementSpeed > projectileMissfireTreshold)
                    {
                        if (GetRandomNumber(0, 6) > 4) //Gives us a 1/6 chance that a projectile will missfire
                        {
                            //print("Firing missfire projectile");
                            nonHomingHitPoint = (Tower.CurrentCreepTarget.myPath.path.GetPointAtDistance((Tower.CurrentCreepTarget.DistanceTravelled + (((Tower.CurrentCreepTarget.MovementSpeed) + 2) - (currentProjectileLoaded.GetComponent<scrProjectiles>().ProjectileMovementSpeed))) + GetRandomNumber(towerAimValue * -1, towerAimValue * 1.5f), Tower.CurrentCreepTarget.endOfPathInstruction)) + Tower.CurrentCreepTarget.RandomizedPossition;
                        }
                    }
                    FireWhenReady();
                }
                if (Tower.CurrentCreepTarget.MovementSpeed < 3f) //For targeting really slow enemies
                {
                    if(Tower.CurrentCreepTarget.MovementSpeed <= 0.1f) //If target is practically standing still
                    {
                        nonHomingHitPoint = (Tower.CurrentCreepTarget.myPath.path.GetPointAtDistance(Tower.CurrentCreepTarget.DistanceTravelled, Tower.CurrentCreepTarget.endOfPathInstruction)) + Tower.CurrentCreepTarget.RandomizedPossition;
                        //print("Firing projectile for 0 speed target. Destination is: " + nonHomingHitPoint);
                        FireWhenReady();
                    }
                    else
                    //print("Firing projectile for slow speed target");
                    nonHomingHitPoint = (Tower.CurrentCreepTarget.myPath.path.GetPointAtDistance((Tower.CurrentCreepTarget.DistanceTravelled + (((Tower.CurrentCreepTarget.MovementSpeed)+2) - (currentProjectileLoaded.GetComponent<scrProjectiles>().ProjectileMovementSpeed))) + towerAimValue, Tower.CurrentCreepTarget.endOfPathInstruction)) + Tower.CurrentCreepTarget.RandomizedPossition;
                    FireWhenReady();
                }
                //PROBLEM: "nonHomingHitPoint" can take a value that is greater than path length. This can be avoided through level design though.
                //The value abowe "nonHomingHitPoint" needs to take into acount that the movement speed of the projectile is modified in the scrProjectile script. As such, 
                //I will probably need to do a lot of testing in order to find the correct value
            }
            _nextAttackTime = Time.time + delayBetweenAttacks; //This will always increment the amount of time that has gone with the 
                                                               //delayBetweenAttacks.
        }
    }
    private void FireWhenReady()
    {
        if (!currentProjectileLoaded.projectileIsFired)
        {
            currentProjectileLoaded.projectileIsFired = true;
            currentProjectileLoaded.SetPossitionsForNonHoming(nonHomingHitPoint); //This might be where i need to do some magic to improve the aim
        }
    }
    private float GetRandomNumber(float minValue, float maxValue)
    {
        return Random.Range(minValue, maxValue);
    }
    private void LoadProjectile()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool(); //Removes an instance from the pool
        newInstance.transform.localPosition = projectileSpawnPos.position; //Gives it this spawning possition

        newInstance.transform.SetParent(projectileSpawnPos); //THIS IS NECESSARY TO MAKE THE PROJECTILE INSTANCE FACE THE SAME WAY AS THE TURRET
        //AS IT SPAWNS

        currentProjectileLoaded = newInstance.GetComponent<scrProjectiles>(); //Assignes it as the "currentprojectileloaded"
        currentProjectileLoaded.TurretOwner = this; //This is weird, covered in episode 26, around 3.30
        currentProjectileLoaded.ResetProjectile(); //Removes any prior creep target, and resets the rotation
        currentProjectileLoaded.Damage = Damage; //Sets the "Damage" property in the scrProjectiles 
        newInstance.SetActive(true); //Activates it
    }

    public void UpdateProjectileStats(int _towerPath, int _towerLevel)
    {
        assignedStats = projectileStatsTracker.UpgradeProjectileStats(_towerPath, _towerLevel); //Gets the projectile stats from the projectile tracker
        assignedStats.ResetStats(); //Makes sure the stats are kept to assigned values
        delayBetweenAttacks = assignedStats.DelayBetweenAttacks;
        Damage = assignedStats.ProjectileDamage;
    }
    public void ResetTurretProjectile()
    {
        currentProjectileLoaded = null;
    }
}
