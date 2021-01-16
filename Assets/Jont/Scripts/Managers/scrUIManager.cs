using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrUIManager : Singleton<scrUIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject towerShopPanel;

    private scrTowerNode _currentNodeSelected;

    public void CloseTowerShopPanel()
    {
        towerShopPanel.SetActive(false);
    }

    private void NodeSelected(scrTowerNode nodeSelected)
    {
        _currentNodeSelected = nodeSelected;
        if (_currentNodeSelected.NodeIsEmpty())
        {
            towerShopPanel.SetActive(true);
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
