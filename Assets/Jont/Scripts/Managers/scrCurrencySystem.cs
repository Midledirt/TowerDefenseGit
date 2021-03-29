using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrCurrencySystem : Singleton<scrCurrencySystem>
{
    [SerializeField] private int coinTest;
    private string CURRENCY_SAVE_KEY = "MYGAME_CURRENCY"; //Setting up for saving, I think

    public int TotalCoins { get; set; }

    private void Start()
    {
        PlayerPrefs.DeleteKey(CURRENCY_SAVE_KEY); //This is done so that we don`t keep the coins we got(or had left) after last play
        LoadCoins();
    }

    private void LoadCoins()
    {
        TotalCoins = PlayerPrefs.GetInt(CURRENCY_SAVE_KEY, coinTest);
    }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
        PlayerPrefs.Save();
    }
    public void SpendCoins(int amount)
    {
        if (TotalCoins >= amount) //Check that we have enough to spend
        {
            TotalCoins -= amount;
            PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
            PlayerPrefs.Save();
        }
    }

    private void EarnCoins(Creep creep) //Triggers when enemies are killed
    {
        AddCoins(1);
    }

    private void OnEnable()
    {
        scrUnitHealth.OnEnemyKilled += EarnCoins; //Subscribe
    }

    private void OnDisable()
    {
        scrUnitHealth.OnEnemyKilled -= EarnCoins; //Desubscribe
    }

}
