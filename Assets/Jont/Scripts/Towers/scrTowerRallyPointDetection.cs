using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTowerRallyPointDetection : MonoBehaviour
{
    scrDefenderTowerTargets defenderTowerTargets;

    //This script needs a reference to the scrDefenderTowerTargets found in the tower object
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(defenderTowerTargets == null)
            {
                print("defenderTowerTargets is not assigned!");
                return;
            }
            print("defenderTowerTargets is assigned!");
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Creep"))
        {
            //print("RallyPoint detected creep");
            //Add detected creeps references to a list, in one of the tower scripts
            defenderTowerTargets.GetTargetReferenceFromRallyPoint(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Creep"))
        {
            //Creep theCreep = other.GetComponent<Creep>();
            //print("RallyPoint lost creep");
            defenderTowerTargets.LooseTargetReferenceFromRallyPoint(other.gameObject);
        }
    }
    public scrDefenderTowerTargets setReference(scrDefenderTowerTargets _reference)
    {
        return defenderTowerTargets = _reference;
    }
}
