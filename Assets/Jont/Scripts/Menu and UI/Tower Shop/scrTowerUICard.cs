using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
/// <summary>
/// Some of this code might be a little bit confusing since I have never instantiated UI elements like this before. It is all explained 
/// in part 45 "Load Turrest" tho. Whilst I will certainly work on and edit this before my own release, I do think this is a good system
/// and it might make it into my final version of the game. Thus, its good to go back to that episode later and study it!
/// </summary>

public class scrTowerUICard : MonoBehaviour
{
    public static Action<scrTowerSettings> OnPlaceTower;
    scrUIManager Uimanager;

    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI towerCost;

    public scrTowerSettings TowerLoaded { get; set; }

    private void Awake()
    {
        Uimanager = scrUIManager.Instance;
    }
    public void SetupTurretButton(scrTowerSettings towerSettings)
    {
        TowerLoaded = towerSettings;
        towerImage.sprite = towerSettings.TowerShopSprite; //Get the image reference from the scriptable object
        towerCost.text = towerSettings.TowerShopCost.ToString(); //Turns an integr into text (as you should know)
    }

    public void PlaceTower() //This function is called by the Button found on the "TowerButton" object
    {
        if (scrCurrencySystem.Instance.TotalCoins >= TowerLoaded.TowerShopCost)
        {
            scrCurrencySystem.Instance.SpendCoins(TowerLoaded.TowerShopCost);
            Uimanager.CloseTowerShopPanel(); //scrUIManager was made into a singleton so that we could do this.
            OnPlaceTower?.Invoke(TowerLoaded);
        }
    }
}
