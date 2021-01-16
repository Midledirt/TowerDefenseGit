using UnityEngine;
/// <summary>
/// Some of this code might be a little bit confusing since I have never instantiated UI elements like this before. It is all explained 
/// in part 45 "Load Turrest" tho. Whilst I will certainly work on and edit this before my own release, I do think this is a good system
/// and it might make it into my final version of the game. Thus, its good to go back to that episode later and study it!
/// </summary>

[CreateAssetMenu(fileName = "Tower shop setting")] //Since a scriptable object is NOT placed on a GameObject, this 
//is needed to INSTANTIATE it in our folder.
//We are also defining its filename, though I don`t think this is necessary. This is covered on episode: 44 "Create Turrets Panel"
public class scrTowerSettings : ScriptableObject //This is necessary for this to work as a scriptable object
{
    public GameObject TowerPrefab;
    public int TowerShopCost;
    public Sprite TowerShopSprite;
}
