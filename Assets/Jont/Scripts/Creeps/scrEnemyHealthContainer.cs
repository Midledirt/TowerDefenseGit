using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrEnemyHealthContainer : MonoBehaviour
{
    //The reason why this class exists and is placed on the canvas is that it needs the reference for the health slider
    [SerializeField] private Image myFillAmount;

    //Take a look at this: It returns the variable above, I think...
    //ANSWERING MYSELF AT A LATER DATE:
    //Yes, saw a video today covering that. The code bellow is a function... or an one line if statement? It does something similar to this:
    
    //public Image MyFillAmount()
    //{
    //   myFillAmount;
    //}
    public Image MyFillAmount => myFillAmount;
}
