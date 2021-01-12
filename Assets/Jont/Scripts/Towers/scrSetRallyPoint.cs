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
    [SerializeField] private float rallyRange = 3f;
    
    

    private void Start()
    {
        
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rallyRange);
    }
}
