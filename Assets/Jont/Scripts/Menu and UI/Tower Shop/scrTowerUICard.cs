using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// Some of this code might be a little bit confusing since I have never instantiated UI elements like this before. It is all explained 
/// in part 45 "Load Turrest" tho. Whilst I will certainly work on and edit this before my own release, I do think this is a good system
/// and it might make it into my final version of the game. Thus, its good to go back to that episode later and study it!
/// </summary>

public class scrTowerUICard : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI towerCost;

    public void SetupTurretButton(scrTowerSettings towerSettings)
    {
        towerImage.sprite = towerSettings.TowerShopSprite; //Get the image reference from the scriptable object
        towerCost.text = towerSettings.TowerShopCost.ToString(); //Turns an integr into text (as you should know)
    }
}
