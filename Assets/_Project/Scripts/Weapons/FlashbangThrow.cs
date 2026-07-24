using UnityEngine;
namespace Weapons
{
    public class FlashbangThrow : WeaponBase
    {
        [SerializeField] Projectile projectile;
        [SerializeField] int ammo = 1;
        [SerializeField] float projectileVelocity = 40f;

        protected override void Fire()
        {
            if (ammo > 0)
            {
                ammo -= 1;
                var rb = Instantiate(projectile, transform.position, transform.rotation);
                rb.Owner = transform;
                rb.velocity = projectileVelocity;
                rb.damage = 1;
                rb.gameObject.SetActive(true);
            }
        }
    }
}
