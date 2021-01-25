using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/WaveSO")]
public class scrWaveSO : ScriptableObject
{
    [Header("Wave data")]
    [Tooltip("Define the name of the wave prefab in the inspector")]
    [SerializeField] private string WaveName = "";
    [Header("CreepList")]
    [Tooltip("Drag creep prefabs you want to instantiate with the wave here")]
    [SerializeField] public List<GameObject> creepList;
    [Tooltip("How many seconds from awake until this wave spawns")]
    [SerializeField] private float setWaveTimer;
    [SerializeField] private float setTimeBetweenCreeps;
    [HideInInspector] public float waveTimer;
    [HideInInspector] public float timeBetweenCreeps;

    private int numberOfCreepsToSpawn; //This is used to keep the spawn function from running once it has spawned all enemies
    private int numberOfCreepsSpawned; //This is used to keep the spawn function from running once it has spawned all enemies
    private GameObject WaveContainer;
    public List<GameObject> referenceList; //Logikken (min) bak dette: 
    //referenceList lager jeg fordi at jeg ønsker å bruke de spesefike instansene av Gameobjects som lages i InitialiazeCreeps() senere.
    //Dette er nødvendig fordi "creepList" inneholder kun "prefabs" ikke FAKTISKE GAMEOBJETS. Så det vil ikke hjelpe å "sette dem til active(true)"
    //senere med en "foreach loop", fordi det setter bare "prefabene" (som ikke har blitt spawnet uansett) til true.

    public void InitializeCreeps()
    {
        numberOfCreepsToSpawn = 0; //Remember you always need to reset these numbers when you work with SOs
        numberOfCreepsSpawned = 0;

        referenceList = new List<GameObject>();

        WaveContainer = new GameObject($"Wave Container for wave: " + WaveName);

        foreach (GameObject creep in creepList)
        {
            GameObject newInstance = Instantiate(creep);

            newInstance.transform.SetParent(WaveContainer.transform);

            newInstance.SetActive(false);

            referenceList.Add(newInstance); //Add this newly created instance of a prefab into a list that can be referenced later
            numberOfCreepsToSpawn += 1;
        }
        
    }

    public GameObject ReturnInstanceFromPool()
    {
        for (int i = 0; i < referenceList.Count; i++)
        {
            if (!referenceList[i].activeInHierarchy)
            {
                return referenceList[i];
            }
        }
        return null; //There are no more gameobjects to spawn

    }
    public void SpawnCreep()
    {
        if (timeBetweenCreeps >= 0f && numberOfCreepsSpawned < numberOfCreepsToSpawn)
        {
            timeBetweenCreeps -= Time.deltaTime;
            if (timeBetweenCreeps <= 0f)
            {
                timeBetweenCreeps = setTimeBetweenCreeps;
                GameObject newInstance = ReturnInstanceFromPool();
                newInstance.SetActive(true);
                numberOfCreepsSpawned += 1;
            }
        }
    }
    public void ResetTimers()
    {
        waveTimer = setWaveTimer;
        timeBetweenCreeps = setTimeBetweenCreeps;
    }

    public void IncrementWaveTimer()
    {
        if (waveTimer >= 0f)
        {
            waveTimer -= Time.deltaTime;
        }
    }
}
