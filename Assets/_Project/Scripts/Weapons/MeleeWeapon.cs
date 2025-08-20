using Animancer;
using EventBus;
using HP;
using System.Collections.Generic;
using UnityEngine;
namespace Weapons
{
    public class MeleeWeapon : WeaponBase
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
            Debug.Log(nrOfHits);
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
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + dist * transform.forward, radius);
        }

        protected override void HandleNotFiring()
        {

        }
    }
}