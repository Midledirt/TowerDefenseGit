using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scrDefenderSpawner : MonoBehaviour
{
    private ObjectPooler pooler;
    //private scrTowerRallypointPos rallyPointPos;
    private List<GameObject> defenders;
    scrTowerRallypointPos rallyPointUpdater;


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
        GameObject newInstance = pooler.GetInstanceFromPool();

        newInstance.SetActive(true);

        defenders.Add(newInstance); //Add defender to the list
    }
    public void orderDefendersToMoveTowardsTarget(Vector3 rallyPointPossition) //Gets called from an Action in the scrTowerRallyPointPos
    {
        foreach(GameObject defender in defenders) //Runs for each defender in the list defender
        {
            scrDefenderMovement defenderMovement = defender.GetComponent<scrDefenderMovement>(); //Gets the defenders movement script

            defenderMovement.getRallyPointPos(rallyPointPossition); //Feeds the movement funciton for each defender, the possition of the rallypoint
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
