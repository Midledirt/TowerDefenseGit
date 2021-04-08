using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSplashDamage : MonoBehaviour
{
    [SerializeField] private float splashDamageRadius = 5f;
    [SerializeField] private LayerMask whatCanBeTargeted;
    [Tooltip("The minimum damage dealth regardless of distance to target")]
    [SerializeField] private float minimumDamage = 5f;
    public float MinimumDamage { get; private set; }
    [Tooltip("Set how quickly the damage will be reduced based on distance")]
    [SerializeField] private int damageFallOff;

    private void Start()
    {
        MinimumDamage = minimumDamage;
    }
    public void DealSplashDamage(Vector3 _location, float _damage)
    {
        //Check if "we" are the referenced spash damage instance, find out where to cast the explosion
        print("Dealing splash damage!");
        Collider[] Colliders = Physics.OverlapSphere(_location, splashDamageRadius, whatCanBeTargeted);
        foreach(Collider _collider in Colliders)
        {
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
