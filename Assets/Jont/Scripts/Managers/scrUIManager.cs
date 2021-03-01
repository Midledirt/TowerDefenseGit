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
    [SerializeField] private GameObject TowerUpgradePanel; //My own addition
    [SerializeField] private GameObject TowerSpecializationUpgradesPanel; //My own addition

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI upgradeTextForPathSelectionPanelPath1;
    [SerializeField] private TextMeshProUGUI upgradeTextForPathSelectionPanelPath2;
    [SerializeField] private TextMeshProUGUI upgradeTextForPathSelectionPanelPath3;
    [SerializeField] private TextMeshProUGUI upgradeTextForUpgradePanel;
    [SerializeField] private TextMeshProUGUI upgradeTextForSpesializationSelectionPanelSpez1;
    [SerializeField] private TextMeshProUGUI upgradeTextForSpesializationSelectionPanelSpez2;
    [SerializeField] private TextMeshProUGUI upgradeTextForSpesializationSelectionPanelSpez3;
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
        RallyPointButton.SetActive(false); //Opens the rally point button, ONLY if the tower has defenders
    }

    public void CloseNodeUIPanel()
    {
        //This is where the tutorial runs the "scrTowerNode" CloseAttackRangeSprite(). //IMPORTANT: DOES NOT WORK RIGHT NOW
        nodeUIPanel.SetActive(false);
        RallyPointButton.SetActive(false);
        CloseOpenPanels(); //Close open panels
    }

    public void OpenUpgradeMenu()
    {
        CloseOpenPanels(); //Close open panels
        if (_currentNodeSelected.TowerLevelTracker.CurrentTowerLevel == 1) //Checks if the tower is at level one
        {
            TowerPathSelectionPanel.SetActive(true);
            UpgradeMenuOpened();
        }
        if(_currentNodeSelected.TowerLevelTracker.CurrentTowerLevel !=1 && _currentNodeSelected.TowerLevelTracker.CurrentTowerLevel < 4)
        {
            TowerUpgradePanel.SetActive(true);
            UpgradeMenuOpened();
        }
        else if(_currentNodeSelected.TowerLevelTracker.CurrentTowerLevel == 4)
        {
            TowerSpecializationUpgradesPanel.SetActive(true);
            UpgradeMenuOpened();
        }
    }
    private void CloseOpenPanels()
    {
        TowerPathSelectionPanel.SetActive(false);
        TowerUpgradePanel.SetActive(false);
        TowerSpecializationUpgradesPanel.SetActive(false);
    }
    private void UpgradeMenuOpened()
    {
        UpdateUpgradeText();
        UpdateSellValue();
    }

    public void SellTower()
    {
        _currentNodeSelected.SellTower(); //Sell the tower
        _currentNodeSelected = null; //Loose the reference
        nodeUIPanel.SetActive(false); //Close the UI Panel after we sell the tower
    }

    private void ShouWUpgradePanel() //Function that shows the UIPanel when run
    {
        nodeUIPanel.SetActive(true); //Senere vil jeg ha en animasjon som får det til å ploppe opp og ned
        UpdateUpgradeText();
        UpdateSellValue();
        if (_currentNodeSelected != null && _currentNodeSelected.Tower.TowerHasDefenders == true)
        {
            RallyPointButton.SetActive(true); //Make this dependent on a bool that cheks if the tower has "defenders"
        }
    }

    private void UpdateUpgradeText()
    {
        upgradeTextForPathSelectionPanelPath1.text = _currentNodeSelected.Tower.TowerUpgrade.Path1Cost.ToString(); //So this references the "currentNodeSelected", 
        //because we clicked on it. Then it accesses the scrTowerTargeting class connected to the tower on that node. It then cheks for that classes reference
        //to the "TowerUpgrade" variable which references the "scrUppgradeTurret" script, where we find the uppgradecost. A long snake of references.
        upgradeTextForPathSelectionPanelPath2.text = _currentNodeSelected.Tower.TowerUpgrade.Path2Cost.ToString();
        upgradeTextForPathSelectionPanelPath3.text = _currentNodeSelected.Tower.TowerUpgrade.Path3Cost.ToString();
        upgradeTextForUpgradePanel.text = _currentNodeSelected.Tower.TowerUpgrade.UppgradeCost.ToString();
        upgradeTextForSpesializationSelectionPanelSpez1.text = _currentNodeSelected.Tower.TowerUpgrade.UppgradeCost.ToString();
        upgradeTextForSpesializationSelectionPanelSpez2.text = _currentNodeSelected.Tower.TowerUpgrade.UppgradeCost.ToString();
        upgradeTextForSpesializationSelectionPanelSpez3.text = _currentNodeSelected.Tower.TowerUpgrade.UppgradeCost.ToString();
    }

    private void UpdateSellValue()
    {
        int sellAmount = _currentNodeSelected.Tower.TowerUpgrade.GetSellValue(); //Get the value
        sellText.text = sellAmount.ToString(); //Show the value in the text
    }

    private void NodeSelected(scrTowerNode nodeSelected) //gets the reference
    {
        _currentNodeSelected = nodeSelected; //If the node is empty, show the tower shop UI
        if (_currentNodeSelected.NodeIsEmpty())
        {
            towerShopPanel.SetActive(true);
        }
        else
        {
            ShouWUpgradePanel(); //If there is a tower on the node, show the NodeUIpanel
        }
    }
    public void SelectPath1()
    {
        if (scrCurrencySystem.Instance.TotalCoins > _currentNodeSelected.Tower.TowerUpgrade.Path1Cost)
        {
            _currentNodeSelected.Tower.TowerUpgrade.AssignPathFromButtonPress(1);
            PathSelected(1);
        }
    }
    public void SelectPath2()
    {
        if (scrCurrencySystem.Instance.TotalCoins > _currentNodeSelected.Tower.TowerUpgrade.Path2Cost)
        {
            _currentNodeSelected.Tower.TowerUpgrade.AssignPathFromButtonPress(1);
            PathSelected(2);
        }
    }
    public void SelectPath3()
    {
        if (scrCurrencySystem.Instance.TotalCoins > _currentNodeSelected.Tower.TowerUpgrade.Path3Cost)
        {
            _currentNodeSelected.Tower.TowerUpgrade.AssignPathFromButtonPress(1);
            PathSelected(3);
        }
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
    public void UpgradeTower()
    {
        _currentNodeSelected.Tower.TowerUpgrade.UpgradeTower(); //This is how we comunicate with the tower from this script
        UpdateUpgradeText();
        UpdateSellValue();
        TowerUpgradePanel.SetActive(false);
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
