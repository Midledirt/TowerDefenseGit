using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scrDefenderSpawner : MonoBehaviour
{
    private ObjectPooler pooler;
    //private scrTowerRallypointPos rallyPointPos;
    private List<GameObject> defenders;
    scrTowerRallypointPos rallyPointUpdater;
    scrDefenderTowerTargets defenderTowerTargets;
    [Tooltip("Assign the main tower prefab for this. It is used for defender respawn possition")]
    public Transform defenderRespawnPos;

    private void Awake()
    {
        defenderTowerTargets = GetComponent<scrDefenderTowerTargets>();
        pooler = GetComponent<ObjectPooler>(); //Get the instance on this tower
        //rallyPointPos = GetComponent<scrTowerRallypointPos>(); //Get the instance on this tower
        rallyPointUpdater = GetComponent<scrTowerRallypointPos>(); //Get the instance
        defenders = new List<GameObject>(); //Initialize the list
    }
    private void Start()
    {
        spawnDefenders();
        if(defenderRespawnPos == null)
        {
            Debug.LogError("You forgot to assign the defenderRespawnPos for defender tower. See the scrDefenderSpawner class");
        }
    }
    private void spawnDefenders()
    {
        for (int i = 0; i < pooler.poolSizeCount; i++)
        {
            GameObject newInstance = pooler.GetInstanceFromPool();

            newInstance.transform.position = transform.position; //Spawn the defenders at tower location

            defenders.Add(newInstance); //Add defender to the list

            scrRallyPointPlacement defenderPossition = newInstance.GetComponent<scrRallyPointPlacement>(); //Get a reference to the instances defender stats script

            if (i != 0 && i < 4) //Assign possitions for the newly instantiated defenders
            {
                defenderPossition.DefenderPossition = i + 1;
            }
            scrDefenderAnimation defenderAnimator = newInstance.GetComponent<scrDefenderAnimation>();
            defenderAnimator.GetDefenderRespawnPossition(defenderRespawnPos.position); //Assign the tower possition as the respawn possition for defenders
            DefenderEngagementHandler defender = newInstance.GetComponent<DefenderEngagementHandler>();
            defender.AssignDefenderTowerTargets(defenderTowerTargets); //Assign this script for local defenders, so they can use the list of targets
            defenderTowerTargets.InitializeLocalDefenders(defender); //Assign the defenders to the defenderTowerTargets, so they can be issued orders from the defenderTowerTargetets script
            newInstance.SetActive(true); //Set defenders to active
        }
    }
    //Assign the defenders targets based on their number
    public void orderDefendersToMoveTowardsTarget(Vector3 rallyPointPossitionA, Vector3 rallyPointPossitionB, Vector3 rallyPointPossitionC) //Gets called from an Action in the scrTowerRallyPointPos
    {
        foreach(GameObject defender in defenders) //Runs for each defender in the list defender
        {
            scrDefenderMovement defenderMovement = defender.GetComponent<scrDefenderMovement>(); //Gets the defenders movement script

            scrRallyPointPlacement defenderPossition = defender.GetComponent<scrRallyPointPlacement>(); //Get a reference to the instances defender stats script

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
