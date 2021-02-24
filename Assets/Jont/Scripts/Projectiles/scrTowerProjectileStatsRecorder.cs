using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class holds a reference to the different statsSO s for different projectiles the tower may use depending on the level.
/// </summary>

public class scrTowerProjectileStatsRecorder : MonoBehaviour
{
    [Header("Level 1 stats")]
    [SerializeField] private TowerProjectileTypeSO initialProjectile;
    [Header("Stats for path 1")]
    [SerializeField] private TowerProjectileTypeSO Path1Level2Projectile;
    [SerializeField] private TowerProjectileTypeSO Path1Level3Projectile;
    [SerializeField] private TowerProjectileTypeSO Path1Level4Projectile;
    [Header("Stats for path 2")]
    [SerializeField] private TowerProjectileTypeSO Path2Level2Projectile;
    [SerializeField] private TowerProjectileTypeSO Path2Level3Projectile;
    [SerializeField] private TowerProjectileTypeSO Path2Level4Projectile;
    [Header("Stats for path 3")]
    [SerializeField] private TowerProjectileTypeSO Path3Level2Projectile;
    [SerializeField] private TowerProjectileTypeSO Path3Level3Projectile;
    [SerializeField] private TowerProjectileTypeSO Path3Level4Projectile;

    private int currentTowerLevel;

    public int TowerProjectileUpgradePath { get; set; }

    public int TowerProjectileLevel { get; set; }

    private void Awake()
    {
        currentTowerLevel = 1;
        TowerProjectileLevel = currentTowerLevel;
        TowerProjectileUpgradePath = 0;
    }

    public TowerProjectileTypeSO SetInitialStasts() //Assigns the initial projectile stats
    {
        return initialProjectile;
    }
    public TowerProjectileTypeSO UpgradeProjectileStats(int _towerPath, int _towerLevel)
    {
        TowerProjectileLevel = _towerLevel;
        TowerProjectileUpgradePath = _towerPath;
        if (TowerProjectileLevel == 2) //If the tower is at level 2
        {
            switch(TowerProjectileUpgradePath)
            {
                case 1:
                    return Path1Level2Projectile;
                case 2:
                    return Path2Level2Projectile;
                case 3:
                    return Path3Level2Projectile;
                default:
                    Debug.LogError("The TowerProjectileUpgradePath variable in the scrTowerProjectileStatsRecorder script is assigned a number that is either null or incorrect.");
                    return initialProjectile;
            }
        }
        if (TowerProjectileLevel == 3)
        {
            switch (TowerProjectileUpgradePath)
            {
                case 1:
                    return Path1Level3Projectile;
                case 2:
                    return Path2Level3Projectile;
                case 3:
                    return Path3Level3Projectile;
                default:
                    Debug.LogError("The TowerProjectileUpgradePath variable in the scrTowerProjectileStatsRecorder script is assigned a number that is either null or incorrect.");
                    return initialProjectile;
            }
        }
        if (TowerProjectileLevel == 4)
        {
            switch (TowerProjectileUpgradePath)
            {
                case 1:
                    return Path1Level4Projectile;
                case 2:
                    return Path2Level4Projectile;
                case 3:
                    return Path3Level4Projectile;
                default:
                    Debug.LogError("The TowerProjectileUpgradePath variable in the scrTowerProjectileStatsRecorder script is assigned a number that is either null or incorrect.");
                    return initialProjectile;
            }
        }
        else
        {
            Debug.LogError("The TowerProjectileLevel variable in the scrTowerProjectileStatsRecorder script is assigned a number that is either null or incorrect.");
            return initialProjectile;
        }
    }
}
