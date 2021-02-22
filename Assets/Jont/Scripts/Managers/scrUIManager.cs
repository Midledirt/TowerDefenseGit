using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scrUIManager : Singleton<scrUIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject towerShopPanel;    
    [SerializeField] private GameObject nodeUIPanel;
    [SerializeField] private GameObject RallyPointButton; //My own addition
    [SerializeField] private GameObject TowerPathSelectionPanel; //My own addition

    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI currentWaveText;

    private scrTowerNode _currentNodeSelected;

    private void Update() //Make events out of these for the sake of optimization?
    {
        totalCoinsText.text = scrCurrencySystem.Instance.TotalCoins.ToString();
        livesText.text = scrLevelManager.Instance.TotalLives.ToString();
        currentWaveText.text = $"Wave { scrLevelManager.Instance.CurrentWaveForUI}";
    }

    public void CloseTowerShopPanel()
    {
        towerShopPanel.SetActive(false);
        RallyPointButton.SetActive(false);
    }

    public void CloseNodeUIPanel()
    {
        //This is where the tutorial runs the "scrTowerNode" CloseAttackRangeSprite(). //IMPORTANT: DOES NOT WORK RIGHT NOW
        nodeUIPanel.SetActive(false);
        RallyPointButton.SetActive(false);
    }

    public void UpgradeTurret()
    {
        if (_currentNodeSelected.TowerLevelTracker.CurrentTowerLevel == 1) //Checks if the tower is at level one
        {
            TowerPathSelectionPanel.SetActive(true);
        }
        else if(_currentNodeSelected.TowerLevelTracker.CurrentTowerLevel !=1)
        {
            _currentNodeSelected.Tower.TowerUpgrade.UpgradeTower(); //This is how we comunicate with the tower from this script
            UpdateUpgradeText();
            UpdateSellValue();
        }
    }

    public void SellTower()
    {
        _currentNodeSelected.SellTower(); //Sell the tower
        _currentNodeSelected = null; //Loose the reference
        nodeUIPanel.SetActive(false); //Close the UI Panel after we sell the tower
    }

    private void ShouUpgradePanel() //Function that shows the UIPanel when run
    {
        nodeUIPanel.SetActive(true); //Senere vil jeg ha en animasjon som får det til å ploppe opp og ned
        UpdateUpgradeText();
        UpdateSellValue();
        RallyPointButton.SetActive(true); //Make this dependent on a bool that cheks if the tower has "defenders"
    }

    private void UpdateUpgradeText()
    {
        upgradeText.text = _currentNodeSelected.Tower.TowerUpgrade.UppgradeCost.ToString(); //So this references the "currentNodeSelected", 
        //because we clicked on it. Then it accesses the scrTowerTargeting class connected to the tower on that node. It then cheks for that classes reference
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
            ShouUpgradePanel(); //If there is a tower on the node, show the NodeUIpanel
        }
    }
    public void SelectPath1()
    {
        PathSelected(1);
    }
    public void SelectPath2()
    {
        PathSelected(2);
    }
    public void SelectPath3()
    {
        PathSelected(3);
    }
    public void PathSelected(int _path)
    {
        _currentNodeSelected.TowerLevelTracker.TowerUpgradePath = _path; //Sets the path for the tower to _path
        _currentNodeSelected.Tower.TowerUpgrade.UpgradeTower(); //This is how we comunicate with the tower from this script
        UpdateUpgradeText();
        UpdateSellValue();
        TowerPathSelectionPanel.SetActive(false);
        Debug.Log("Tower Path set to: " + _currentNodeSelected.TowerLevelTracker.TowerUpgradePath);
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
