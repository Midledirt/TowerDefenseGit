using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="ScriptableObject/Stats/Creeps")]
public class CreepStatsSO : ScriptableObject
{
    public float initialHealth;
    public float movementSpeed;
}
