using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates an enumerator for switching between spawn modes
public enum SpawnModes
{
    Fixed,
    Random
}

public class scrSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float delayBtweenWaves = 1f; //How much time there is between each wave of enemies

    [Header("Fixed Delay")]
    [SerializeField] private float delayBetweenSpawns;

    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;


    private float mySpawnTimer;
    private int numberOfEnemiesSpawned;
    private int creepsRemaining; //How many enemies are still alive

    private ObjectPooler myPooler;
    private scrWaypoint waypoint;

    private void Start()
    {
        //This will work for as long as the pooler script is on the same object as the spawner script!
        myPooler = GetComponent<ObjectPooler>();
        waypoint = GetComponent<scrWaypoint>();

        creepsRemaining = enemyCount; 
    }

    // Update is called once per frame
    void Update()
    {
        //Decrease the spawntimer every second
        mySpawnTimer -= Time.deltaTime;
        if (mySpawnTimer < 0)
        {
            //This method is made bellow
            mySpawnTimer = GetSpawnDelay();
            if (numberOfEnemiesSpawned < enemyCount)
            {
                numberOfEnemiesSpawned++;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        //Reference the instance created by the pooler
        GameObject newInstance = myPooler.GetInstanceFromPool();

        Creep creep = newInstance.GetComponent<Creep>();
        creep.myWaypoint = waypoint;
        creep.ResetCreep(); //Runs a function that (at the time of writing) simply resets the creep`s end checkpoint back to 0

        creep.transform.localPosition = transform.position;

        //The new instance is set to unactive by default in the pooler. This will activate it
        newInstance.SetActive(true);
    }

    //Checks what spawn mode is used ("Fixed" or "Random"), and returns the right delay
    private float GetSpawnDelay()
    {
        float delay = 0f;

        //If we are using fixed
        if (spawnMode == SpawnModes.Fixed)
        {
            delay = delayBetweenSpawns;
        }
        //Else (if we are using random)
        else
        {
            delay = GetRandomDelay();
        }

        return delay;
    }

    //Gets the random delay between spawns, IF random is selected
    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay, maxRandomDelay);
        return randomTimer;
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtweenWaves);
        //Reset the variables
        creepsRemaining = enemyCount;
        mySpawnTimer = 0f;
        numberOfEnemiesSpawned = 0;
    }
    private void RecordCreep(Creep creep)
    {
        //This is meant to start the next waves, as all enemies are killed. Thats not how I want it 2 work in the final game. I want it
        //To be time based instead. 
        creepsRemaining--;
        if (creepsRemaining <= 0)
        {
            //Start the next wave
            StartCoroutine(NextWave());
        }
    }

    //Important 2 understand: 
    //This is how we can make different scripts do something, when something happens in one script:
    private void OnEnable()
    {
        Creep.OnEndReaced += RecordCreep;
        scrCreepHealth.OnEnemyKilled += RecordCreep;
    }
    //Important 2 understand: 
    //This is how we can make different scripts do something, when something happens in one script:
    private void OnDisable()
    {
        Creep.OnEndReaced -= RecordCreep;
        scrCreepHealth.OnEnemyKilled -= RecordCreep;
    }
}
