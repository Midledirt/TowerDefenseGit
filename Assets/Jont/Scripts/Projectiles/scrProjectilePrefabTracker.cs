using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrProjectilePrefabTracker : MonoBehaviour
{
    [Header ("Initial prefab for gameobject")]
    [SerializeField] public GameObject initialPrefab;
    [Header("Prefabs for upgrade path 1")]
    [SerializeField] public GameObject prefabLevel2Version1;
    [SerializeField] public GameObject prefabLevel3Version1;
    [SerializeField] public GameObject pregabLevel4Version1;
    [Header("Prefabs for upgrade path 2")]
    [SerializeField] public GameObject prefabLevel2Version2;
    [SerializeField] public GameObject prefabLevel3Version2;
    [SerializeField] public GameObject prefabLevel4Version2;
    [Header("Prefabs for upgrade path 3")]
    [SerializeField] public GameObject prefabLevel2Version3;
    [SerializeField] public GameObject prefabLevel3Version3;
    [SerializeField] public GameObject prefabLevel4Version3;
    protected int defenderlevel;
    protected int projectileOrUnitMaxLevel;

    public int PrefabVersion { get; private set; }


    protected virtual void Awake()
    {
        PrefabVersion = 0;
        defenderlevel = 1;
        projectileOrUnitMaxLevel = 4;
        ResetProjectileLevel();
    }

    public virtual void ResetProjectileLevel()
    {
        initialPrefab.SetActive(true);
        prefabLevel2Version1.SetActive(false);
        prefabLevel3Version1.SetActive(false);
        pregabLevel4Version1.SetActive(false);
        prefabLevel2Version2.SetActive(false);
        prefabLevel3Version2.SetActive(false);
        prefabLevel4Version2.SetActive(false);
        prefabLevel2Version3.SetActive(false);
        prefabLevel3Version3.SetActive(false);
        prefabLevel4Version3.SetActive(false);
    }
    public virtual void SetProjectilePath(int _path)
    {
        PrefabVersion = _path;
    }
    public virtual void SetProjectileLevel(int _projectileLevel)
    {
        defenderlevel = _projectileLevel;
        if (defenderlevel <= projectileOrUnitMaxLevel && PrefabVersion == 1)
        {
            switch (defenderlevel)
            {
                case 1:
                    initialPrefab.gameObject.SetActive(true);
                    prefabLevel2Version1.gameObject.SetActive(false);
                    prefabLevel3Version1.gameObject.SetActive(false);
                    pregabLevel4Version1.gameObject.SetActive(false);
                    return;
                case 2:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version1.gameObject.SetActive(true);
                    prefabLevel3Version1.gameObject.SetActive(false);
                    pregabLevel4Version1.gameObject.SetActive(false);
                    return;
                case 3:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version1.gameObject.SetActive(false);
                    prefabLevel3Version1.gameObject.SetActive(true);
                    pregabLevel4Version1.gameObject.SetActive(false);
                    return;
                case 4:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version1.gameObject.SetActive(false);
                    prefabLevel3Version1.gameObject.SetActive(false);
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
                    prefabLevel2Version2.gameObject.SetActive(false);
                    prefabLevel3Version2.gameObject.SetActive(false);
                    prefabLevel4Version2.gameObject.SetActive(false);
                    return;
                case 2:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version2.gameObject.SetActive(true);
                    prefabLevel3Version2.gameObject.SetActive(false);
                    prefabLevel4Version2.gameObject.SetActive(false);
                    return;
                case 3:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version2.gameObject.SetActive(false);
                    prefabLevel3Version2.gameObject.SetActive(true);
                    prefabLevel4Version2.gameObject.SetActive(false);
                    return;
                case 4:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version2.gameObject.SetActive(false);
                    prefabLevel3Version2.gameObject.SetActive(false);
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
                    prefabLevel2Version3.gameObject.SetActive(false);
                    prefabLevel3Version3.gameObject.SetActive(false);
                    prefabLevel4Version3.gameObject.SetActive(false);
                    return;
                case 2:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version3.gameObject.SetActive(true);
                    prefabLevel3Version3.gameObject.SetActive(false);
                    prefabLevel4Version3.gameObject.SetActive(false);
                    return;
                case 3:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version3.gameObject.SetActive(false);
                    prefabLevel3Version3.gameObject.SetActive(true);
                    prefabLevel4Version3.gameObject.SetActive(false);
                    return;
                case 4:
                    initialPrefab.gameObject.SetActive(false);
                    prefabLevel2Version3.gameObject.SetActive(false);
                    prefabLevel3Version3.gameObject.SetActive(false);
                    prefabLevel4Version3.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The projectileLevel variable in scrTpwerProjectileLoader is set to an incorrect value");
                    return;
            }
        }
    }
}
