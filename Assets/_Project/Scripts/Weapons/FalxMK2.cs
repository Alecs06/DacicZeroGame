using Animancer;
using EventBus;
using HP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Weapons
{
    public class FalxMK2 : WeaponBase
    {
        [SerializeField] protected AnimationClip clip;
        [SerializeField] protected AnimationClip pokeClip;
        protected AnimancerComponent animancer;
        [SerializeField] float radius = 1, dist = 1;
        protected void Awake()
        {
            animancer = GetComponent<AnimancerComponent>();
        }

        protected override void Fire()
        {
            animancer.Play(clip).Time = 0;
            CreateSphereAttack(radius, dist, 1);
        }
        private float boostForce = 1500;
        private float lungeDuration = 1f;
        private float hitCheckInterval = 0.05f;
        protected override void AltFire()
        {
            StartCoroutine(RepeatedAltFireAction());
        }

        private void InitialSpinAttack()
        {
            animancer.Play(clip).Time = 0;
            CreateSphereAttack(3 * radius, 0f, 1);
        }

        private IEnumerator RepeatedAltFireAction()
        {
            InitialSpinAttack();

            yield return new WaitForSeconds(0.15f);

            BoostPlayer.Invoke(boostForce);
            animancer.Play(pokeClip).Time = 0;

            HashSet<Transform> hitsThisLunge = new();

            float elapsed = 0f;
            while (elapsed < lungeDuration)
            {
                CreateSphereAttack(radius, dist, 1, hitsThisLunge);

                yield return new WaitForSeconds(hitCheckInterval);
                elapsed += hitCheckInterval;
            }
            if (IsEnhanced)
            {
                CreateSphereAttack(5, 0, 1);
            }
        }
    }
}