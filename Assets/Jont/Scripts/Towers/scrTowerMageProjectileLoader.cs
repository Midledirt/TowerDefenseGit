using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class takes the Objects in the pooler and runs the LoadProjectile function. Essentially, this is the fire mechanism of the tower
/// It is used (as is) for the "mage tower", and the "scrTowerArrows" also uses this class, as it derives from it.
/// </summary>
public class scrTowerMageProjectileLoader : MonoBehaviour
{
    [SerializeField] protected Transform projectileSpawnPos;
    [Tooltip("This is effectivly the reload time for this tower")]
    [Range(0.1f, 10f)]
    [SerializeField] protected float delayBetweenAttacks = 2f;
    [SerializeField] protected float damage = 2f;

    public float Damage { get; set; }

    protected float _nextAttackTime;
    protected ObjectPooler _pooler;
    protected scrTower Tower;
    protected scrMainProjectile currentProjectileLoaded;

    private void Start()
    {
        _pooler = GetComponent<ObjectPooler>(); //Gets the specific instance of a pooler script attached to THIS GAMEOBJECT
        Tower = GetComponent<scrTower>();

        Damage = damage;
        LoadProjectile();
    }

    protected virtual void Update()
    {
        if (IsTurretEmpty())
        {
            LoadProjectile();
        }

        if (Time.time > _nextAttackTime) //Checks that enough time has gone since the game started for the turret to load
        {
            if (Tower.CurrentCreepTarget != null && currentProjectileLoaded != null && Tower.CurrentCreepTarget._CreepHealth.currentHealth > 0f)
            //Check that the turret this script is attached to has a target, and that we do have a projectile ready (I think), and that the target of
            //the mage turrets health is greater than 0
            {
                currentProjectileLoaded.transform.parent = null; //"Release" the projectile
                currentProjectileLoaded.SetTarget(Tower.CurrentCreepTarget);
            }

            _nextAttackTime = Time.time + delayBetweenAttacks; //This will always increment the amount of time that has gone with the 
            //delayBetweenAttacks.
        }
    }

    protected virtual void LoadProjectile()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.transform.localPosition = projectileSpawnPos.position;

        newInstance.transform.SetParent(projectileSpawnPos); //THIS IS NECESSARY TO MAKE THE PROJECTILE INSTANCE FACE THE SAME WAY AS THE TURRET
        //AS IT SPAWNS

        currentProjectileLoaded = newInstance.GetComponent<scrMainProjectile>();
        currentProjectileLoaded.TurretOwner = this; //This is weird, covered in episode 26, around 3.30
        currentProjectileLoaded.ResetProjectile();
        currentProjectileLoaded.Damage = Damage; //Sets the "Damage" property in the scrMainProjectile 
        //(or scrArrow projectile, which derrives from this) to equal the "Damage" property in this script.
        newInstance.SetActive(true);
    }

    private bool IsTurretEmpty()
    {
        return currentProjectileLoaded == null; //Will be set to true, if this condition is met
    }
    public void ResetTurretProjectile()
    {
        currentProjectileLoaded = null;
    }
}
