using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// This class needs to become a singleton. Each of the scripts that are on objects that STARTS OUT ALREADY IN THE HIERARCHY should probably
/// be singletons. 
/// </summary>
public class scrCameraMovement : MonoBehaviour
{
    private Transform followTransform;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float normalSpeed;
    [SerializeField] private float fastSpeed;
    private float movementSpeed;
    [SerializeField] private float movementTime;
    [SerializeField] private float rotationAmount;
    [SerializeField] private Vector3 zoomAmount;


    private Vector3 newPossition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private Vector3 dragStartPossitino;
    private Vector3 dragCurrentPossition;
    private Vector3 rotateStartPossition;
    private Vector3 rotateCurrentPossition;

    private void Start()
    {
        newPossition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        if(followTransform != null) //This is currently not used!
        {
            transform.position = followTransform.position;
        }
        else
        {
            HandleMovementInput();
            HandleMouseInput();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }
    }
    private void HandleMouseInput()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        if (EventSystem.current.IsPointerOverGameObject()) //This is taken from a youtube video by Jason Weimann ("Avoid/Detect clicks through your UI")
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) //Left mouse button on click
        {
            Plane plane = new Plane(Vector3Int.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPossitino = ray.GetPoint(entry);
            }
        }
        if(Input.GetMouseButton(0)) //Left mouse button if we are still holding it
        {
            Plane plane = new Plane(Vector3Int.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragCurrentPossition = ray.GetPoint(entry);

                newPossition = transform.position + dragStartPossitino - dragCurrentPossition;
            }
        }
        if(Input.GetMouseButtonDown(2))
        {
            rotateStartPossition = Input.mousePosition;
        }
        if(Input.GetMouseButton(2))
        {
            rotateCurrentPossition = Input.mousePosition;

            Vector3 difference = rotateStartPossition - rotateCurrentPossition;
            rotateStartPossition = rotateCurrentPossition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5));
        }
    }
    private void HandleMovementInput()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        #region movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPossition += (transform.forward * movementSpeed);
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPossition += (transform.forward * -movementSpeed);
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPossition += (transform.right * -movementSpeed);
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPossition += (transform.right * movementSpeed);
        }
        #endregion
        #region rotation
        if(Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if(Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        #endregion
        #region Zoom
        if(Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if(Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }
        #endregion
        transform.position = Vector3.Lerp(transform.position, newPossition, movementTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTime * Time.deltaTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, movementTime * Time.deltaTime);
    }
}
