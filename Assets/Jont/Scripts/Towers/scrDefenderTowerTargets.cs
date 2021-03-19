using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scrDefenderTowerTargets : MonoBehaviour
{
    public List<Creep> DefenderCreepList { get; private set; }
    public static Action<Creep> LooseDefenderTarget;
    private void Awake()
    {
        DefenderCreepList = new List<Creep>();
    }
    public void GetTargetReferenceFromRallyPoint(GameObject other)
    {
        Creep newCreep = other.GetComponent<Creep>();
        DefenderCreepList.Add(newCreep);
    }
    public void LooseTargetReferenceFromRallyPoint(GameObject other)
    {
        Creep theCreep = other.GetComponent<Creep>();
        if (DefenderCreepList.Contains(theCreep))
        {
            LooseDefenderTarget?.Invoke(theCreep);
            DefenderCreepList.Remove(theCreep);
        }
        //print("Lost a creep reference: " + other);
    }
}
