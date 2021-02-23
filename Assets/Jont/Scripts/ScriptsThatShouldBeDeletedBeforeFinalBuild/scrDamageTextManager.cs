using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDamageTextManager : MonoBehaviour
{
    public ObjectPooler TextPooler { get; set; }

    public static scrDamageTextManager instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        TextPooler = GetComponent<ObjectPooler>(); //Gets the object pooler instance on this gameobject
    }
}
