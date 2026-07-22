using EventBus;
using HP;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static Animancer.Easing;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class SicaMK1Projectile : Projectile
{
    private bool isReturning = false;
    protected float explosionRadius = 5f;

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
            if (isEnhanced)
            {
                Explode();
            }
        }
        if (!HitEnemies.Contains(other))
        {
            EventBus<TakeDamage>.Raise(other.transform.root.GetInstanceID(), new TakeDamage(damage, transform.root, other));
            HitEnemies.Add(other);     
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
