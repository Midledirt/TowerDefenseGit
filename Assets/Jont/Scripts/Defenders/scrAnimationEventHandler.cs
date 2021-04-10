using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// This class MUST be placed with the animation controller, for the events to work!
/// </summary>
public class scrAnimationEventHandler : MonoBehaviour
{
    public Action<float> OnDealingDamage;
    private CreepStatsSO defenderOrCreepStats;
    private scrSplashDamage localSplashDamageClass;
    public bool UnitDealsSplashDamage { get; private set; }

    private void Awake()
    {
        defenderOrCreepStats = GetComponentInParent<scrCreepTypeDefiner>().CreepType; //Get the "creepType" from the stats on the gameobject
        localSplashDamageClass = GetComponentInParent<scrSplashDamage>(); //Gets the reference
        UnitDealsSplashDamage = false;
    }
    public void MakeUnitDealSplashDamage()
    {
        UnitDealsSplashDamage = true;
    }
    public void DefenderIsUpgraded(CreepStatsSO _newStats)
    {
        defenderOrCreepStats = _newStats;
    }
    public void dealDamage() //Called by animation event
    {
        if(!UnitDealsSplashDamage)
        {
            OnDealingDamage?.Invoke(defenderOrCreepStats.meleeDamage); //Damage needs to inherit from stats
        }
        if(UnitDealsSplashDamage)
        {
            localSplashDamageClass.DealSplashDamage(transform.position, defenderOrCreepStats.meleeDamage); //Deal damage at locaiton
        }
    }
}
