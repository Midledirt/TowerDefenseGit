﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scrUIManager : Singleton<scrUIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject towerShopPanel;    
    [SerializeField] private GameObject nodeUIPanel;

    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI sellText;

    private scrTowerNode _currentNodeSelected;

    public void CloseTowerShopPanel()
    {
        towerShopPanel.SetActive(false);
    }

    public void UpgradeTurret()
    {
        _currentNodeSelected.Tower.TowerUpgrade.UpgradeTurret();
        UpdateUpgradeText();
        UpdateSellValue();
    }

    public void SellTower()
    {
        _currentNodeSelected.SellTower(); //Sell the tower
        _currentNodeSelected = null; //Loose the reference
        nodeUIPanel.SetActive(false); //Close the UI Panel after we sell the tower
    }

    private void ShouNodeUI() //Function that shows the UIPanel when run
    {
        nodeUIPanel.SetActive(true);
        UpdateUpgradeText();
        UpdateSellValue();
    }

    private void UpdateUpgradeText()
    {
        upgradeText.text = _currentNodeSelected.Tower.TowerUpgrade.UppgradeCost.ToString(); //So this references the "currentNodeSelected", 
        //because we clicked on it. Then it accesses the scrTower class connected to the tower on that node. It then cheks for that classes reference
        //to the "TowerUpgrade" variable which references the "scrUppgradeTurret" script, where we find the uppgradecost. A long snake of references.
    }

    private void UpdateSellValue()
    {
        int sellAmount = _currentNodeSelected.Tower.TowerUpgrade.GetSellValue(); //Get the value
        sellText.text = sellAmount.ToString(); //Show the value in the text
    }

    private void NodeSelected(scrTowerNode nodeSelected)
    {
        _currentNodeSelected = nodeSelected; //If the node is empty, show the tower shop UI
        if (_currentNodeSelected.NodeIsEmpty())
        {
            towerShopPanel.SetActive(true);
        }
        else
        {
            ShouNodeUI(); //If there is a tower on the node, show the NodeUIpanel
        }
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
