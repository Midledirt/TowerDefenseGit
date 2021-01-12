using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class inherits from the scrTowerProjecctileLoader.
/// It is (so far) used exclusivly for the archer tower
/// </summary>
public class scrTowerArrowsProjectileLoader : scrTowerMageProjectileLoader
{
    protected override void Update()
    {
        if (Time.time > _nextAttackTime) //Checks that enough time has gone since the game started for the turret to load
        {
            if (Tower.CurrentCreepTarget != null)
            {
                Vector3 dirToTarget = Tower.CurrentCreepTarget.transform.position - projectileSpawnPos.position; 
                //This makes the tower fire at the current possition of the target. This possition is not updated in the arrow script after
                //it is fired. Thus, the tower works quite well. However, it does not "lead the target yet..." Ill wait with this
                FireProjectile(dirToTarget);
            }
            _nextAttackTime = Time.time + delayBetweenAttacks; //This will always increment the amount of time that has gone with the 
            //delayBetweenAttacks.
        }
    }


    protected override void LoadProjectile()
    {
        
    }

    private void FireProjectile(Vector3 direction)
    {
        GameObject instance = _pooler.GetInstanceFromPool(); //See the pooler script for information about this method
        instance.transform.position = projectileSpawnPos.position; //Protected variable from the scrTurretProjectile class

        scrArrow projectile = instance.GetComponent<scrArrow>();
        projectile.Direction = direction;
        instance.SetActive(true);
    }
}
