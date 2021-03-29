using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles the different animations for the (at least for now) first test creerp
/// It should be expanded to work with other 3D models and animations as well, that is, if it doesn`t already...
/// IMPORTANT: This class comunicates with others through a subscription system. (See the code at the bottom)
/// </summary>
public class scrCreepAnimations : MonoBehaviour
{
    private Animator animator; //Sets up a "generic" animator
    private Creep _creep;
    private scrCreepEngagementHandler engagementHandler;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>(); //Get the reference to the animator conponent attached to this object
        _creep = GetComponent<Creep>();
        engagementHandler = GetComponent<scrCreepEngagementHandler>(); //Gets the reference on this game object
    }
    private void Update()
    {
        switch(engagementHandler.ThisCreepIsEngaged)
        {
            case true:
                if(animator.GetBool("CreepIsInCombat") != true && _creep._CreepHealth.CurrentHealth > 0)
                {
                    //print("Creep starting combat animation"); //TESTED TO WORK
                    PlayAttackAnimation();
                }
                return;
            case false:
                //print("Stop attack animation, return to moving");
                StopAttackAnimation();
                return;
            default:
                return;
        }
    }
    public void PlayAttackAnimation()
    {
        animator.SetBool("CreepIsInCombat", true);
    }
    public void StopAttackAnimation()
    {
        if(animator.GetBool("CreepIsInCombat") != false)
        {
            animator.SetBool("CreepIsInCombat", false);
            //print("Stopping");
        }
    }
    private void DieAnimation()
    {
        animator.SetBool("CreepIsInCombat", false);
        animator.SetTrigger("Dead");
    }
    private float GetCurrentAnimationLength()
    {
        float animLength = animator.GetCurrentAnimatorStateInfo(0).length; //Useful function!
        return animLength;
    }
    private IEnumerator PlayHurt()
    {
        //_creep.StopMovement(); //See how you can call something from another script just like that? :O Once again, STUDY EPISODE 19!
        //PlayAttackAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength() - 0.4f);
        //_creep.ResumeMovement();
    }
    private IEnumerator PlayDeath()
    {
        _creep.StopMovement();
        //Find a way to stop any other animations?
        DieAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength() + 0.5f); //I think this one is faulty because if the creep is currently in the
        //"take damage" animation, and this is called, the "yield return..." will return the length of the current (ATTACK) animation, and so the
        //next ("DIE") animation will be cut short.
        ObjectPooler.SetObjectToInactive(_creep.gameObject); //Only sets the gameobject to "not active".
    }
    private void CreepHit(Creep creep)
    {
        if (_creep == creep)
        {
            StartCoroutine(PlayHurt());
        }
    }
    private void CreepDead(Creep creep)
    {
        if (_creep == creep)//Check that the reference is for the correct object
        {
            StartCoroutine(PlayDeath());
        }
    }

    private void OnEnable()
    {
        scrUnitHealth.OnEnemyBlocked += CreepHit;
        scrUnitHealth.OnEnemyKilled += CreepDead;
    }

    private void OnDisable()
    {
        scrUnitHealth.OnEnemyBlocked -= CreepHit;
        scrUnitHealth.OnEnemyKilled -= CreepDead;
    }
}
