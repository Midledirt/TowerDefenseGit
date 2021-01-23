using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Waves/WaveSO")]
public class WaveSO : ScriptableObject
{
    public List<GroupSO> Group;
    [SerializeField] private float timeBetweenGroups;
}
