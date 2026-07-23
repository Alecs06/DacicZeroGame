using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class FalxMK1 : WeaponBase
    {
        [SerializeField] protected AnimationClip clip;
        [SerializeField] protected AnimationClip pokeClip;
        protected AnimancerComponent animancer;

        [SerializeField] float radius = 1, dist = 1;

        private float boostForce = 1500;
        private float lungeDuration = 1f;
        private float hitCheckInterval = 0.05f;

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
            StartCoroutine(RepeatedAltFireAction());
        }

        private IEnumerator RepeatedAltFireAction()
        {
            BoostPlayer.Invoke(boostForce);
            animancer.Stop();
            animancer.Play(pokeClip).Time = 0;

            HashSet<Transform> hitsThisLunge = new();

            float elapsed = 0f;
            while (elapsed < lungeDuration)
            {
                CreateSphereAttack(radius, dist, 1, hitsThisLunge);

                yield return new WaitForSeconds(hitCheckInterval);
                elapsed += hitCheckInterval;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + dist * transform.forward, radius);
        }
    }
}