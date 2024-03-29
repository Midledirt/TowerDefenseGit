﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSetRallyPoint : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayer; //Allows us to filther using layers
    private Camera mainCamera;
    private bool recordingMousePos = false;
    private RaycastHit hit;
    private Vector3 rallyPointPossition;
    private scrTowerNode _currentNodeSelected;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (recordingMousePos) // 2. Vi starter å recorde posisjonen til musen
        {
            GetMousePosition(rallyPointPossition);
            if (Input.GetMouseButtonDown(0) && recordingMousePos)  //3. Vi slutter å recorde possisjonen til musen, men først...
            {
                SetTheRallyPoint(); //4. Oppdaterer vi rally point possisjonen med posisjonen til musen.
                recordingMousePos = false; //Turn recording mouse pos of again
            }
        }
    }
    public void StartRecordingMousePos() // 1. Denne blir called fra rally point knapp
    {
        recordingMousePos = true;
    }
    private void SetTheRallyPoint()
    {
        rallyPointPossition = hit.point;
        _currentNodeSelected.TowerRallypointPos.SetRallypointPos(rallyPointPossition);         //No issues here, tested!
    }

    private Vector3 GetMousePosition(Vector3 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
        {
            return hit.point;
        }
        //print("Got no position for the mouse");
        return new Vector3(0, 0, 0);
    }
    private void NodeSelected(scrTowerNode nodeSelected) //gets the reference
    {
        _currentNodeSelected = nodeSelected;
       //get reference to a tower rallypoint button
    }
    private void OnEnable()
    {
        scrTowerNode.OnNodeSelected += NodeSelected;
    }
    private void OnDisable()
    {
        scrTowerNode.OnNodeSelected -= NodeSelected;
    }
}
