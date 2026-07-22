using Animancer;
using EventBus;
using HP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Weapons
{
    public class SicaMK1 : WeaponBase
    {
        [SerializeField] Projectile projectile = new Projectile();
        [SerializeField] float projectileVelocity = 80;
        [SerializeField] protected AnimationClip clip;
        protected AnimancerComponent animancer;
        [SerializeField] float radius = 0.7f, dist = 0.7f;

        Projectile rb;
        protected void Awake()
        {
            animancer = GetComponent<AnimancerComponent>();
        }

        protected override void Fire()
        {
            animancer.Play(clip).Time = 0;
            Collider[] colliders = new Collider[10];
            int nrOfHits = Physics.OverlapSphereNonAlloc(transform.position + dist * transform.forward, radius, colliders, GlobalSettings.TargetMasks[gameObject.layer]);
            HashSet<Transform> hits = new();
            for (int i = 0; i < nrOfHits; i++)
            {
                if (!hits.Contains(colliders[i].transform.root))
                {
                    hits.Add(colliders[i].transform.root);
                    EventBus<TakeDamage>.Raise(colliders[i].transform.root.GetInstanceID(), new TakeDamage(1, transform.root, colliders[i]));
                }
            }
        }
        protected override void AltFire()
        {
            rb = Instantiate(projectile, transform.position, transform.rotation);
            rb.Owner = transform;
            rb.velocity = projectileVelocity;
            rb.damage = 1;
            rb.gameObject.SetActive(true);
            rb.OnExpire += onProjectileExpire;
        }

        private void onProjectileExpire()
        {
            // if the sica projectile returns, override the long cooldown
            Debug.Log("we are so back");
            CooldownTo = Time.time + 0.5f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + dist * transform.forward, radius);
        }
    }
}