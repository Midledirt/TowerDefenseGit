using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A script for selecting objects in the scene. There may be a way better way of doing this though.
/// </summary>
public class scrNodeSelect : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayer; //Allows us to filther using layers
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        #region genericMousePosition
        if (Input.GetMouseButtonDown(0)) //This should be the left mouse button
        {
            //Instantiate(TestObject, GetMousePosition(), Quaternion.identity); //Instantiate something at the mouse position
            RaycastHit rayHit; //Set up a new variable for storing raycast hit information

            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickableLayer)) //If "fire a raycast"
                //fire a ray from the main camera towards the mouse position. IMPORTANT: From what I understand, you should not use "Camera.main" 
                //in a final product. Instead I would probably need to use some variable that stores the camera I am using?
                //ALSO: "Out" allows you to choose what to store this information to. So "out rayHit" means store this info in rayHit.
            {
                rayHit.collider.GetComponent<scrTowerNode>().WhenClicked(); //IMPORTANT:
                //What I love about this is that is allows us to get classes from objects that we already know have those classes, because they are
                //on the "clickableLayer". This should prevent null references. However, as this is setup right now, it will check for a specific
                //script (scrTowerNode) and will thus only work for objects CONTAINING that script. 
                //This works for now, however, in the future, I will need to make some changes. My first idea right now would be to do the following:
                //Have a "scrClickable" class that is placed on everything that can be clicked, allowing this (scrNodeSelect) class to only communicate
                //with that one "scrClickable" class. Then, the "scrClickable" class checks for whatever classes it contain that may run code when clicked
                //through something like a for loop. I think this approach sould work fine in theory, however, it may be incredebly un-optimized. So look
                //for better solutions, if you do go for this approach.
                //May have a better way: Use Actions. OnEnable and OnDissable. It is used in episode 46. And there should be info online about it
            }
        }
        #endregion
    }
}
