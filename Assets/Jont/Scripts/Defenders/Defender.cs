using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] private float respawnTimer = 2f;
    scrCreepHealth defenderHealth;
    private bool defenderIsDead;

    private void Awake()
    {
        defenderHealth = GetComponent<scrCreepHealth>(); //Gets the instance
        defenderIsDead = false;
    }
    private void DefenderKilled(Defender _defender)
    {
        Debug.Log("I died");
        defenderIsDead = true;
        transform.gameObject.SetActive(false); //Set the gameobject to unactive
        defenderHealth.ResetHealth(); //Reset its health
        StartRespawnTimer(respawnTimer);
        //Reset the defender position.
        //Respawn after timer
    }
    private void StartRespawnTimer(float _respawnTimer) //This wont work because this function will not run whilst the defender is dead. Perhaps I should make
        //a new script on the defender tower for handling this? A script that has constant reference for ALL the defenders
    {
        if (defenderIsDead)
        {
            _respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                respawnTimer = 0;
                transform.gameObject.SetActive(true);
                defenderIsDead = false;
            }
        }
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
