using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTowerRallypointPos : MonoBehaviour
{
    [Header("Rallypoint prefab")]
    [SerializeField] private GameObject rallyPointPrefab;
    [Header("Rally range")]
    [Range(2f, 5f)]
    [SerializeField] private float towerRallyRange; //Should inherit from a "towerStatsSO" in the future
    private GameObject rallyPoint;
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
        if (towerParent != null && towerParent.TowerHasDefenders == true) //Move this to awake later
        {
            rallyPoint = Instantiate(rallyPointPrefab, transform.position, Quaternion.identity, towerParent.transform);
        }
    }
 
    public void SetRallypointPos(Vector3 possition)
    {
        distanceFromTowerToRallypoint = (towerParent.transform.position - possition).magnitude;
        if (distanceFromTowerToRallypoint <= attackRange.radius)
        {
            latestViablePossition = possition; //Update latestViablePossition
            rallyPoint.transform.position = possition; //Move the rallypoint
            //print("Inside circle");
        }
        else
        {
            rallyPoint.transform.position = latestViablePossition; //Rallypoint remains at latest viable possition
            //print("outside circle");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(towerParent.transform.position, attackRange.radius);
    }
    //Next is limiting the rallypoint possition to the actual range of the tower
    //And finding some way of setting a random possition 
    //How about creating an object that serves as the tower range, then use som function to get that objects extremeties?
}
