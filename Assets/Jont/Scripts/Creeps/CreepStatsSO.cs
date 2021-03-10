using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="ScriptableObject/Stats/Creeps")]
public class CreepStatsSO : ScriptableObject
{
    [Tooltip("Assign health for creeps or defenders")]
    public float initialHealth;
    [Tooltip("Assign movement speed. Does NOT affect DEFENDERS inhereting from creep stats")]
    public float movementSpeed;
}
