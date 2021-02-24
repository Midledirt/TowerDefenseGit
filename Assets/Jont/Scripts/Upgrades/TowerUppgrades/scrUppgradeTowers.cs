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

    private void Awake()
    {
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

    public void UpgradeTower()
    {
        if (scrCurrencySystem.Instance.TotalCoins >= UppgradeCost && towerCurrentLevel < 4) //Check that we have enough coins. scrCurrenctySystem is a Singleton btw
            //So we do not need a reference (I think)
        {

            //_towerProjectileLoader.Damage += damageIncremental; //Adds to the damage, this is only how ill do it for this very first prototype
            towerCurrentLevel += 1; //Increases the level of the tower
            towerLevelTracker.UpgradeTowerWithLevel(towerCurrentLevel); //Set the level the tower and projectiles will be at
            projectilePool.TowerUpgraded(towerLevelTracker.TowerUpgradePath); //Update the projectile prefab
            _towerProjectileLoader.UpdateProjectileStats(towerLevelTracker.TowerUpgradePath, towerCurrentLevel); //Update the projectile stats
            UpdateUppgrade(); //Spends the coins, and increments the cost of upgrading
        }
    }

    public int GetSellValue()
    {
        int sellValue = Mathf.RoundToInt(UppgradeCost * SellPercentage);
        return sellValue;
    }

    private void UpdateUppgrade()
    {
        scrCurrencySystem.Instance.SpendCoins(UppgradeCost); //Spend the coins
        UppgradeCost += upgradeCostIncremental; //Increase the cost for the next time
    }
}
