using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script handles creep attacks
/// </summary>
public class scrCreepAttack : MonoBehaviour
{
    scrAnimationEventHandler animationEventHandler;
    private List<GameObject> defenderTargetsFromCreep;

    private void Awake()
    {
        animationEventHandler = GetComponentInChildren<scrAnimationEventHandler>();
    }
    private void DealDamageToDefender(float _damage)
    {
        defenderTargetsFromCreep = GetComponent<Creep>().DefenderTargets; //This list is initialized in the awake method for creep
        //print("Deal damage is run from creep"); //Tested to work
        if (defenderTargetsFromCreep.Count > 0) //This is where my current problem lies
        {
            //print("List is larger than 0"); //The list count was never greater than 0...
            scrCreepHealth targetedDefender = defenderTargetsFromCreep[0].GetComponent<scrCreepHealth>();
            targetedDefender.DealDamage(_damage);
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
