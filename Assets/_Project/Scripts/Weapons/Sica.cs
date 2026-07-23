using Animancer;
using EventBus;
using HP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Weapons
{
    public class Sica: WeaponBase
    {
        [SerializeField] Projectile projectile;
        [SerializeField] float projectileVelocity;
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
            CreateSphereAttack(radius, dist, 1);
        }
        protected override void AltFire()
        {
            rb = Instantiate(projectile, transform.position, transform.rotation);
            rb.Owner = transform;
            rb.velocity = projectileVelocity;
            rb.damage = 1;
            rb.isEnhanced = IsEnhanced;
            rb.gameObject.SetActive(true);
            rb.OnExpire += onProjectileExpire;
        }

        private void onProjectileExpire()
        {
            // if the sica projectile returns, override the long cooldown
            cooldownTo = Time.time + 0.5f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + dist * transform.forward, radius);
        }
    }
}