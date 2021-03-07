using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDefenderStats : MonoBehaviour
{
    public int DefenderPossition { get; set; }

    private void Awake()
    {
        DefenderPossition = 1; //Initialize it to 1 in order to prevent bugs
    }
}
