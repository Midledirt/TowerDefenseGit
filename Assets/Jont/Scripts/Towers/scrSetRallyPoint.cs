using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// What I need to do in this script:
/// 1 Get the mouse possition
/// 2 Find a way to store mouse position on click
/// 3 Use that to set rally point
/// 4 Find out if THIS tower is selected
/// 5 Only allow above code to run if this tower is selected.
/// 
/// In other words, I need some script to manage the mouse potsition!
/// </summary>
public class scrSetRallyPoint : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; //So that we can check if we have clicked on the ground
    private Vector3 RallyPossition;
    [SerializeField] private GameObject testObject;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //This stores a position the right way
        {
            RaycastHit newRay; //Set up a new temporary var
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out newRay, Mathf.Infinity, groundLayer))
            {
                RallyPossition = newRay.point;
                Instantiate(testObject, RallyPossition, Quaternion.identity); //Spawn something for testing
            }
        }
    }

    public void SetRallyPoint() //This will be called by a UI button
    {
    }
}
