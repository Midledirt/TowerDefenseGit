﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scrDefenderSpawner : MonoBehaviour
{
    private ObjectPooler pooler;
    //private scrTowerRallypointPos rallyPointPos;
    private List<GameObject> defenders;
    scrTowerRallypointPos rallyPointUpdater;
    [SerializeField] private float respawnTimer = 2f;

    private void Awake()
    {
        pooler = GetComponent<ObjectPooler>(); //Get the instance on this tower
        //rallyPointPos = GetComponent<scrTowerRallypointPos>(); //Get the instance on this tower
        rallyPointUpdater = GetComponent<scrTowerRallypointPos>(); //Get the instance
        defenders = new List<GameObject>(); //Initialize the list
    }

    private void Start()
    {
        spawnDefenders();
        //orderDefendersToMoveTowardsTarget();
        //moveTowardsTarget(); //This should get a while loop, so that it can run without being called here, and the end when defender reaches possition
    }
    private void spawnDefenders()
    {
        for (int i = 0; i < pooler.poolSizeCount; i++)
        {
            GameObject newInstance = pooler.GetInstanceFromPool();

            newInstance.transform.position = transform.position; //Spawn the defenders at tower location

            defenders.Add(newInstance); //Add defender to the list

            scrDefenderStats defenderPossition = newInstance.GetComponent<scrDefenderStats>(); //Get a reference to the instances defender stats script

            if (i != 0 && i < 4) //Assign possitions for the newly instantiated defenders
            {
                defenderPossition.DefenderPossition = i + 1;
            }

            newInstance.SetActive(true); //Set defenders to active
        }
    }
    //Assign the defenders targets based on their number
    public void orderDefendersToMoveTowardsTarget(Vector3 rallyPointPossitionA, Vector3 rallyPointPossitionB, Vector3 rallyPointPossitionC) //Gets called from an Action in the scrTowerRallyPointPos
    {
        foreach(GameObject defender in defenders) //Runs for each defender in the list defender
        {
            scrDefenderMovement defenderMovement = defender.GetComponent<scrDefenderMovement>(); //Gets the defenders movement script

            scrDefenderStats defenderPossition = defender.GetComponent<scrDefenderStats>(); //Get a reference to the instances defender stats script

            if (defenderPossition.DefenderPossition == 1)
            {
                defenderMovement.getRallyPointPos(rallyPointPossitionA); //Feeds the movement funciton for each defender, the possition of the rallypoint
            }
            if (defenderPossition.DefenderPossition == 2)
            {
                defenderMovement.getRallyPointPos(rallyPointPossitionB); //Feeds the movement funciton for each defender, the possition of the rallypoint

            }
            if (defenderPossition.DefenderPossition == 3)
            {
                defenderMovement.getRallyPointPos(rallyPointPossitionC); //Feeds the movement funciton for each defender, the possition of the rallypoint

            }
            else if (defenderPossition.DefenderPossition < 1 || defenderPossition.DefenderPossition > 3)
            {
                Debug.Log("Incorrect number assigned to DefenderPossition variable in scrDefenderSpawner script");
            }

            //Get instance of defenderMovement script
            //moveTowardsTarget();
        }
    }
    private void OnEnable()
    {
        rallyPointUpdater.OnSetRallyPoint += orderDefendersToMoveTowardsTarget;
    }
    private void OnDisable()
    {
        rallyPointUpdater.OnSetRallyPoint -= orderDefendersToMoveTowardsTarget;
    }
}
