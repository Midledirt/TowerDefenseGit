using System.Collections;
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
    public TowerProjectileTypeSO stats;     //Inherit stats from SO
    [SerializeField] protected Transform projectileSpawnPos;
    [Tooltip("This is effectivly the reload time for this tower")]
    [Range(0.1f, 10f)]
    private float delayBetweenAttacks;
    private float towerAimValue = 0f; //his value decides how far ahead the tower will aim in order to hit consistently. Defaults to 1.

    public float Damage { get; set; }

    protected float _nextAttackTime;
    protected ObjectPooler _pooler;
    protected scrTowerTargeting Tower;
    protected scrProjectiles currentProjectileLoaded; //Lets keep this reference
    private Vector3 nonHomingHitPoint;

    private void Start()
    {
        stats.ResetStats();
        _pooler = GetComponent<ObjectPooler>(); //Gets the specific instance of a pooler script attached to THIS GAMEOBJECT
        Tower = GetComponent<scrTowerTargeting>();

        //IMPORTANT: Other scripts will acess the properties in stats. They do not need to run this method, HOWEVER, they cannot acces 
        //the stats in AWAKE. Instead, do it in START. This is done to make sure that the stats are initialized before they are called, as other classes AWAKE
        //might run before this one.
        //Assign stats
        delayBetweenAttacks = stats.DelayBetweenAttacks;
        Damage = stats.ProjectileDamage;
        LoadProjectile();
    }

    private void Update()
    {
        if (Time.time > _nextAttackTime) //Checks that enough time has gone since the game started for the turret to load
        {
            LoadProjectile();
            if (Tower.CurrentCreepTarget != null && currentProjectileLoaded != null && Tower.CurrentCreepTarget._CreepHealth.currentHealth > 0f)
            //Check that the turret this script is attached to has a target, and that we do have a projectile ready (I think), and that the target of
            //the mage turrets health is greater than 0
            {
                currentProjectileLoaded.transform.parent = null; //"Release" the projectile
                currentProjectileLoaded.SetTarget(Tower.CurrentCreepTarget);
                //nonHomingHitPoint = Tower.CurrentCreepTarget.transform.position; //This might be where i need to do some magic to improve the aim
                nonHomingHitPoint = Tower.CurrentCreepTarget.myPath.path.GetPointAtDistance((Tower.CurrentCreepTarget.DistanceTravelled + ((Tower.CurrentCreepTarget.MovementSpeed * 2)/(currentProjectileLoaded.GetComponent<scrProjectiles>().ProjectileMovementSpeed / 2))) + towerAimValue, Tower.CurrentCreepTarget.endOfPathInstruction);
                if (!currentProjectileLoaded.projectileIsFired)
                {
                    currentProjectileLoaded.projectileIsFired = true;
                    currentProjectileLoaded.SetHitPossitionForNonHoming(nonHomingHitPoint); //This might be where i need to do some magic to improve the aim
                }
            }
            _nextAttackTime = Time.time + delayBetweenAttacks; //This will always increment the amount of time that has gone with the 
            //delayBetweenAttacks.
        }
    }

    private void LoadProjectile()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.transform.localPosition = projectileSpawnPos.position;

        newInstance.transform.SetParent(projectileSpawnPos); //THIS IS NECESSARY TO MAKE THE PROJECTILE INSTANCE FACE THE SAME WAY AS THE TURRET
        //AS IT SPAWNS

        currentProjectileLoaded = newInstance.GetComponent<scrProjectiles>();
        currentProjectileLoaded.TurretOwner = this; //This is weird, covered in episode 26, around 3.30
        currentProjectileLoaded.ResetProjectile();
        currentProjectileLoaded.Damage = Damage; //Sets the "Damage" property in the scrProjectiles 
        //(or scrArrow projectile, which derrives from this) to equal the "Damage" property in this script.
        newInstance.SetActive(true);
    }

    /*private bool IsTurretEmpty()
    {
        return currentProjectileLoaded == null; //Will be set to true, if this condition is met
    }*/ //Not in use currently, might delete
    public void ResetTurretProjectile()
    {
        currentProjectileLoaded = null;
    }
}
