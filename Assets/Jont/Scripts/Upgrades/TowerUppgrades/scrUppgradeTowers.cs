using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// This upgrade system currently only works for the mage tower. It may be fixed later on in the tutorial (right now, its called through
/// a button press). If not, I may find a fix myself, or go back to when he made new towers like the "tankTower", and see if theres something
/// to take from that video.
/// </summary>

public class scrUppgradeTowers : MonoBehaviour
{
    [SerializeField] private int uppgradeCost;
    [SerializeField] private int upgradeCostIncremental;
    [SerializeField] private float damageIncremental;

    [Header("Sell")]
    [Range (0, 1)]
    [SerializeField] private float sellPercentage;

    public float SellPercentage { get; set; }
    public int UppgradeCost { get; set; }
    private scrTowerPrefabTracker towerLevelTracker;
    private scrTowerProjectileLoader _towerProjectileLoader;
    private ObjectPooler projectilePool;
    private int towerCurrentLevel;

    [SerializeField] private int path1cost;
    [SerializeField] private int path2cost;
    [SerializeField] private int path3cost;
    public int Path1Cost { get; private set; }
    public int Path2Cost { get; private set; }
    public int Path3Cost { get; private set; }

    private void Awake()
    {
        Path1Cost = path1cost;
        Path2Cost = path2cost;
        Path3Cost = path3cost;
        towerCurrentLevel = 1;
        projectilePool = GetComponent<ObjectPooler>(); //Gets the instance on this tower
    }

    private void Start()
    {
        _towerProjectileLoader = GetComponent<scrTowerProjectileLoader>(); //get the instance!
        towerLevelTracker = GetComponent<scrTowerPrefabTracker>(); //Get the instance!

        UppgradeCost = uppgradeCost;

        SellPercentage = sellPercentage;
    }
    public void AssignPathFromButtonPress(int path)
    {
        switch(path)
        {
            case 1:
                UppgradeCost = path1cost;
                return;
            case 2:
                UppgradeCost = path2cost;
                return;
            case 3:
                UppgradeCost = path3cost;
                return;
            default:
                Debug.LogError("The getPathFromButtonPress function in scrUppgradeTowers is assigned an invalid value");
                return;
        }
    }

    public void UpgradeTower()
    {
        //Path selection
        if(scrCurrencySystem.Instance.TotalCoins >= UppgradeCost && towerCurrentLevel < 4)
        {
            //_towerProjectileLoader.Damage += damageIncremental; //Adds to the damage, this is only how ill do it for this very first prototype
            towerCurrentLevel += 1; //Increases the level of the tower
            towerLevelTracker.UpgradeTowerWithLevel(towerCurrentLevel); //Set the level the tower and projectiles will be at
            projectilePool.TowerUpgraded(towerLevelTracker.TowerUpgradePath); //Update the projectile prefab
            _towerProjectileLoader.UpdateProjectileStats(towerLevelTracker.TowerUpgradePath, towerCurrentLevel); //Update the projectile stats
            UpdateUppgrade(); //Spends the coins, and increments the cost of upgrading
        }
        else if (scrCurrencySystem.Instance.TotalCoins < UppgradeCost)
        {
            Debug.Log("Not enough money");
        }
    }

    public int GetSellValue()
    {
        int sellValue = Mathf.RoundToInt(UppgradeCost * SellPercentage);
        return sellValue;
    }

    private void UpdateUppgrade()
    {
        //scrCurrencySystem.Instance.SpendCoins(UppgradeCost); //Spend the coins
        //UppgradeCost += upgradeCostIncremental; //Increase the cost for the next time
        int selectedTowerPath = towerLevelTracker.TowerUpgradePath;
        switch(selectedTowerPath)
        {
            case 1:
                UppgradeCost = (path1cost += upgradeCostIncremental);
                scrCurrencySystem.Instance.SpendCoins(UppgradeCost);
                return;
            case 2:
                UppgradeCost = (path2cost += upgradeCostIncremental);
                scrCurrencySystem.Instance.SpendCoins(UppgradeCost);
                return;
            case 3:
                UppgradeCost = (path3cost += upgradeCostIncremental);
                scrCurrencySystem.Instance.SpendCoins(UppgradeCost);
                return;
            default:
                UppgradeCost = path1cost;
                Debug.LogError("The selectedTowerPath local var in the UpdateUpgrade function in the scrUpgradeTowers script is assigned an incorrect value");
                return;
        }
    }
}
