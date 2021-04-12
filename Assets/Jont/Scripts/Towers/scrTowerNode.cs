using UnityEngine;
using System; //Required for "Action"

public class scrTowerNode : MonoBehaviour
{
    public static Action<scrTowerNode> OnNodeSelected; //Once again, we are using an "Action". I NEED 2 read up on this. Requiers System. 
    public static Action OnTowerSold; //This needs no specific reference, unlike the Action above, because "we don`t care which node sold the
    //turret, we just want to lose the "_currentNodeSelected" reference." "_currentNodeSelected" will reference this script, from the 
    //"scrUIManager" class.
    public scrTowerRallypointPos TowerRallypointPos { get; set; }
    public scrTowerTargeting Tower { get; set; }
    public scrTowerPrefabTracker TowerLevelTracker { get; set; }
    //THE REASON WHY THIS SCRIPT HOLDS SO MANY REFERENCES TO OTHER SCRIPTS is that du to the public action abowe, this script can be used to get specific
    //references to the different instances of scripts on the tower

    public void WhenClicked() //This fires when the mouse has clicked on this object
    {
        SelectTower(); //Runs the select tower ACTION
    }
    public void SetRallyPointReference(scrTowerRallypointPos _rallypointInstance)
    {
        TowerRallypointPos = _rallypointInstance;
    }
    public void SetTowerLevelTracker(scrTowerPrefabTracker _towerLevelTracker)
    {
        TowerLevelTracker = _towerLevelTracker;
    }

    public void SetTower(scrTowerTargeting tower) //This is called by the scrTowerShopManager in the PlaceTower() event
    {
        Tower = tower;
    }

    public bool NodeIsEmpty() //Checks if there is a tower at the node
    {
        return Tower == null;
    }

    public void SelectTower()
    {
        OnNodeSelected?.Invoke(this); //This will be a reference to this script (instance?). Meaning that THIS is what wants to open the tower
        //Shop panel Confusing? Its covered in episode 46 "Place Turrets"
    }

    public void SellTower()
    {
        if (!NodeIsEmpty())
        {
            scrCurrencySystem.Instance.AddCoins(Tower.TowerUpgrade.GetSellValue()); //Get money back
            //Destroy defenders
            Tower.TryGetComponent<scrDefenderSpawner>(out scrDefenderSpawner _localSpawner);
            if(_localSpawner != null)
            {
                _localSpawner.DeleteDefendersOnTowerSold();
            }
            Destroy(Tower.gameObject); //Destroy the tower gameobject 
            Tower = null; //Loose the REFERENCE (for this class), so that a new tower can be built here
            OnTowerSold?.Invoke();
        }
    }
}
