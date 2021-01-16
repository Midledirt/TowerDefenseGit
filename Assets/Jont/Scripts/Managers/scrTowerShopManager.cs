using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Some of this code might be a little bit confusing since I have never instantiated UI elements like this before. It is all explained 
/// in part 45 "Load Turrest" tho. Whilst I will certainly work on and edit this before my own release, I do think this is a good system
/// and it might make it into my final version of the game. Thus, its good to go back to that episode later and study it!
/// </summary>
public class scrTowerShopManager : MonoBehaviour
{
    [Tooltip("There is a prefab of the button in Prefabs -> UI")]
    [SerializeField] private GameObject towerCardPrefab;
    [Tooltip("This should be the canvas panel that will serve as this buttons parent object")]
    [SerializeField] private Transform towerPanelContainer;

    [Header("Tower Settings")]
    [Tooltip("You will need to manualy drag and drop one of EACH main tower SCRIPTABLE OBJECT into this field")]
    [SerializeField] private scrTowerSettings[] towers; //An arracy of the class scrTowerSettings 

    private scrTowerNode _currentNodeSelected;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < towers.Length; i ++)
        {
            CreateTowerCard(towers[i]);
        }
    }
    
    //This will instantiate "buttons" or "cards" with the right UI elements as needed in runtime, If I understand this right
    private void CreateTowerCard(scrTowerSettings towerSettings) //We want to load each Tower in our scriptable object
    {
        GameObject newInstance = Instantiate(towerCardPrefab, towerPanelContainer.position, Quaternion.identity);
        newInstance.transform.SetParent(towerPanelContainer); //Parent the new instance
        newInstance.transform.localScale = Vector3.one; //To avoid potential errors since we are using a grid layout

        scrTowerUICard cardButton = newInstance.GetComponent<scrTowerUICard>();
        cardButton.SetupTurretButton(towerSettings);
    }

    private void NodeSelected(scrTowerNode nodeSelected)
    {
        _currentNodeSelected = nodeSelected;
    }

    private void PlaceTower(scrTowerSettings towerLoaded)
    {
        if(_currentNodeSelected != null)
        {
            GameObject towerInstance = Instantiate(towerLoaded.TowerPrefab);
            towerInstance.transform.localPosition = _currentNodeSelected.transform.position;
            towerInstance.transform.parent = _currentNodeSelected.transform;

            scrTower towerPlaced = towerInstance.GetComponent<scrTower>();
            _currentNodeSelected.SetTower(towerPlaced);
        }
    }

    private void TowerSold()
    {
        _currentNodeSelected = null;
    }

    private void OnEnable()
    {
        scrTowerNode.OnNodeSelected += NodeSelected;
        scrTowerNode.OnTowerSold += TowerSold;
        scrTowerUICard.OnPlaceTower += PlaceTower;
    }


    private void OnDisable()
    {
        scrTowerNode.OnNodeSelected -= NodeSelected;
        scrTowerNode.OnTowerSold -= TowerSold;
        scrTowerUICard.OnPlaceTower -= PlaceTower;
    }
}
