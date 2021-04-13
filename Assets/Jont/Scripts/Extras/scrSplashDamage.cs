using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSplashDamage : MonoBehaviour
{
    [Tooltip("Decides if this will target defenders or creep")]
    [SerializeField] private bool targetsDefenders;
    [Tooltip("Assign the explosion effect used by this AOE attack")]
    [SerializeField] private GameObject ExplosionEffect;
    private ParticleSystem effect;
    [Tooltip("Decides if the splash damage from this object uses an explosion effect")]
    [SerializeField] private bool objectHasExplosionEffect;
    [SerializeField] private float splashDamageRadius = 5f;
    [SerializeField] private LayerMask whatCanBeTargeted;
    [Tooltip("The minimum damage dealth regardless of distance to target")]
    [SerializeField] private float minimumDamage = 5f;
    public float MinimumDamage { get; private set; }
    [Tooltip("Set how quickly the damage will be reduced based on distance")]
    [SerializeField] private int damageFallOff;

    private void Awake()
    {
        if(objectHasExplosionEffect)
        {
            ExplosionEffect = Instantiate(ExplosionEffect, transform.position, transform.rotation);
            effect = ExplosionEffect.GetComponentInChildren<ParticleSystem>();
            ExplosionEffect.SetActive(false);
        }
    }
    private void Start()
    {
        MinimumDamage = minimumDamage;
    }
    public void DealSplashDamage(Vector3 _location, float _damage)
    {
        //Show explosion effect
        if(objectHasExplosionEffect)
        {
            ExplosionEffect.transform.position = transform.position;
            ExplosionEffect.SetActive(true);
            effect.Play();
        }

        //Check if "we" are the referenced spash damage instance, find out where to cast the explosion
        Collider[] Colliders = Physics.OverlapSphere(_location, splashDamageRadius, whatCanBeTargeted);
        foreach(Collider _collider in Colliders)
        {
            _collider.TryGetComponent<scrDefenderMovement>(out scrDefenderMovement _defender); //Checks if the target was a defender
            if(!targetsDefenders && _defender != null) //If we don`t target defenders, and this hit a defender...
            {
                break; //stop, continue checking the next colliders
            }
            _collider.TryGetComponent<scrUnitHealth>(out scrUnitHealth _targetHealth);
            if(_targetHealth != null)
            {
                //Check distance
                float distanceToTarget = (_location - _collider.transform.position).magnitude; //Check distance to target
                _damage = _damage - (distanceToTarget * damageFallOff);
                if(_damage < MinimumDamage)
                {
                    _damage = MinimumDamage;
                }
                //Deal damage based on distance
                print("Damage dealth is: " + _damage);
                _targetHealth.DealDamage(_damage);
            }
        }
    }

}
