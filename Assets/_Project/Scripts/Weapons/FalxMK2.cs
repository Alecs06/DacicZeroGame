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
        protected AnimancerComponent animancer;
        [SerializeField] float radius = 1, dist = 1;
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
        private float boostForce = 1500;
        private float lungeDuration = 1f;
        private float hitCheckInterval = 0.05f;
        protected override void AltFire()
        {
            StartCoroutine(repeatedAltFireAction());
        }

        private void InitialSpinAttack()
        {
            animancer.Play(clip).Time = 0;
            Collider[] colliders = new Collider[10];
            int nrOfHits = Physics.OverlapSphereNonAlloc(transform.position, 
                3*radius, 
                colliders, 
                GlobalSettings.TargetMasks[gameObject.layer]);
            HashSet<Transform> spinAttackHits = new();
            for (int i = 0; i < nrOfHits; i++)
            {
                if (!spinAttackHits.Contains(colliders[i].transform.root))
                {
                    spinAttackHits.Add(colliders[i].transform.root);
                    EventBus<TakeDamage>.Raise(colliders[i].transform.root.GetInstanceID(), new TakeDamage(1, transform.root, colliders[i]));
                }
            }
        }

        private IEnumerator repeatedAltFireAction()
        {
            InitialSpinAttack();
            yield return new WaitForSeconds(0.15f);
            BoostPlayer.Invoke(boostForce);
            animancer.Play(clip).Time = 0;

            HashSet<Transform> hitsThisLunge = new();
            float elapsed = 0f;

            while (elapsed < lungeDuration)
            {
                altFireAction(hitsThisLunge);
                yield return new WaitForSeconds(hitCheckInterval);
                elapsed += hitCheckInterval;
            }
        }

        private void altFireAction(HashSet<Transform> hits)
        {
            Collider[] colliders = new Collider[10];
            int nrOfHits = Physics.OverlapSphereNonAlloc(transform.position + dist * transform.forward, radius, colliders, GlobalSettings.TargetMasks[gameObject.layer]);

            for (int i = 0; i < nrOfHits; i++)
            {
                Transform root = colliders[i].transform.root;
                if (hits.Add(root))
                {
                    EventBus<TakeDamage>.Raise(root.GetInstanceID(), new TakeDamage(1, transform.root, colliders[i]));
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + dist * transform.forward, radius);
        }
    }
}