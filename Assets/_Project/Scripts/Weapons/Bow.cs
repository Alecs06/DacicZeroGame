using Animancer;
using EventBus;
using HP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace Weapons
{
    public class Bow: WeaponBase
    {
        [SerializeField] Projectile projectile;
        [SerializeField] float projectileVelocity;
        [SerializeField] protected AnimationClip clip;
        protected AnimancerComponent animancer;
        float maxCharge = 100;
        [SerializeField] float chargeTime = 0.7f;
        float chargeIncrement = -1f;
        float currentCharge = 0;
        string chargeType = string.Empty;

        Projectile rb;
        protected void Awake()
        {
            animancer = GetComponent<AnimancerComponent>();
        }
        void Start()
        {
            chargeIncrement = maxCharge / chargeTime;
        }

        protected override void Update()
        {
            if (Firing && !AltFiring)
            {
                if (Time.time >= cooldownTo)
                {
                    currentCharge += chargeIncrement * Time.deltaTime;
                    currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
                    chargeType = "fire";
                }
            }
            else if (AltFiring && !Firing)
            {
                if (Time.time >= cooldownTo)
                {
                    currentCharge += chargeIncrement * Time.deltaTime;
                    currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
                    chargeType = "altfire";
                }
            }
            else if (AltFiring && Firing)
            {
                currentCharge = 0;

            }
            else
            {
                if (currentCharge > 30)
                {
                    if (chargeType == "fire") Fire();
                    else AltFire();
                }
                currentCharge = 0;
                HandleNotFiring();
            }
        }

        protected override void Fire()
        {
            animancer.Play(clip).Time = 0;

            cooldownTo = Time.time + fireCooldown;
            shootArrow(currentCharge);
        }
        protected override void AltFire()
        {
            const float SIDE_ARROW_YAW_OFFSET = 7.5f; 

            animancer.Play(clip).Time = 0;

            cooldownTo = Time.time + altFireCooldown;

            shootArrow(currentCharge);
            shootArrow(currentCharge, SIDE_ARROW_YAW_OFFSET);
            shootArrow(currentCharge, SIDE_ARROW_YAW_OFFSET * (-1));
        }

        protected void shootArrow(float charge, float yawOffset = 0)
        {
            rb = Instantiate(projectile, transform.position, transform.rotation);
            rb.Owner = transform;
            rb.velocity = projectileVelocity * charge / maxCharge;
            rb.damage = 2 * (int)(charge / maxCharge);
            rb.hasGravity = true;
            rb.yawOffset = yawOffset;
            rb.gameObject.SetActive(true);
        }
    }
}