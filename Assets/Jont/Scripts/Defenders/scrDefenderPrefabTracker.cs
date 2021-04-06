using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDefenderPrefabTracker : scrProjectilePrefabTracker
{
    public override void SetProjectileLevel(int _defenderLevel)
    {
        defenderlevel = _defenderLevel; 
        if (defenderlevel <= projectileOrUnitMaxLevel && PrefabVersion == 1)
        {
            switch (defenderlevel)
            {
                case 1:
                    initialPrefab.gameObject.SetActive(true);
                    return;
                case 2:
                    prefabLevel2Version1.gameObject.SetActive(true);
                    return;
                case 3:
                    prefabLevel3Version1.gameObject.SetActive(true);
                    return;
                case 4:
                    pregabLevel4Version1.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The projectileLevel variable in scrTpwerProjectileLoader is set to an incorrect value");
                    return;
            }
        }if (defenderlevel <= projectileOrUnitMaxLevel && PrefabVersion == 2)
        {
            switch (defenderlevel)
            {

                case 1:
                    initialPrefab.gameObject.SetActive(true);
                    return;
                case 2:
                    prefabLevel2Version2.gameObject.SetActive(true);
                    return;
                case 3:
                    prefabLevel3Version2.gameObject.SetActive(true);
                    return;
                case 4:
                    prefabLevel4Version2.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The projectileLevel variable in scrTpwerProjectileLoader is set to an incorrect value");
                    return;
            }
        }if (defenderlevel <= projectileOrUnitMaxLevel && PrefabVersion == 3)
        {
            switch (defenderlevel)
            {

                case 1:
                    initialPrefab.gameObject.SetActive(true);
                    return;
                case 2:
                    prefabLevel2Version3.gameObject.SetActive(true);
                    return;
                case 3:
                    prefabLevel3Version3.gameObject.SetActive(true);
                    return;
                case 4:
                    prefabLevel4Version3.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The projectileLevel variable in scrTpwerProjectileLoader is set to an incorrect value");
                    return;
            }
        }
    }
}
