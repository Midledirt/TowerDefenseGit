﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrRallyPointPlacement : MonoBehaviour
{
    public int DefenderPossition { get; set; } //Used for defining the defender possition in formation (rally point)

    private void Awake()
    {
        DefenderPossition = 1; //Initialize it to 1 in order to prevent bugs
    }
}
