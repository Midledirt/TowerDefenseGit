using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script handles creep attacks
/// </summary>
public class scrCreepAttack : MonoBehaviour
{
    scrAnimationEventHandler animationEventHandler;
    private List<GameObject> defenderTargets;
    scrCreepEngagementHandler engagementHandler;
    Creep thisCreep;

    private void Awake()
    {
        engagementHandler = GetComponent<scrCreepEngagementHandler>();
        animationEventHandler = GetComponentInChildren<scrAnimationEventHandler>();
        thisCreep = GetComponent<Creep>();
        defenderTargets = new List<GameObject>();
    }
    private void DealDamageToDefender(float _damage)
    {
        defenderTargets = GetComponent<Creep>().DefenderTargets; //This list is initialized in the awake method for creep
        //print("Deal damage is run from creep"); //Tested to work
        if (defenderTargets.Count > 0 && thisCreep.creepsFirstDefenderTarget != null) //This is where my current problem lies
        {
            //scrCreepHealth targetedDefenderHealth = defenderTargets[0].GetComponent<scrCreepHealth>();
            scrCreepHealth targetedDefenderHealth = thisCreep.creepsFirstDefenderTarget.GetComponent<scrCreepHealth>();
            if (targetedDefenderHealth.CurrentHealth <= 0)
            {
                //print("The defender I fought is dead");
                //engagementHandler.SetEngagementToFalse(); //Stop combat behaviour
                //Increment target
                if (defenderTargets.Count <= 0) //THIS IS THE CAUSE OF THE ISSUE!!!!!!!!!!!!!!
                {
                    engagementHandler.SetEngagementToFalse(); //Stop combat behaviour
                    return; 
                }
                else if(defenderTargets.Count > 0)
                {
                    for(int i = 0; i < defenderTargets.Count; i++)
                    {
                        thisCreep.creepsFirstDefenderTarget = defenderTargets[i]; //Attack the latest enemy to engage
                    }
                    return;
                }
            }
            //print("List is larger than 0"); //The list count was never greater than 0...
            targetedDefenderHealth.DealDamage(_damage);
        }
    }
    private void OnEnable() //If this works, move it to its own script
    {
        animationEventHandler.OnDealingDamage += DealDamageToDefender;
    }
    private void OnDisable()
    {
        animationEventHandler.OnDealingDamage -= DealDamageToDefender;
    }
}
