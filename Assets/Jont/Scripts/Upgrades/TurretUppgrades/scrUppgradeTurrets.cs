using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrUppgradeTurrets : MonoBehaviour
{
    [SerializeField] private int uppgradeCost;
    [SerializeField] private int upgradeCostIncremental;
    [SerializeField] private float damageIncremental;

    private scrTowerMageProjectileLoader _turretProjectileLoader;

    public int UppgradeCost { get; set; }

    private void Start()
    {
        _turretProjectileLoader = GetComponent<scrTowerMageProjectileLoader>(); //Gets the reference

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
            _turretProjectileLoader.Damage += damageIncremental; //Adds to the damage, this is only how ill do it for this very first prototype
            UpdateUppgrade(); //Spends the coins, and increments the cost of upgrading
        }
    }

    private void UpdateUppgrade()
    {
        scrCurrencySystem.Instance.SpendCoins(UppgradeCost); //Spend the coins
        UppgradeCost += upgradeCostIncremental; //Increase the cost for the next time
    }
}
