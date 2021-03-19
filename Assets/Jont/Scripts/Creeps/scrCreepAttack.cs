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
    Creep theCreep;

    private void Awake()
    {
        animationEventHandler = GetComponentInChildren<scrAnimationEventHandler>();
        theCreep = GetComponent<Creep>();
    }
    private void DealDamageToDefender(float _damage)
    {
        defenderTargets = GetComponent<Creep>().DefenderTargets; //This list is initialized in the awake method for creep
        //print("Deal damage is run from creep"); //Tested to work
        if (defenderTargets.Count > 0) //This is where my current problem lies
        {
            scrCreepHealth targetedDefenderHealth = defenderTargets[0].GetComponent<scrCreepHealth>();
            if (targetedDefenderHealth.CurrentHealth <= 0)
            {
                print("The defender I fought is dead");
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
