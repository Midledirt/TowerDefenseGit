using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// This class is placed on the defender tower. Used to keep reference of the creeps that enters the collider for the rally ponint, and send that
/// information to the defenders. 
/// </summary>
public class scrDefenderTowerTargets : MonoBehaviour
{
    public List<Creep> DefenderCreepList { get; private set; }
    public static Action<Creep, List<Defender>> LooseDefenderTarget;
    public List<Defender> Defenders { get; private set; }
    private void Awake()
    {
        DefenderCreepList = new List<Creep>();
        Defenders = new List<Defender>();
    }
    public void InitializeLocalDefenders(Defender _defender)
    {
        Defenders.Add(_defender); //Add defenders to list
    }
    public void GetTargetReferenceFromRallyPoint(GameObject other)
    {
        Creep newCreep = other.GetComponent<Creep>();
        DefenderCreepList.Add(newCreep);
        foreach(Defender _defender in Defenders)
        {
            _defender.NewCreepTargetInCollider(newCreep); //Tell the defender there is a new creep
        }
    }
    public void LooseTargetReferenceFromRallyPoint(GameObject other)
    {
        Creep theCreep = other.GetComponent<Creep>();
        if (DefenderCreepList.Contains(theCreep))
        {
            LooseDefenderTarget?.Invoke(theCreep, Defenders);
            DefenderCreepList.Remove(theCreep);
        }
        //print("Lost a creep reference: " + other);
    }
}
