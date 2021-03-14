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
    public void dealDamage() //Called by animation event
    {
        OnDealingDamage?.Invoke(5f); //Damage needs to inherit from stats
        print("DealingDamage");
        //Run the damage function for the target
    }
}
