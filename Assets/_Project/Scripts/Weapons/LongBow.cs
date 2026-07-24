using Animancer;
using EventBus;
using HP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace Weapons
{
    public class LongBow: Bow
    {
        protected override void Update()
        {
            if (Firing && !AltFiring && ammo >= 1)
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
                    AltFire();
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
            ammo -= 1;
        }
        protected override void AltFire()
        {
            animancer.Play(clip).Time = 0;
            cooldownTo = Time.time + fireCooldown;
            CreateSphereAttack(2, 0f, 1);
        }
    }
}