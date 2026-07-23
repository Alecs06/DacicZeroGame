using EventBus;
using HP;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected float fireCooldown;
        [SerializeField] protected float altFireCooldown;
        [SerializeField] public bool Firing { get; set; }
        [SerializeField] public bool AltFiring { get; set; }

        protected float cooldownTo = -1;
        protected float timeLastShot = -1;
        public UnityAction<float> BoostPlayer = delegate { };

        protected Renderer[] modelRenderers;

        protected Renderer[] ModelRenderers
        {
            get
            {
                if (modelRenderers == null)
                    modelRenderers = GetComponentsInChildren<Renderer>();
                return modelRenderers;
            }
        }

        protected virtual void Update()
        {
            if (Firing)
            {
                if (Time.time >= cooldownTo)
                {
                    cooldownTo = Time.time + fireCooldown;
                    Fire();
                }
            }
            else if (AltFiring)
            {
                if (Time.time >= cooldownTo)
                {
                    cooldownTo = Time.time + altFireCooldown;
                    AltFire();
                }
            }
            else
            {
                HandleNotFiring();
            }
        }

        protected virtual void OnEnable()
        {
            Firing = false;
        }

        protected virtual void HandleNotFiring() { }
        protected abstract void Fire();
        protected virtual void AltFire() { }

        public virtual void SetModelVisible(bool visible)
        {
            foreach (var renderer in ModelRenderers)
            {
                if (renderer != null)
                    renderer.enabled = visible;
            }
        }
#nullable enable
        /// <summary>
        /// Standardasied way of creating a damage field/attack.
        /// </summary>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="dist">Distance at which the sphere should spawn in front of the player</param>
        /// <param name="damage">Damage dealt</param>
        /// <param name="hits">Used for multi-phase attacks, keeps track of already hit reference, leave null if its a 1 phase attack.</param>
        public void CreateSphereAttack(float radius, float dist, int damage,HashSet<Transform>? hits = null)
        {
            hits ??= new HashSet<Transform>();
            Collider[] colliders = new Collider[10];
            int nrOfHits = Physics.OverlapSphereNonAlloc(transform.position + dist * transform.forward, radius, colliders, GlobalSettings.TargetMasks[gameObject.layer]);
            for (int i = 0; i < nrOfHits; i++)
            {
                if (!hits.Contains(colliders[i].transform.root))
                {
                    hits.Add(colliders[i].transform.root);
                    EventBus<TakeDamage>.Raise(colliders[i].transform.root.GetInstanceID(), new TakeDamage(damage, transform.root, colliders[i]));
                }
            }
        }
#nullable disable
    }
}