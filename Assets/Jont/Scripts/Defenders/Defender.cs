using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    scrCreepHealth defenderHealth;
    [Tooltip("Drag the body of the defender itself into this slot")]
    [SerializeField] private GameObject defenderBody;
    [SerializeField] private float respawnTimer = 2f;

    private void Awake()
    {
        defenderHealth = GetComponent<scrCreepHealth>(); //Gets the instance
    }
    private void DefenderKilled(Defender _defender)
    {
        Debug.Log("I died"); 
        defenderBody.SetActive(false); //Set the gameobject to unactive
        RespawnDefender(respawnTimer);
        StartCoroutine(RespawnDefender(respawnTimer));
        //Reset the defender position.
        //Respawn after timer
    }
    private IEnumerator RespawnDefender(float _respawnTimer)
    {
        yield return new WaitForSeconds(_respawnTimer);
        defenderHealth.ResetHealth(); //Reset its health
        defenderBody.SetActive(true);
    }
    
    private void OnEnable()
    {
        scrCreepHealth.OnDefenderKilled += DefenderKilled;
    }
    private void OnDisable()
    {
        scrCreepHealth.OnDefenderKilled -= DefenderKilled;
    }
}
