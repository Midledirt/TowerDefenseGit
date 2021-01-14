using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower shop setting")] //Since a scriptable object is NOT placed on a GameObject, this 
//is needed to INSTANTIATE it in our folder.
//We are also defining its filename, though I don`t think this is necessary. This is covered on episode: 44 "Create Turrets Panel"
public class scrTowerSettings : ScriptableObject //This is necessary for this to work as a scriptable object
{
    [SerializeField] private GameObject TowerPrefab;
    [SerializeField] private int TowerShopCost;
    [SerializeField] private Sprite TowerShopSprite;
}
