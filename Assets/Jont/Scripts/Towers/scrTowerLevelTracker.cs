using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTowerLevelTracker : MonoBehaviour
{
    [Header("The initial prefab of the tower")]
    [SerializeField] private GameObject InitialTowerPrefab;
    [Header("Tower version 1 prefabs")]
    [SerializeField] private GameObject towerLevel2Version1;
    [SerializeField] private GameObject towerLevel3Version1;
    [SerializeField] private GameObject towerLevel4Version1;
    [Header("Tower version 2 prefabs")]
    [SerializeField] private GameObject towerLevel2Version2;
    [SerializeField] private GameObject towerLevel3Version2;
    [SerializeField] private GameObject towerLevel4Version2;
    [Header("Tower version 3 prefabs")]
    [SerializeField] private GameObject towerLevel2Version3;
    [SerializeField] private GameObject towerLevel3Version3;
    [SerializeField] private GameObject towerLevel4Version3;
    private int maxTowerLevel = 4;
    private int currentTowerLevel;
    public int TowerUpgradePath { get; set; } //Tested that this number works
    public int CurrentTowerLevel { get; private set; }

    private void Awake()
    {
        TowerUpgradePath = 0;
        currentTowerLevel = 1;
        CurrentTowerLevel = currentTowerLevel;
    }
    private void Start()
    {
        InitialTowerPrefab.SetActive(true);
        towerLevel2Version1.SetActive(false);
        towerLevel3Version1.SetActive(false);
        towerLevel4Version1.SetActive(false);
        towerLevel2Version2.SetActive(false);
        towerLevel3Version2.SetActive(false);
        towerLevel4Version2.SetActive(false);
        towerLevel2Version3.SetActive(false);
        towerLevel3Version3.SetActive(false);
        towerLevel4Version3.SetActive(false);
    }
    public void UpgradeTowerWithLevel(int _currentLevel)
    {
        CurrentTowerLevel = _currentLevel; //Update the current level from other scripts
        if (CurrentTowerLevel > 1 && CurrentTowerLevel < maxTowerLevel && TowerUpgradePath == 1)
        {
            switch(CurrentTowerLevel) //Set the current prefab to active, others to unactive
            {
                case 2:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version1.gameObject.SetActive(true);
                    towerLevel3Version1.gameObject.SetActive(false);
                    towerLevel4Version1.gameObject.SetActive(false);
                    return;
                case 3:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version1.gameObject.SetActive(false);
                    towerLevel3Version1.gameObject.SetActive(true);
                    towerLevel4Version1.gameObject.SetActive(false);
                    return;
                case 4:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version1.gameObject.SetActive(false);
                    towerLevel3Version1.gameObject.SetActive(false);
                    towerLevel4Version1.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The currentTowerLevel variable in scrTowerLevelTracker script is set to a value outside the scope of the switch statement");
                    return;

            }
        }
        if (CurrentTowerLevel > 1 && CurrentTowerLevel < maxTowerLevel && TowerUpgradePath == 2)
        {
            switch (CurrentTowerLevel) //Set the current prefab to active, others to unactive
            {
                case 2:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version2.gameObject.SetActive(true);
                    towerLevel3Version2.gameObject.SetActive(false);
                    towerLevel4Version2.gameObject.SetActive(false);
                    return;
                case 3:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version2.gameObject.SetActive(false);
                    towerLevel3Version2.gameObject.SetActive(true);
                    towerLevel4Version2.gameObject.SetActive(false);
                    return;
                case 4:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version2.gameObject.SetActive(false);
                    towerLevel3Version2.gameObject.SetActive(false);
                    towerLevel4Version2.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The currentTowerLevel variable in scrTowerLevelTracker script is set to a value outside the scope of the switch statement");
                    return;

            }
        }
        if (CurrentTowerLevel > 1 && CurrentTowerLevel < maxTowerLevel && TowerUpgradePath == 3)
        {
            switch (CurrentTowerLevel) //Set the current prefab to active, others to unactive
            {
                case 2:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version3.gameObject.SetActive(true);
                    towerLevel3Version3.gameObject.SetActive(false);
                    towerLevel4Version3.gameObject.SetActive(false);
                    return;
                case 3:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version3.gameObject.SetActive(false);
                    towerLevel3Version3.gameObject.SetActive(true);
                    towerLevel4Version3.gameObject.SetActive(false);
                    return;
                case 4:
                    InitialTowerPrefab.gameObject.SetActive(false);
                    towerLevel2Version3.gameObject.SetActive(false);
                    towerLevel3Version3.gameObject.SetActive(false);
                    towerLevel4Version3.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The currentTowerLevel variable in scrTowerLevelTracker script is set to a value outside the scope of the switch statement");
                    return;

            }
        }
    }
}
