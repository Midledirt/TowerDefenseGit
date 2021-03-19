using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scrTowerRallypointPos : MonoBehaviour
{
    [Header("Rallypoint prefab")]
    [SerializeField] private GameObject rallyPointPrefab;
    [Header("Rally range")]
    [Range(2f, 5f)]
    [SerializeField] private float towerRallyRange; //Should inherit from a "towerStatsSO" in the future
    public Action<Vector3, Vector3, Vector3> OnSetRallyPoint; //Contains information for the three transform possitions of the rally points
    public GameObject rallyPoint { get; private set; }
    public Transform RallyPointAPos { get; private set; }
    public Transform RallyPointBPos { get; private set; }
    public Transform RallyPointCPos { get; private set; }

    private scrTowerTargeting towerParent;
    private float distanceFromTowerToRallypoint;
    BoundingSphere attackRange;
    private Vector3 latestViablePossition;

    private void Awake()
    {
        towerParent = GetComponent<scrTowerTargeting>();
        attackRange = new BoundingSphere(towerParent.transform.position, towerRallyRange);
    }
    private void Start()
    {
        if (towerParent != null && towerParent.TowerHasDefenders == true) //Dont move this to awake!
        {
            rallyPoint = Instantiate(rallyPointPrefab, transform.position, Quaternion.identity, towerParent.transform);
        }
        if (rallyPoint != null) //Check for null reference
        {
            //Assign reference of the scrDefenderTowerTargets script to the newly instantiated rallypoint
            scrDefenderTowerTargets _localDefenderTowerTargets = GetComponent<scrDefenderTowerTargets>(); //Find the script on this tower
            scrTowerRallyPointDetection _newlyInstantiatedRallyPointDetectionScripot = rallyPoint.GetComponent<scrTowerRallyPointDetection>();
            _newlyInstantiatedRallyPointDetectionScripot.setReference(_localDefenderTowerTargets); //Send it the reference

            RallyPointAPos = rallyPoint.transform.Find("PossitionA");
            RallyPointBPos = rallyPoint.transform.Find("PossitionB");
            RallyPointCPos = rallyPoint.transform.Find("PossitionC");
            //Debug.Log("Assigned rallyPoint points");
        }
    }
 
    public void SetRallypointPos(Vector3 possition) //Make this into an action so that it is easy to reference it in the "scrDefenderSpawner" class as well as future ones
    {
        distanceFromTowerToRallypoint = (towerParent.transform.position - possition).magnitude;
        if (distanceFromTowerToRallypoint <= attackRange.radius)
        {
            latestViablePossition = possition; //Update latestViablePossition
            rallyPoint.transform.position = possition; //Move the rallypoint
            //defenders.orderDefendersToMoveTowardsTarget();
            OnSetRallyPoint?.Invoke(RallyPointAPos.transform.position, RallyPointBPos.transform.position, RallyPointCPos.transform.position);
        }
        else
        {
            rallyPoint.transform.position = latestViablePossition; //Rallypoint remains at latest viable possition
        }
    }

    /*private void OnDrawGizmos() //THIS WILL CAUSE ERRORS WHILEST VIEWING THE TOWER PREFAB IN THE PREFAB WINDOW
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(towerParent.transform.position, attackRange.radius);
    }*/
    //Next is limiting the rallypoint possition to the actual range of the tower
    //And finding some way of setting a random possition 
    //How about creating an object that serves as the tower range, then use som function to get that objects extremeties?
}
