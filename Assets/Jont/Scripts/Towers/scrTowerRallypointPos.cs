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
    public Action<Vector3, Vector3, Vector3> OnSetRallyPoint; //Contains information for the three transform possitions of the rally points
    public GameObject rallyPoint { get; private set; }
    public Transform RallyPointAPos { get; private set; }
    public Transform RallyPointBPos { get; private set; }
    public Transform RallyPointCPos { get; private set; }

    private scrTowerTargeting towerParent;
    private float distanceFromTowerToRallypoint;
    private SphereCollider attackRange;
    private Vector3 latestViablePossition;

    private void Awake()
    {
        towerParent = GetComponent<scrTowerTargeting>();
    }
    private void Start()
    {
        attackRange = towerParent.collider; //Get the same collider used in the towerParent script
        //attackRange = new BoundingSphere(towerParent.transform.position, towerParent.AttackRange);

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
        if (distanceFromTowerToRallypoint <= (attackRange.radius / 2.5))
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
}
