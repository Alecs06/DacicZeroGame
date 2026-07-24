using EventBus;
using HP;
using System.Collections.Generic;
using UnityEngine;

public class SicaProjectile : Projectile
{
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private float bounceRadiusMultiplier = 1.2f;
    private float bounceCooldown = 0.05f;

    private bool isReturning = false;
    protected float explosionRadius = 5f;

    private int bounceCount = 0;
    private float lastBounceTime = -999f;

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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (isReturning) return;
        if (((1 << collision.gameObject.layer) & wallLayers) == 0) return;
        if (Time.time - lastBounceTime < bounceCooldown) return;

        Bounce(collision);
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (isReturning) return;
        if (((1 << collision.gameObject.layer) & wallLayers) == 0) return;
        if (Time.time - lastBounceTime < bounceCooldown) return;

        Vector3 normal = collision.contacts[0].normal;
        if (Vector3.Dot(ProjectileBody.linearVelocity, normal) < -0.01f)
        {
            Bounce(collision);
        }
    }

    private void Bounce(Collision collision)
    {
        float speed = ProjectileBody.linearVelocity.magnitude;
        Vector3 normal = collision.contacts[0].normal;
        Vector3 reflected = Vector3.Reflect(ProjectileBody.linearVelocity, normal).normalized * speed;

        ProjectileBody.linearVelocity = reflected;
        lastBounceTime = Time.time;

        bounceCount++;
        explosionRadius *= bounceRadiusMultiplier;
        damage += 1; // not sure

        if (bounceCount >= maxBounces)
        {
            StartReturning();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayers) == 0) return;
        if (!HitEnemies.Contains(other))
        {
            EventBus<TakeDamage>.Raise(other.transform.root.GetInstanceID(), new TakeDamage(damage, transform.root, other));
            HitEnemies.Add(other);
        }
        if (!isReturning)
        {
            StartReturning();
        }
    }

    private void StartReturning()
    {
        isReturning = true;
        ProjectileBody.linearVelocity = Vector3.zero;
        if (isEnhanced)
        {
            Explode();
        }
    }

    protected virtual void Explode()
    {
        Collider[] explosionColliders = new Collider[10];
        int nrOfHits = Physics.OverlapSphereNonAlloc(
            transform.position,
            explosionRadius,
            explosionColliders,
            GlobalSettings.TargetMasks[gameObject.layer]
        );

        HashSet<Transform> hits = new();
        for (int i = 0; i < nrOfHits; i++)
        {
            Transform root = explosionColliders[i].transform.root;

            if (HitEnemies.Contains(explosionColliders[i]))
                continue;

            if (!hits.Contains(root))
            {
                hits.Add(root);
                HitEnemies.Add(explosionColliders[i]);
                EventBus<TakeDamage>.Raise(root.GetInstanceID(), new TakeDamage(damage, transform.root, explosionColliders[i]));
            }
        }
    }
}