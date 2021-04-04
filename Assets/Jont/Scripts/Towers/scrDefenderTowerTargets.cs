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
    public static Action<Creep, List<DefenderEngagementHandler>> LooseDefenderTarget;
    public List<DefenderEngagementHandler> Defenders { get; private set; }
    private void Awake()
    {
        DefenderCreepList = new List<Creep>();
        Defenders = new List<DefenderEngagementHandler>();
    }
    public void InitializeLocalDefenders(DefenderEngagementHandler _defender)
    {
        Defenders.Add(_defender); //Add defenders to list
    }
    public void GetTargetReferenceFromRallyPoint(GameObject other)
    {
        Creep newCreep = other.GetComponent<Creep>();
        scrCreepEngagementHandler _newCreepEngagementHandler = newCreep.GetComponent<scrCreepEngagementHandler>();
        if (newCreep._CreepHealth.CurrentHealth > 0f) //Extra check needed in case a creep that just died "slides" into the collider
        {
            DefenderCreepList.Add(newCreep);
            foreach(DefenderEngagementHandler _defender in Defenders) //Send all defenders to engage as none
            {
                if(_defender.defenderIsAlive && !_defender.thisDefenderIsEngagedAsMainTarget && !_defender.thisDefenderIsEngagedAsNoneTarget)
                {
                    //print("Sent a defender as none target");
                    _defender.DefenderEngageNewTargetAsNone(newCreep);
                }
            }
            for (int i = 0; i < Defenders.Count; i++) //Send a defender to engage as main
            {
                if (!Defenders[i].thisDefenderIsEngagedAsMainTarget && Defenders[i].defenderIsAlive && _newCreepEngagementHandler.CurrentTarget == null)
                {
                    //print("Sent a defender to engage new target: " + newCreep);
                    Defenders[i].DefenderEngageNewTargetAsMain(Defenders[i], newCreep);
                    return; //Only send one
                }
            }
        }
        /*foreach (DefenderEngagementHandler _defender in Defenders)
        {
            _defender.NewCreepTargetInCollider(newCreep); //Tell the defender there is a new creep
        }*/

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
    private void RemoveDeadCreepFromList(Creep _creep) //If the creep that died is in the current list...
    {
        if(DefenderCreepList.Contains(_creep))
        {
            DefenderCreepList.Remove(_creep); //Remove it
        }
    }
    private void OnEnable()
    {
        scrUnitHealth.OnEnemyKilled += RemoveDeadCreepFromList;
    }
    private void OnDisable()
    {
        scrUnitHealth.OnEnemyKilled -= RemoveDeadCreepFromList;
    }
}
