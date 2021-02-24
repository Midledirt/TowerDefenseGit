using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrProjectilePrefabTracker : MonoBehaviour
{
    [Header ("Initial prefab for projectile")]
    [SerializeField] public GameObject projectileInitialPrefab;
    [Header("Prefabs for upgrade path 1")]
    [SerializeField] public GameObject projectileLevel2Version1;
    [SerializeField] public GameObject projectileLevel3Version1;
    [SerializeField] public GameObject projectileLevel4Version1;
    [Header("Prefabs for upgrade path 2")]
    [SerializeField] public GameObject projectileLevel2Version2;
    [SerializeField] public GameObject projectileLevel3Version2;
    [SerializeField] public GameObject projectileLevel4Version2;
    [Header("Prefabs for upgrade path 3")]
    [SerializeField] public GameObject projectileLevel2Version3;
    [SerializeField] public GameObject projectileLevel3Version3;
    [SerializeField] public GameObject projectileLevel4Version3;
    private int projectileLevel;
    private int projectileMaxLevel;

    public int ProjectileVersion { get; private set; }


    private void Awake()
    {
        ProjectileVersion = 0;
        projectileLevel = 1;
        projectileMaxLevel = 4;
        ResetProjectileLevel();
    }

    public void ResetProjectileLevel()
    {
        projectileInitialPrefab.SetActive(true);
        projectileLevel2Version1.SetActive(false);
        projectileLevel3Version1.SetActive(false);
        projectileLevel4Version1.SetActive(false);
        projectileLevel2Version2.SetActive(false);
        projectileLevel3Version2.SetActive(false);
        projectileLevel4Version2.SetActive(false);
        projectileLevel2Version3.SetActive(false);
        projectileLevel3Version3.SetActive(false);
        projectileLevel4Version3.SetActive(false);
    }
    public void SetProjectilePath(int _path)
    {
        ProjectileVersion = _path;
    }
    public void SetProjectileLevel(int _projectileLevel)
    {
        projectileLevel = _projectileLevel;
        if (projectileLevel <= projectileMaxLevel && ProjectileVersion == 1)
        {
            switch (projectileLevel)
            {
                case 1:
                    projectileInitialPrefab.gameObject.SetActive(true);
                    projectileLevel2Version1.gameObject.SetActive(false);
                    projectileLevel3Version1.gameObject.SetActive(false);
                    projectileLevel4Version1.gameObject.SetActive(false);
                    return;
                case 2:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version1.gameObject.SetActive(true);
                    projectileLevel3Version1.gameObject.SetActive(false);
                    projectileLevel4Version1.gameObject.SetActive(false);
                    return;
                case 3:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version1.gameObject.SetActive(false);
                    projectileLevel3Version1.gameObject.SetActive(true);
                    projectileLevel4Version1.gameObject.SetActive(false);
                    return;
                case 4:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version1.gameObject.SetActive(false);
                    projectileLevel3Version1.gameObject.SetActive(false);
                    projectileLevel4Version1.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The projectileLevel variable in scrTpwerProjectileLoader is set to an incorrect value");
                    return;
            }
        }if (projectileLevel <= projectileMaxLevel && ProjectileVersion == 2)
        {
            switch (projectileLevel)
            {

                case 1:
                    projectileInitialPrefab.gameObject.SetActive(true);
                    projectileLevel2Version2.gameObject.SetActive(false);
                    projectileLevel3Version2.gameObject.SetActive(false);
                    projectileLevel4Version2.gameObject.SetActive(false);
                    return;
                case 2:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version2.gameObject.SetActive(true);
                    projectileLevel3Version2.gameObject.SetActive(false);
                    projectileLevel4Version2.gameObject.SetActive(false);
                    return;
                case 3:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version2.gameObject.SetActive(false);
                    projectileLevel3Version2.gameObject.SetActive(true);
                    projectileLevel4Version2.gameObject.SetActive(false);
                    return;
                case 4:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version2.gameObject.SetActive(false);
                    projectileLevel3Version2.gameObject.SetActive(false);
                    projectileLevel4Version2.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The projectileLevel variable in scrTpwerProjectileLoader is set to an incorrect value");
                    return;
            }
        }if (projectileLevel <= projectileMaxLevel && ProjectileVersion == 3)
        {
            switch (projectileLevel)
            {

                case 1:
                    projectileInitialPrefab.gameObject.SetActive(true);
                    projectileLevel2Version3.gameObject.SetActive(false);
                    projectileLevel3Version3.gameObject.SetActive(false);
                    projectileLevel4Version3.gameObject.SetActive(false);
                    return;
                case 2:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version3.gameObject.SetActive(true);
                    projectileLevel3Version3.gameObject.SetActive(false);
                    projectileLevel4Version3.gameObject.SetActive(false);
                    return;
                case 3:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version3.gameObject.SetActive(false);
                    projectileLevel3Version3.gameObject.SetActive(true);
                    projectileLevel4Version3.gameObject.SetActive(false);
                    return;
                case 4:
                    projectileInitialPrefab.gameObject.SetActive(false);
                    projectileLevel2Version3.gameObject.SetActive(false);
                    projectileLevel3Version3.gameObject.SetActive(false);
                    projectileLevel4Version3.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The projectileLevel variable in scrTpwerProjectileLoader is set to an incorrect value");
                    return;
            }
        }
    }
}
