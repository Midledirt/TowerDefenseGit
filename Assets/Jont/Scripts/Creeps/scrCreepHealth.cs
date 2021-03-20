using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// This class handles the healt amount for creeps
/// It communicates with other classes using (?) "properties".
/// This class is also managing the position of the healthbar. 
/// </summary>
public class scrCreepHealth : MonoBehaviour
{
    private CreepStatsSO stats;
    //Requires the namespace "System"!
    public static Action<Creep> OnEnemyKilled;
    public static Action<Defender> OnDefenderKilled;
    //IMPORTANT check tutorial episode 19. Timestamp 3.30 ish for HOW TO CHECK FOR SPESIFIC ENEMY ID
    public static Action<Creep> OnEnemyBlocked; //In stead of "on enemy hit"

    [SerializeField] private GameObject healthbarPreab;
    [SerializeField] private Transform barPosition;
    public GameObject InstantiatedHealthBar { get; private set; }
    public float CurrentHealth { get; private set; }

    private Image healthBar;
    //IMPORTANT, read text at the top
    private Creep _creep;
    private Defender _defender;

    private void Awake()
    {
        stats = GetComponent<scrCreepTypeDefiner>().creepType;
    }
    void Start()
    {
        CreateHealthbar();
        CurrentHealth = stats.initialHealth;

        //This could be written better i suppose, using virtual classes or such. I might go back and make that change later, if I deem it necessary
        //or feel like I would like the practise. 4 now tho, lets focus on getting this done at my current level of understanding.
        _creep = GetComponent<Creep>();
        _defender = GetComponent<Defender>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DealDamage(5f); //For test purposes
        }

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, CurrentHealth / stats.initialHealth, Time.deltaTime * 10f); //Updates and lerps the fillamount
        //between the currenthealth and the max health
    }

    private void CreateHealthbar()
    {
        InstantiatedHealthBar = Instantiate(healthbarPreab, barPosition.position, Quaternion.identity); //Instantiate healthbar
        InstantiatedHealthBar.transform.SetParent(transform); //Parent this bar to the enemy instance

        scrEnemyHealthContainer container = InstantiatedHealthBar.GetComponent<scrEnemyHealthContainer>(); //Assigns the healthbar image sprite of the 
        //scrEnemyHealthContainer script to this variable named container... I think.

        healthBar = container.MyFillAmount; //Assigns it to the health bar var
    }

    public void DealDamage(float damageRecieved)
    {
        CurrentHealth -= damageRecieved; //Take damage
        if (CurrentHealth <= 0)
        {
            InstantiatedHealthBar.SetActive(false); //Hide the healthbar when the defender dies
            CurrentHealth = 0;
            if(_creep != null)
            {
                creepDies(); //Kill the creep
            }
            if(_defender != null)
            {
                defenderDies(); //Kill the defender (if this is a defender)
            }
        }
        else //Remove this entire else statement later, when the animation is updated to trigger whilst enemy is in combat
        {
            OnEnemyBlocked?.Invoke(_creep);
        }
    }
    public void ResetHealth()
    {
        InstantiatedHealthBar.SetActive(true); //Turn the healthbar back on as the defender respawns
        CurrentHealth = stats.initialHealth; //Reset the health
        healthBar.fillAmount = 1f; //Reset the health bar
    }
    private void creepDies()
    {
        OnEnemyKilled?.Invoke(_creep); //Invokes the on enemy killed.
    }
    private void defenderDies() //Making a specific death action for defenders, just in case.
    {
        OnDefenderKilled?.Invoke(_defender);
    }
}
