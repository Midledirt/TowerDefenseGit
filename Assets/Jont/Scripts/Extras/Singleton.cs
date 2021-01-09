using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This will use a generic singleton pattern. It will allow me to use this code, anywhere I wish. However, be carefull when using singletons.
/// </summary>
/// Creating the singleton:
///     Add <T> "Keyword" to the name of the class.
///     Actually, just watch episode 28 "SingletonPattern"...

// public class DamageTextManager : Singleton<DamageTextManager>
public class Singleton<T> : MonoBehaviour where T : Component
{
    //This script is currently not in use
    private static T instance;
    
    public static T Instance
    {
        get
        {
            if (instance == null) //If we have no reference...
            {
                instance = FindObjectOfType<T>(); //Look for one
                if (instance == null) //If we still don`t have one...
                {
                    //Make one:
                    GameObject newInstance = new GameObject();
                    instance = newInstance.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    public void Awake()
    {
        instance = this as T;
    }
}
