﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Tower/Projectile")]
public class TowerProjectileTypeSO : ScriptableObject
{
    [Header("Projectile")]
    [Tooltip("Define the projectile type")]
    [SerializeField] private string projectileType;
    [Header("Stats")]
    [Tooltip("How much damage the projectile does")]
    [SerializeField] private float projectileDamage;
    [Tooltip("How much time there is between each attacks")] 
    [SerializeField] private float delayBetweenAttacks;
    public float ProjectileDamage { get; private set; }
    public float DelayBetweenAttacks { get; private set; }

    public void ResetStats()
    {
        //Sets the stats back to assigned inspector value. In case they will be effected by other elements in the game at runtime
        ProjectileDamage = projectileDamage;
        DelayBetweenAttacks = delayBetweenAttacks;
    }
}
