using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDefenderAnimation : MonoBehaviour
{
    private Animator defenderAnimator;
    [SerializeField] private GameObject defenderBody;
    private scrUnitHealth defenderHealth;
    private float respawnTimer = 5f;

    private void Awake()
    {
        defenderAnimator = GetComponentInChildren<Animator>(); //Gets the reference of the animator
        defenderHealth = GetComponent<scrUnitHealth>(); //Gets the instance
    }
    public void PlayDeathAnimation()
    {
        defenderAnimator.SetTrigger("DefenderDies");
    }
    public void PlayAttackAnimation()
    {
        defenderAnimator.SetBool("DefenderInCombat", true);
    }
    public void StopAttackAnimation()
    {
        defenderAnimator.SetBool("DefenderInCombat", false);
    }
    public void PlayWalkAnimation()
    {
        //defenderAnimator.SetTrigger("");
    }
    private float GetCurrentAnimationLength()
    {
        float animLength = defenderAnimator.GetCurrentAnimatorStateInfo(0).length; //Useful function!
        return animLength;
    }
    private void DefenderKilled(Defender _defender)
    {
        //Debug.Log("I died"); 
        RespawnDefender(respawnTimer);
        StartCoroutine(RespawnDefender(respawnTimer));
        //Reset the defender position.
        //Respawn after timer
    }
    private IEnumerator RespawnDefender(float _respawnTimer)
    {
        PlayDeathAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength());
        defenderBody.SetActive(false); //Set the gameobject to unactive
        yield return new WaitForSeconds(_respawnTimer);
        defenderHealth.ResetHealth(); //Reset its health
        defenderBody.SetActive(true);
    }
    private void OnEnable()
    {
        scrUnitHealth.OnDefenderKilled += DefenderKilled;
    }
    private void OnDisable()
    {
        scrUnitHealth.OnDefenderKilled -= DefenderKilled;
    }
}
