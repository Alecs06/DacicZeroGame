using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;
namespace PlayerController
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] protected InputReader inputReader;
        [SerializeField] protected List<WeaponBase> weapons = new();
        private void OnEnable()
        {
            inputReader.Weapon += OnWeapon;
        }
        private void OnDisable()
        {
            inputReader.Weapon -= OnWeapon;
        }
        /// <summary>
        /// Takes in an input context for a certain weapon. Displays an error log if the weapon is not found.
        /// </summary>
        /// <param name="context">The input context that the weapon must receive.</param>
        /// <param name="weapon">The weapon that must receive the input.</param>
        protected void OnWeapon(InputAction.CallbackContext context, int weapon)
        {
            if (weapon < 0 || weapon >= weapons.Count)
            {
                Debug.LogError($"{transform} attempted to use nonexistant weapon {weapon}.");
                return;
            }
            if (context.ReadValue<float>() == 1)
            {
                weapons[weapon].Firing = true;
            }
            else
            {
                weapons[weapon].Firing = false;
            }
        }
    }
}