using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is an EXTREMELY usefull class that is used by several other classes.
/// Its function is this:
/// - Instantiates a group of gameobjects, specified in the INSPECTOR
/// - instantiated objects are put into a <list>
/// - these will then be instantiated at the location of this pooler
/// - instantiated objects will be set as (inactive)
///        - The reason why is that this allows us to load the necesary amount of gameobjects once, when the scene loads, instad of continuously.
/// - other scripts can then take instantiated objects from this <list> and set them active
/// IMPORTANT: This class will also "return" (set objects back to (inactive)) afther a certain time. This is good functionality for projectiles, 
/// however this may not be such a good idea for instantiated "soldiers" later on. I may make a new pooler specifically for barrack soldiers. Or 
/// I may use something like a bool to filther "projectiles" from "soldiers"
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;
    [Header("Set projectile spawner")]
    [Tooltip("Set this to true, if this pooler is spawning projectiles")]
    [SerializeField] private bool isProjectileSpawner = false;
    private scrTowerLevelTracker towerLevelTracker; 

    private List<GameObject> pool;

    //Create a new object that houses all the instantiated prefabs in runtime
    private GameObject poolContainer;

    private void Awake()
    {
        //Initialize pool
        pool = new List<GameObject>();

        poolContainer = new GameObject($"Pool of {prefab.name}");

        towerLevelTracker = GetComponent<scrTowerLevelTracker>(); //Get the instance on this object
    }
    private void Start()
    {

        //Run the createPooler method from below, based on the poolsize
        CreatePooler();
    }

    private void CreatePooler()
    {
        for (int i = 0; i < poolSize; i++)
        {
            //References the createInstance method below
            pool.Add(CreateInstance());
        }
    }

    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab);
        if (isProjectileSpawner)
        {
            newInstance.GetComponent<scrProjectileLevelTracker>().SetProjectileLevel(towerLevelTracker.CurrentTowerLevel);
        }

        //Set the parent to be the pool container
        newInstance.transform.SetParent(poolContainer.transform);

        newInstance.SetActive(false);
        return newInstance;
    }
    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            //Return any instances that are NOT active in the heirarchy
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        //And if there are no non-active instances, create one instead
        return CreateInstance();
    }

    //Static lets us use this without a reference
    public static void MoveToDeathPool(GameObject instance)
    {
        //Destroy(instance); //I don`t know if this might be a bad way of handling it. Is this resources intensive or error prone?
        //I do it this way, because if a object is returned to the hierarchy after its death(or reaching the end of the path) the spawner might prioritize
        //respawning it instead of spawning the next gameobject instance. This screws up my group spawner.
        instance.SetActive(false);
    }
    public void TowerUpgraded()
    {
        if (isProjectileSpawner)
        {
            foreach(GameObject _projectile in pool)
            {
                _projectile.GetComponent<scrProjectileLevelTracker>().SetProjectileLevel(towerLevelTracker.CurrentTowerLevel);
            }
        }
    }

    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
    }
}
