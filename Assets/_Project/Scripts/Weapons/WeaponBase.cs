using UnityEngine;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected float cooldown;
        [field: SerializeField] public bool Firing { get; set; }
        protected float timeLastShot = -1;
        protected void Update()
        {
            if (Firing)
            {
                if (Time.time - timeLastShot >= cooldown)
                {
                    timeLastShot = Time.time;
                    Fire();
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
        protected abstract void HandleNotFiring();
        protected abstract void Fire();
    }
}