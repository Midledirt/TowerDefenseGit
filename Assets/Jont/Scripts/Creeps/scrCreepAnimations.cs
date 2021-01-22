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
    private scrCreepHealth creepHealth;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //Get the reference to the animator conponent attached to this object
        _creep = GetComponent<Creep>();
        creepHealth = GetComponent<scrCreepHealth>();
    }

    private void PlayAttackAnimation() //Stand in for the "hurtAnimation" in the tutorial. This game will most likely not have a hurt animation
    {
        animator.SetTrigger("InCombat");
    }

    private void DieAnimation()
    {
        animator.SetTrigger("Dead");
    }

    private float GetCurrentAnimationLength()
    {
        float animLength = animator.GetCurrentAnimatorStateInfo(0).length; //Usefuil function!
        return animLength;
    }

    private IEnumerator PlayHurt()
    {
        _creep.StopMovement(); //See how you can call something from another script just like that? :O Once again, STUDY EPISODE 19!
        PlayAttackAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength() - 0.4f);
        _creep.ResumeMovement();
    }

    private IEnumerator PlayDeath()
    {
        _creep.StopMovement();
        //Find a way to stop any other animations?
        DieAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength() + 0.5f); //I think this one is faulty because if the creep is currently in the
        //"take damage" animation, and this is called, the "yield return..." will return the length of the current (ATTACK) animation, and so the
        //next ("DIE") animation will be cut short.
        _creep.ResumeMovement();
        creepHealth.ResetHealth();
        _creep.ReturnPosition(_creep); //Reset the path variable for the creep (so it does not teleport back onto the same place when it respawns)
        ObjectPooler.ReturnToPool(_creep.gameObject); //Moved this
    }

    private void CreepHit(Creep creep)
    {
        if (_creep == creep) //Checks if this enemy is the same one that is hit as the one with the other script
            //Confusing... This is covered in the turorial at episode 19. An episode I will need 2 study
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
        scrCreepHealth.OnEnemyBlocked += CreepHit;
        scrCreepHealth.OnEnemyKilled += CreepDead;
    }

    private void OnDisable()
    {
        scrCreepHealth.OnEnemyBlocked -= CreepHit;
        scrCreepHealth.OnEnemyKilled -= CreepDead;
    }
}
