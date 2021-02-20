using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrProjectileLevelTracker : MonoBehaviour
{
    [SerializeField] public GameObject projectileLevel1;
    [SerializeField] public GameObject projectileLevel2;
    [SerializeField] public GameObject projectileLevel3;
    [SerializeField] public GameObject projectileLevel4;
    private int projectileLevel;
    private int projectileMaxLevel;
    
    //[SerializeField] public GameObject ProjectileLevel1 { get; private set; }
    //[SerializeField] public GameObject ProjectileLevel2 { get; private set; }
    //[SerializeField] public GameObject ProjectileLevel3 { get; private set; }
    //[SerializeField] public GameObject ProjectileLevel4 { get; private set; }

    private void Awake()
    {
        projectileLevel = 1;
        projectileMaxLevel = 4;
        ResetProjectileLevel();

        //ProjectileLevel1 = projectileLevel1;
        //ProjectileLevel2 = projectileLevel2;
        //ProjectileLevel3 = projectileLevel3;
        //ProjectileLevel4 = projectileLevel4;
    }

    public void ResetProjectileLevel()
    {
        projectileLevel1.SetActive(true);
        projectileLevel2.SetActive(false);
        projectileLevel3.SetActive(false);
        projectileLevel4.SetActive(false);
    }
    public void SetProjectileLevel(int _projectileLevel)
    {
        projectileLevel = _projectileLevel;
        if (projectileLevel < projectileMaxLevel)
        {
            switch (projectileLevel)
            {

                case 1:
                    projectileLevel1.gameObject.SetActive(true);
                    projectileLevel2.gameObject.SetActive(false);
                    projectileLevel3.gameObject.SetActive(false);
                    projectileLevel4.gameObject.SetActive(false);
                    return;
                case 2:
                    projectileLevel1.gameObject.SetActive(false);
                    projectileLevel2.gameObject.SetActive(true);
                    projectileLevel3.gameObject.SetActive(false);
                    projectileLevel4.gameObject.SetActive(false);
                    return;
                case 3:
                    projectileLevel1.gameObject.SetActive(false);
                    projectileLevel2.gameObject.SetActive(false);
                    projectileLevel3.gameObject.SetActive(true);
                    projectileLevel4.gameObject.SetActive(false);
                    return;
                case 4:
                    projectileLevel1.gameObject.SetActive(false);
                    projectileLevel2.gameObject.SetActive(false);
                    projectileLevel3.gameObject.SetActive(false);
                    projectileLevel4.gameObject.SetActive(true);
                    return;
                default:
                    Debug.LogError("The projectileLevel variable in scrTpwerProjectileLoader is set to an incorrect value");
                    return;
            }
        }
    }
}
