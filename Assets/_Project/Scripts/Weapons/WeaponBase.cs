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
        protected float CooldownTo = -1;
        protected float timeLastShot = -1;
        public UnityAction<float> boostPlayer = delegate { };
        protected virtual void Update()
        {
            if (Firing)
            {
                if (Time.time >= CooldownTo)
                {
                    CooldownTo = Time.time + fireCooldown;
                    Fire();
                }
            }
            else if (AltFiring)
            {
                if (Time.time >= CooldownTo)
                {
                    CooldownTo = Time.time + altFireCooldown;
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
    }
}