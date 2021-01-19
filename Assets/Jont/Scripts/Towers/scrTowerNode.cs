using UnityEngine;
using System; //Required for "Action"
/// <summary>
/// In the tutorial, a button is used to activate this script. That does not work for me, as I am making a 3D game, and I cannot attach a button
/// to the node. However, I can get the mouse position, and make this node clickable. 
/// </summary>
public class scrTowerNode : MonoBehaviour
{
    public static Action<scrTowerNode> OnNodeSelected; //Once again, we are using an "Action". I NEED 2 read up on this. Requires System. 
    public static Action OnTowerSold; //This needs no specific reference, unlike the Action above, because "we don`t care which node sold the
    //turret, we just want to lose the "_currentNodeSelected" reference." "_currentNodeSelected" will reference this script, from the 
    //"scrUIManager" class.

    //[SerializeField] GameObject attackRangeSprite; //IMPORTANT: DOES NOT WORK RIGHT NOW

    public scrTower Tower { get; set; }

    // private float _rangeSpriteSize; //IMPORTANT: DOES NOT WORK RIGHT NOW
    // private Vector3 _rangeSpriteOriginalSize; //IMPORTANT: DOES NOT WORK RIGHT NOW

    /*private void Start() //IMPORTANT: DOES NOT WORK RIGHT NOW
    {
        _rangeSpriteSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
        _rangeSpriteOriginalSize = attackRangeSprite.transform.localScale;
    }*/

    public void WhenClicked() //This fires when the mouse has clicked on this object
    {
        print("I am clicked, I am: " + this.gameObject.name); //For testing, tested to work fine!
        SelectTower(); //Runs the select tower ACTION
    }

    public void SetTower(scrTower tower) //This is called by the scrTowerShopManager in the PlaceTower() event
    {
        Tower = tower;
    }

    public bool NodeIsEmpty() //Checks if there is a tower at the node
    {
        return Tower == null;
    }

    /*public void CloseAttackRangeSprite() //IMPORTANT: DOES NOT WORK RIGHT NOW
    {
        attackRangeSprite.SetActive(false);
    }*/

    public void SelectTower()
    {
        OnNodeSelected?.Invoke(this); //This will be a reference to this script (instance?). Meaning that THIS is what wants to open the tower
        //Shop panel Confusing? Its covered in episode 46 "Place Turrets"
        if (!NodeIsEmpty())
        {
            //ShowTowerInfo(); //Displays the attack range of the tower, if there is a tower placed here //IMPORTANT: DOES NOT WORK RIGHT NOW
        }
    }

    public void SellTower()
    {
        if (!NodeIsEmpty())
        {
            scrCurrencySystem.Instance.AddCoins(Tower.TowerUpgrade.GetSellValue()); //Get money back
            Destroy(Tower.gameObject); //Destroy the tower gameobject
            Tower = null; //Loose the REFERENCE (for this class), so that a new tower can be built here
            OnTowerSold?.Invoke();
        }
    }

    /*private void ShowTowerInfo() //IMPORTANT: DOES NOT WORK RIGHT NOW
    {
        attackRangeSprite.SetActive(true);
        attackRangeSprite.transform.localScale = _rangeSpriteOriginalSize * Tower.AttackRange / (_rangeSpriteSize / 2);
    }*/
}
