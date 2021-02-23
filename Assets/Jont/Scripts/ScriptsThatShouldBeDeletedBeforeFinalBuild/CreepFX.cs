using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// THIS CLASS SHOULD BE DELETED
/// It is only here for testing purposes, as I do not intend to have damage numbers as part of my final game
/// </summary>
public class CreepFX : MonoBehaviour
{
    [SerializeField] private Transform textDamageSpawnPossition;

    private Creep creep;

    private void Start()
    {
        creep = GetComponent<Creep>();
    }

    private void CreepHit(Creep _creep, float damage)
    {
        if (creep == _creep)
        {
            GameObject newInstance = scrDamageTextManager.instance.TextPooler.GetInstanceFromPool(); //Gets the instance in the pool, in the pooler, on the 
            //damagetextmanager instance (its a singleton)
            TextMeshProUGUI damageText = newInstance.GetComponent<scrDamageText>().DamageText;
            damageText.text = damage.ToString();

            newInstance.transform.SetParent(textDamageSpawnPossition);
            newInstance.transform.position = textDamageSpawnPossition.position;
            newInstance.SetActive(true);
        }
    }

    private void OnEnable()
    {
        scrProjectiles.OnCreepHit += CreepHit;
    }
    private void OnDisable()
    {
        scrProjectiles.OnCreepHit -= CreepHit;
    }
}
