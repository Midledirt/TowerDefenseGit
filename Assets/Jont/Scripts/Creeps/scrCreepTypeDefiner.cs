using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrCreepTypeDefiner : MonoBehaviour
{
    [Header("Creep Type")]
    [Tooltip("What stats this creep will have (initial stats for defenders)")]
    [SerializeField] private CreepStatsSO creepType;

    //For defenders
    [Header("Stats for path 1")]
    [SerializeField] private CreepStatsSO Path1Level2Defender;
    [SerializeField] private CreepStatsSO Path1Level3Defender;
    [SerializeField] private CreepStatsSO Path1Level4Defender;
    [Header("Stats for path 2")]
    [SerializeField] private CreepStatsSO Path2Level2Defender;
    [SerializeField] private CreepStatsSO Path2Level3Defender;
    [SerializeField] private CreepStatsSO Path2Level4Defender;
    [Header("Stats for path 3")]
    [SerializeField] private CreepStatsSO Path3Level2Defender;
    [SerializeField] private CreepStatsSO Path3Level3Defender;
    [SerializeField] private CreepStatsSO Path3Level4Defender;

    private scrUnitHealth defenderHealth;
    private scrAnimationEventHandler animationEventHandler; //Responsible for dealing damage

    private int currentTowerLevel;

    public int TowerDefenderUpgradePath { get; private set; }

    public int TowerDefenderLevel { get; private set; }

    public CreepStatsSO CreepType { get; private set; }
    private void Awake()
    {
        CreepType = creepType;

        //For defenders
        currentTowerLevel = 1;
        TowerDefenderLevel = currentTowerLevel;
        TowerDefenderUpgradePath = 0;
        defenderHealth = GetComponent<scrUnitHealth>();
        animationEventHandler = GetComponentInChildren<scrAnimationEventHandler>();
    }
    private void UpdateHealthAndDamage(CreepStatsSO newStats)
    {
        defenderHealth.DefenderUpgraded(newStats);
        animationEventHandler.DefenderIsUpgraded(newStats);
    }
    public CreepStatsSO UpgradeDefenderStats(int _towerPath, int _towerLevel)
    {
        //print("Upgrading defender stats...");
        TowerDefenderLevel = _towerLevel;
        TowerDefenderUpgradePath = _towerPath;
        if (TowerDefenderLevel == 2) //If the tower is at level 2
        {
            //print("Defenders are now level 2");
            switch (TowerDefenderUpgradePath)
            {
                case 1:
                    UpdateHealthAndDamage(Path1Level2Defender);
                    return CreepType = Path1Level2Defender;
                case 2:
                    UpdateHealthAndDamage(Path2Level2Defender);
                    return CreepType = Path2Level2Defender;
                case 3:
                    UpdateHealthAndDamage(Path3Level2Defender);
                    return CreepType = Path3Level2Defender;
                default:
                    Debug.LogError("The TowerProjectileUpgradePath variable in the scrTowerProjectileStatsRecorder script is assigned a number that is either null or incorrect.");
                    return CreepType = creepType;
            }
        }
        if (TowerDefenderLevel == 3)
        {
            //print("Defenders are now level 3");
            switch (TowerDefenderUpgradePath)
            {
                case 1:
                    UpdateHealthAndDamage(Path1Level3Defender);
                    return CreepType = Path1Level3Defender;
                case 2:
                    UpdateHealthAndDamage(Path2Level3Defender);
                    return CreepType = Path2Level3Defender;
                case 3:
                    UpdateHealthAndDamage(Path3Level3Defender);
                    return CreepType = Path3Level3Defender;
                default:
                    Debug.LogError("The TowerProjectileUpgradePath variable in the scrTowerProjectileStatsRecorder script is assigned a number that is either null or incorrect.");
                    return CreepType = creepType;
            }
        }
        if (TowerDefenderLevel == 4)
        {
            //print("Defenders are now level 4");
            switch (TowerDefenderUpgradePath)
            {
                case 1:
                    UpdateHealthAndDamage(Path1Level4Defender);
                    return CreepType = Path1Level4Defender;
                case 2:
                    UpdateHealthAndDamage(Path2Level4Defender);
                    return CreepType = Path2Level4Defender;
                case 3:
                    UpdateHealthAndDamage(Path3Level4Defender);
                    return CreepType = Path3Level4Defender;
                default:
                    Debug.LogError("The TowerProjectileUpgradePath variable in the scrTowerProjectileStatsRecorder script is assigned a number that is either null or incorrect.");
                    return CreepType = creepType;
            }
        }
        else
        {
            Debug.LogError("The TowerProjectileLevel variable in the scrTowerProjectileStatsRecorder script is assigned a number that is either null or incorrect.");
            return CreepType = creepType;
        }
    }

}
