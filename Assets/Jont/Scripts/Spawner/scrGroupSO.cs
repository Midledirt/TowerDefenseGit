using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[CreateAssetMenu (menuName = "ScriptableObject/GroupSO")]
public class scrGroupSO : ScriptableObject
{
    [Header("Group data")]
    [Tooltip("Define the name of the group prefab in the inspector")]
    [SerializeField] private string groupName = "";
    [Header("CreepList")]
    [Tooltip("Drag creep prefabs you want to instantiate with the group here")]
    [SerializeField] private List<GameObject> creepList;
    [HideInInspector] public List<GameObject> actualCreepList; //Used because this is a scriptable Object, so this var needs to be reset //CAN WE MAKE THIS A LOCAL VARIABLE THAT IS USED FOR CREEP INSTANCES? :o
    [Tooltip("How many seconds from awake until this group spawns")]
    [SerializeField] private float setWaveTimer;
    [Tooltip("How much time passes between each spawn in the group")]
    [SerializeField] private float setTimeBetweenCreeps;
    [Tooltip("Decide what path the creep will use")]
    [Range(0, 2)]
    [SerializeField] private int groupPathVar;
    [HideInInspector] public int GroupPath { get; private set; }

    [SerializeField] private int setGroupPlacement;
    public int GroupWavePlacement { get; set; }


    [HideInInspector] public float groupTimer;
    [HideInInspector] public float timeBetweenCreeps;
    [HideInInspector] public int CreepPossition { get; private set; }

    private int numberOfCreepsToSpawn; //This is used to keep the spawn function from running once it has spawned all enemies
    private int numberOfCreepsSpawned; //This is used to keep the spawn function from running once it has spawned all enemies
    private GameObject WaveContainer;
    private List<GameObject> referenceList; //Logikken (min) bak dette: 
    //referenceList lager jeg fordi at jeg ønsker å bruke de spesefike instansene av Gameobjects som lages i InitialiazeCreeps() senere.
    //Dette er nødvendig fordi "creepList" inneholder kun "prefabs" ikke FAKTISKE GAMEOBJETS. Så det vil ikke hjelpe å "sette dem til active(true)"
    //senere med en "foreach loop", fordi det setter bare "prefabene" (som ikke har blitt spawnet uansett) til true.

    public void InitializeCreeps(PathCreator path)
    {
        numberOfCreepsToSpawn = 0; //Remember you always need to reset these numbers when you work with SOs
        numberOfCreepsSpawned = 0;

        referenceList = new List<GameObject>();

        WaveContainer = new GameObject($"Wave Container for wave: " + groupName);

        foreach (GameObject creep in actualCreepList)
        {
            GameObject newInstance = Instantiate(creep);

            newInstance.transform.SetParent(WaveContainer.transform);

            newInstance.SetActive(false);

            Creep stats = newInstance.GetComponent<Creep>();
            stats.SetPath(path);

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
  
    public void ResetVars()
    {
        GroupWavePlacement = setGroupPlacement;
        actualCreepList = creepList;
        GroupPath = groupPathVar;
        groupTimer = setWaveTimer;
        timeBetweenCreeps = setTimeBetweenCreeps;
    }

    public void IncrementGroupTimer()
    {
        if (groupTimer >= 0f)
        {
            groupTimer -= Time.deltaTime;
        }
    }
}
