using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Defender : MonoBehaviour
{
    [Tooltip("Drag the body of the defender itself into this slot")]
    [SerializeField] private GameObject defenderBody;

    private scrAnimationEventHandler animEventHandler;
    private scrDefenderTowerTargets defenderTowerTargets;
    private scrDefenderMovement defenderMovement;
    private void Awake()
    {
        defenderMovement = GetComponent<scrDefenderMovement>();
        animEventHandler = GetComponentInChildren<scrAnimationEventHandler>(); //Get the instance
    }
    public scrDefenderTowerTargets AssignDefenderTowerTargets(scrDefenderTowerTargets _defenderTowerTargets)
    {
        //print("Local defender targets assigned to defender");
        return defenderTowerTargets = _defenderTowerTargets; //Gets the reference
    }
}
