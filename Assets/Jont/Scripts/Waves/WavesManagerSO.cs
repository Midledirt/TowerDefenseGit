using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Waves/WavesManagerSO")]
public class WavesManagerSO : ScriptableObject
{
    public List<WaveSO> waves;
    [SerializeField] private float timeBetweenWaves; //Make this into a property later, so it can be called as you the game is playing
}
