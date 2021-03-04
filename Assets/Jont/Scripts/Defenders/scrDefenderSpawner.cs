using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scrDefenderSpawner : MonoBehaviour
{
    private ObjectPooler pooler;
    private scrTowerRallypointPos rallyPointPos;
    private List<GameObject> defenders;
    [Tooltip("Set how fast the defenders move")]
    [SerializeField] private float defenderMovementSpeed;

    private void Awake()
    {
        pooler = GetComponent<ObjectPooler>(); //Get the instance on this tower
        rallyPointPos = GetComponent<scrTowerRallypointPos>(); //Get the instance on this tower
        defenders = new List<GameObject>(); //Initialize the list
    }

    private void Start()
    {
        spawnDefenders();
        orderDefendersToMoveTowardsTarget();
        moveTowardsTarget(); //This should get a while loop, so that it can run without being called here, and the end when defender reaches possition
    }
    private void spawnDefenders()
    {
        GameObject newInstance = pooler.GetInstanceFromPool();

        newInstance.SetActive(true);

        defenders.Add(newInstance); //Add defender to the list
    }
    public void orderDefendersToMoveTowardsTarget() //PUBLIC UNTIL I UPDATE THE "scrTowerRallyPointPos" script, with an action.
    {
        foreach(GameObject defender in defenders)
        {
            moveTowardsTarget();
        }
    }
    private void moveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, rallyPointPos.rallyPoint.transform.position, defenderMovementSpeed * Time.deltaTime);
    }
}
