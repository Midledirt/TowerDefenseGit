using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class handles the healt amount for creeps
/// It communicates with other classes using (?) "properties".
/// This class is also managing the position of the healthbar. 
/// </summary>
public class scrCreepHealth : MonoBehaviour
{
    //Requires the namespace "System"!
    public static Action<Creep> OnEnemyKilled;
    //IMPORTANT check tutorial episode 19. Timestamp 3.30 ish for HOW TO CHECK FOR SPESIFIC ENEMY ID
    public static Action<Creep> OnEnemyBlocked; //In stead of "on enemy hit"

    [SerializeField] private GameObject healthbarPreab;
    [SerializeField] private Transform barPosition;

    [SerializeField] private float initialHealth = 10f;
    [SerializeField] private float maxHealth = 10f;

    public float currentHealth { get; set; }

    private Image healthBar;
    //IMPORTANT, read text at the top
    private Creep _creep;

    // Start is called before the first frame update
    void Start()
    {
        CreateHealthbar();
        currentHealth = initialHealth;

        //IMPORTANT, read text at the top
        _creep = GetComponent<Creep>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DealDamage(5f); //For test purposes
        }

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, Time.deltaTime * 10f); //Updates and lerps the fillamount
        //between the currenthealth and the max health
    }

    private void CreateHealthbar()
    {
        GameObject newBar = Instantiate(healthbarPreab, barPosition.position, Quaternion.identity); //Instantiate healthbar
        newBar.transform.SetParent(transform); //Parent this bar to the enemy instance

        scrEnemyHealthContainer container = newBar.GetComponent<scrEnemyHealthContainer>(); //Assigns the healthbar image sprite of the 
        //scrEnemyHealthContainer script to this variable named container... I think.

        healthBar = container.MyFillAmount; //Assigns it to the health bar var
    }

    public void DealDamage(float damageRecieved)
    {
        currentHealth -= damageRecieved; //Take damage
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            creepDies(); //Kill the creep
        }
        else //Remove this entire else statement later, when the animation is updated to trigger whilst enemy is in combat
        {
            OnEnemyBlocked?.Invoke(_creep);
        }
    }

    public void ResetHealth()
    {
        currentHealth = initialHealth; //Reset the health
        healthBar.fillAmount = 1f; //Reset the health bar
    }

    private void creepDies()
    {
        OnEnemyKilled?.Invoke(_creep); //Invokes the on enemy killed. Why is there a questionmark afteher it tho... The questionmark makes this
        //an "if" statement, is what I believe So: If (OnEnemyKilled == true) i suppose
    }
}
