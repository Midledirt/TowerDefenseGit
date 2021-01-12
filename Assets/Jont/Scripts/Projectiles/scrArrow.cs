using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles the movement of the arrow projectile
/// This class derives from the scrMageBoltProjectile. At least for now. They share a lot of common logic.
/// </summary>

public class scrArrow : scrMageBoltProjectile // inherit from this as these will be very similar
{
    public Vector3 Direction { get; set; } //This "Direction" takes its input from the "scrTpwerArrowsProjectileLoader".FireProjectile

    protected override void Update()
    {
        /*if (_creepTarget != null)
        {
            MovePorjectile();
        }*/
        MovePorjectile(); //OK. So this is the first problem. The projectile only moves whilst it has a target. And it seems to lose the target/never get one
    
    }

    protected override void MovePorjectile()
    {
        Vector3 movement = Direction.normalized * movementSpeed * Time.deltaTime; //Direction does not work... Hmmm
        transform.Translate(movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creep"))
        {
            Creep creep = other.GetComponent<Creep>();
            if (creep._CreepHealth.currentHealth > 0f)
            {
                creep._CreepHealth.DealDamage(damage);
            }

            ObjectPooler.ReturnToPool(gameObject);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ObjectPooler.ReturnToPoolWithDelay(gameObject, 5f)); //Make the object "despawn" if it does not hit anything before a certain time
    }
}
