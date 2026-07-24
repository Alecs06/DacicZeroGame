using Animancer;
using EventBus;
using HP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace Weapons
{
    public class RapidFireBow: Bow
    {
        protected override void Update()
        {
            if (Firing && !AltFiring && ammo >= 1)
            {
                if (Time.time >= cooldownTo)
                {
                    cooldownTo = Time.time + fireCooldown;
                    Fire();
                }
            }
            else if (AltFiring && !Firing && ammo >= 1)
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
            shootArrow(maxCharge);
            ammo -=1;
        }
        protected override void AltFire()
        {
            cooldownTo = Time.time + fireCooldown;
            StartCoroutine(RepeatedAltFireAction(currentCharge));
        }
        private IEnumerator RepeatedAltFireAction(float charge)
        {
            const float RAPID_FIRE_TIME_BETWEEN_ARROWS = 0.1f;

            animancer.Play(clip).Time = 0;
            cooldownTo = Time.time + altFireCooldown;

            for (int i = 0; i < 5; i++)
            {
                shootArrow(charge);
                yield return new WaitForSeconds(RAPID_FIRE_TIME_BETWEEN_ARROWS);
                ammo -= 1;
                if (ammo <= 0) break;
            }
            

        }
    }
}