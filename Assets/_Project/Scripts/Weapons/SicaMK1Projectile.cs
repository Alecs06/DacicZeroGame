using EventBus;
using HP;
using System.Linq;
using UnityEngine;

public class SicaMK1Projectile : Projectile
{
    private bool isReturning = false;

    protected override void FixedUpdate()
    {
        if (isReturning)
        {
            Vector3 targetPos = owner.position;
            ProjectileBody.MovePosition(Vector3.MoveTowards(transform.position, targetPos, velocity * Time.fixedDeltaTime));
            if (Vector3.Distance(ProjectileBody.position, owner.position) <= 3)
            {
                Destroy(gameObject);
            }
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!isReturning)
        {
            isReturning = true;
            ProjectileBody.linearVelocity = Vector3.zero;
        }
        if (!HitEnemies.Contains(other))
        {
            EventBus<TakeDamage>.Raise(other.transform.root.GetInstanceID(), new TakeDamage(damage, transform.root, other));
            HitEnemies.Append(other);     
        }
    }
}
