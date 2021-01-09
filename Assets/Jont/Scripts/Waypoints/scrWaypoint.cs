using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class draws the waypoints in the "scene-view". 
/// </summary>
public class scrWaypoint : MonoBehaviour
{
    [SerializeField] private Vector3[] points;

    //These following 2 lines are used as references for the waypoint editor. See Section 3, episode 7 for more info
    public Vector3[] Points => points;
    public Vector3 CurrentPosition => currentPosition;

    private Vector3 currentPosition;
    private bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = true;
        currentPosition = transform.position;
    }

    public Vector3 GetWaypointPosition(int index)
    {
        return currentPosition + points[index];
    }

    private void OnDrawGizmos()
    {
        //Make it possible to edit the position of the waypoints through their parent object, in the inspector
        if (!gameStarted && transform.hasChanged)
        {
            currentPosition = transform.position;
        }

        for (int i = 0; i < points.Length; i++)
        {
            //Draws a green sphere at the position of each "point".
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(points[i] + currentPosition, 0.5f);

            if (i < points.Length - 1)
            {
                //Draws a line between each point in the array, with the exception of the last point in the array.
                Gizmos.color = Color.red;
                Gizmos.DrawLine(points[i] + currentPosition, points[i + 1] + currentPosition);
            }
        }

    }
}
