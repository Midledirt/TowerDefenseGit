using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scrDefenderTowerTargets : MonoBehaviour
{
    public List<Creep> DefenderCreepList { get; private set; }
    public static Action<Creep> LooseDefenderTarget;
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
        //Assign creep as main target if there are no other targets
        if(DefenderCreepList.Count == 1) //Check that there is exactly one target in the list
        {
            foreach(Defender _defender in Defenders)
            {
                _defender.SetFirstTarget(); //Makes the defenders engage the first target
            }
        }
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
