using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is currnetly (19.03.21) used by the defender tower, mainly for references it seems. This is horrible for optimization, and I need to remove this
/// at a later point
/// </summary>
public class scrTowerTargeting : MonoBehaviour
{
    [SerializeField] private float attackRange = 3f;
    [Tooltip("Assign the tower range object as the range sprite")]
    [SerializeField] private GameObject rangeSprite;
    public scrUppgradeTowers TowerUpgrade { get; set; }
    public Creep CurrentCreepTarget { get; private set; }

    [Header("Tower has defenders?")]
    [Tooltip("Decides wheter or not this tower type has defenders by default")]
    [SerializeField] private bool towerHasDefenders = false;
    public bool TowerHasDefenders { get; private set; }

    private bool _gameStarted;
    private List<Creep> _creeps;

    private void Awake()
    {
        _creeps = new List<Creep>(); //Initialize the list
    }
    private void Start()
    {
        rangeSprite.transform.position = transform.position;
        rangeSprite.transform.localScale = new Vector3(attackRange * 5, attackRange * 5, attackRange * 5);
        rangeSprite.SetActive(false);
        TowerHasDefenders = towerHasDefenders; //Do not place this in awake. Or you might get a null reference
        _gameStarted = true;

        TowerUpgrade = GetComponent<scrUppgradeTowers>();    
    }
    private void Update()
    {
        GetCurrentCreepTarget();
        RotateTowardsTarget();

        if (CurrentCreepTarget != null)
        {
            if (CurrentCreepTarget._CreepHealth.CurrentHealth <= 0) 
            {
                _creeps.Remove(CurrentCreepTarget);
            }
        }
    }
    private void GetCurrentCreepTarget()
    {
        if (_creeps.Count <= 0)
        {
            CurrentCreepTarget = null;
            return; //No point in continuing with the logic of this function. Exit
        }
        CurrentCreepTarget = _creeps[0]; //IMPORTANT This is what makes us choose the first enemy in the list!
        float currentCreepDistance = _creeps[0].DistanceTravelled;
        for (int i = 1; i < _creeps.Count; i ++) //Starting by one, makes it so that this list wont return any if we only have one target
        {
            if (_creeps[i].DistanceTravelled > currentCreepDistance) //Checks if any of the creeps have a larget currentCreepDistance
            {
                //Debug.Log("current creep target has been passed");
                CurrentCreepTarget = _creeps[i]; //Set it as new target
                currentCreepDistance = _creeps[i].DistanceTravelled; //Update the currentCreepDistance
            }
        }
    }
    private void RotateTowardsTarget()
    {
        if(CurrentCreepTarget == null || TowerHasDefenders == true) //Stop defender towers from rotating
        {
            return; // Stop the code
        }

        Vector3 targetPos = CurrentCreepTarget.transform.position - transform.position;
        targetPos.y = 0; //This is where the magic happens. It disables the y rotation 
        var rotation = Quaternion.LookRotation(targetPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
    public void ToggleRangeSpriteOn(scrTowerTargeting _reference) //Turn the rangeSprite on if this is the tower clicked on
    {
        if(this != _reference)
        {
            return;
        }
        rangeSprite.SetActive(true);
    }
    public void ToggleRangeSpriteOff(scrTowerTargeting _reference) //Turn the rangeSprite off if this is not the tower clicked on
    {
        if (this != _reference)
        {
            rangeSprite.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other) //Add creeps to list
    {
        if (other.CompareTag("Creep")) //Filther for tags
        {
            Creep newCreep = other.GetComponent<Creep>(); //Get the new reference
            _creeps.Add(newCreep);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Creep"))
        {
            Creep creep = other.GetComponent<Creep>();
            if (_creeps.Contains(creep))//Checks if this SPESIFIC reference was already in the list (I think)
            {
                _creeps.Remove(creep); //Remove this creep from the list
            }
        }
    }
    private void OnEnable()
    {
        scrUIManager.OnTowerSelected += ToggleRangeSpriteOn;
        scrUIManager.OnTowerSelected += ToggleRangeSpriteOff;
    }
    private void OnDisable()
    {
        scrUIManager.OnTowerSelected -= ToggleRangeSpriteOn;
        scrUIManager.OnTowerSelected -= ToggleRangeSpriteOff;
    }
}