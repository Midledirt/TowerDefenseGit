using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/WaveSO")]
public class scrWaveSO : ScriptableObject
{
    [SerializeField] public List<GameObject> creepList;
    [Tooltip("How many seconds from awake until this wave spawns")]
    [SerializeField] private float waveTimer;
    private GameObject WaveContainer;

    public List<GameObject> referenceList; //Logikken (min) bak dette: 
    //referenceList lager jeg fordi at jeg ønsker å bruke de spesefike instansene av Gameobjects som lages i InitialiazeCreeps() senere.
    //Dette er nødvendig fordi "creepList" inneholder kun "prefabs" ikke FAKTISKE GAMEOBJETS. Så det vil ikke hjelpe å "sette dem til active(true)"
    //senere med en "foreach loop", fordi det setter bare "prefabene" (som ikke har blitt spawnet uansett) til true.

    public void InitializeCreeps()
    {
        referenceList = new List<GameObject>();

        WaveContainer = new GameObject($"Wave Container");

        foreach (GameObject creep in creepList)
        {
            GameObject newInstance = Instantiate(creep);

            newInstance.transform.SetParent(WaveContainer.transform);

            newInstance.SetActive(false);

            referenceList.Add(newInstance); //Add this newly created instance of a prefab into a list that can be referenced later
        }
        
    }

    public void SetCreepActive()
    {
        foreach (GameObject creep in referenceList)
        {
            creep.SetActive(true);
        }
    }
    

    
}
