using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script shall handle the activation of special abileties for defenders or creep!
/// </summary>
public class scrUnitSpecialAbileties : MonoBehaviour
{
    scrAnimationEventHandler localAnimationEventHandler;
    [Tooltip("Toggle wheter this unit deals splash damage or not")]
    [SerializeField] private bool unitDealsSplashDamage;

    private void Awake()
    {
        localAnimationEventHandler = GetComponentInChildren<scrAnimationEventHandler>();
    }
    private void Start()
    {
        if(unitDealsSplashDamage)
        {
            localAnimationEventHandler.MakeUnitDealSplashDamage();
        }
    }
}
