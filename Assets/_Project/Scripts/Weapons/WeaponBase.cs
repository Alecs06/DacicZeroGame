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
    }
}