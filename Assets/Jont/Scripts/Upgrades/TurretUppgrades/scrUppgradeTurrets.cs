﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This upgrade system currently only works for the mage tower. It may be fixed later on in the tutorial (right now, its called through
/// a button press). If not, I may find a fix myself, or go back to when he made new towers like the "tankTower", and see if theres something
/// to take from that video.
/// </summary>

public class scrUppgradeTurrets : MonoBehaviour
{
    [SerializeField] private int uppgradeCost;
    [SerializeField] private int upgradeCostIncremental;
    [SerializeField] private float damageIncremental;

    private scrTowerMageProjectileLoader _towerProjectileLoader;
    //private scrTowerArrowsProjectileLoader _towerArrowsProjectileLoader; //Adding this does not fix the no damage bug. And it causes a null
    //reference for the mage tower...

    public int UppgradeCost { get; set; }

    private void Start()
    {
        _towerProjectileLoader = GetComponent<scrTowerMageProjectileLoader>(); //Gets the reference
        //_towerArrowsProjectileLoader = GetComponent <scrTowerArrowsProjectileLoader>(); //Gets the reference

        UppgradeCost = uppgradeCost;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U)) //For running test
        {
            UpgradeTurret();
        }
    }

    private void UpgradeTurret()
    {
        if (scrCurrencySystem.Instance.TotalCoins >= UppgradeCost) //Check that we have enough coins. scrCurrenctySystem is a Singleton btw
            //So we do not need a reference (I think)
        {
            _towerProjectileLoader.Damage += damageIncremental; //Adds to the damage, this is only how ill do it for this very first prototype
            //_towerArrowsProjectileLoader.Damage += damageIncremental; //Adds to the damage, this is only how ill do it for this very first prototype
            UpdateUppgrade(); //Spends the coins, and increments the cost of upgrading
        }
    }

    private void UpdateUppgrade()
    {
        scrCurrencySystem.Instance.SpendCoins(UppgradeCost); //Spend the coins
        UppgradeCost += upgradeCostIncremental; //Increase the cost for the next time
    }
}
