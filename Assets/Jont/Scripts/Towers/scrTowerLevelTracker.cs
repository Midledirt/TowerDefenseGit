using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTowerLevelTracker : MonoBehaviour
{
    [SerializeField] private GameObject towerLevel1;
    [SerializeField] private GameObject towerLevel2;
    [SerializeField] private GameObject towerLevel3;
    [SerializeField] private GameObject towerLevel4;
    [SerializeField] private int maxTowerLevel = 4;
    private int currentTowerLevel;

    public int CurrentTowerLevel { get; private set; }

    private void Awake()
    {
        currentTowerLevel = 1;
        CurrentTowerLevel = currentTowerLevel;
    }
    private void Start()
    {
        towerLevel1.SetActive(true);
        towerLevel2.SetActive(false);
        towerLevel3.SetActive(false);
        towerLevel4.SetActive(false);
    }
    public void UpgradeTowerWithLevel(int _currentLevel)
    {
        CurrentTowerLevel = _currentLevel; //Update the current level from other scripts
        if (CurrentTowerLevel < maxTowerLevel)
        {
            switch(CurrentTowerLevel) //Set the current prefab to active, others to unactive
            {
                case 1:
                    towerLevel1.gameObject.SetActive(true);
                    towerLevel2.gameObject.SetActive(false);
                    towerLevel3.gameObject.SetActive(false);
                    towerLevel4.gameObject.SetActive(false);
                    return;
                case 2:
                    towerLevel1.gameObject.SetActive(false);
                    towerLevel2.gameObject.SetActive(true);
                    towerLevel3.gameObject.SetActive(false);
                    towerLevel4.gameObject.SetActive(false);
                    return;
                case 3:
                    towerLevel1.gameObject.SetActive(false);
                    towerLevel2.gameObject.SetActive(false);
                    towerLevel3.gameObject.SetActive(true);
                    towerLevel4.gameObject.SetActive(false);
                    return;
                case 4:
                    towerLevel1.gameObject.SetActive(false);
                    towerLevel2.gameObject.SetActive(false);
                    towerLevel3.gameObject.SetActive(false);
                    towerLevel4.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The currentTowerLevel variable in scrTowerLevelTracker script is set to a value outside the scope of the switch statement");
                    return;

            }
        }       
    }
}
