﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class inherits from the scrTowerProjecctileLoader.
/// It is (so far) used exclusivly for the archer tower
/// </summary>
public class scrTowerArrows : scrTowerProjectileLoader
{
    protected override void Update()
    {
        if (Time.time > _nextAttackTime) //Checks that enough time has gone since the game started for the turret to load
        {
            if (mageTurret.CurrentCreepTarget != null)
            {
                Vector3 dirToTarget = mageTurret.CurrentCreepTarget.transform.position - transform.position;
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
