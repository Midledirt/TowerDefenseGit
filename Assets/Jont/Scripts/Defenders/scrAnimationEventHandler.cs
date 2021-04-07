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
    CreepStatsSO defenderOrCreepStats;

    private void Awake()
    {
        defenderOrCreepStats = GetComponentInParent<scrCreepTypeDefiner>().CreepType; //Get the "creepType" from the stats on the gameobject
    }
    public void DefenderIsUpgraded(CreepStatsSO _newStats)
    {
        defenderOrCreepStats = _newStats;
    }
    public void dealDamage() //Called by animation event
    {
        OnDealingDamage?.Invoke(defenderOrCreepStats.meleeDamage); //Damage needs to inherit from stats
        //print("DealingDamage");
        //Run the damage function for the target
    }
}
