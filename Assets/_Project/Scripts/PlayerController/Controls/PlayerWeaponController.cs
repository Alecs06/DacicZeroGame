using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapons;
namespace PlayerController
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] Rigidbody PlayerBody;
        [SerializeField] protected InputReader inputReader;
        [SerializeField] protected List<WeaponBase> weapons = new();
        protected int selectedWeaponIndex = 2;

        private void OnEnable()
        {
            inputReader.Fire += OnFire;
            inputReader.AltFire += OnAltFire;
            inputReader.SwitchWeapon += OnSwitchWeapon;
            weapons[selectedWeaponIndex].boostPlayer += OnBoostPlayer;
        }
        private void OnDisable()
        {
            inputReader.Fire -= OnFire;
            inputReader.AltFire -= OnAltFire;
            inputReader.SwitchWeapon -= OnSwitchWeapon;
            weapons[selectedWeaponIndex].boostPlayer -= OnBoostPlayer;
        }
        /// <summary>
        /// Takes in an input context for a certain selectedWeaponIndex. Displays an error log if the selectedWeaponIndex is not found.
        /// </summary>
        /// <param name="context">The input context that the selectedWeaponIndex must receive.</param>
        /// <param name="weapon">The selectedWeaponIndex that must receive the input.</param>
        protected void OnFire(InputAction.CallbackContext context)
        {
            if (selectedWeaponIndex < 0 || selectedWeaponIndex >= weapons.Count)
            {
                Debug.LogError($"{transform} attempted to use nonexistant selectedWeaponIndex {selectedWeaponIndex}.");
                return;
            }
            if (context.ReadValue<float>() == 1)
            {
                weapons[selectedWeaponIndex].Firing = true;
            }
            else
            {
                weapons[selectedWeaponIndex].Firing = false;
            }
        }
        protected void OnAltFire(InputAction.CallbackContext context)
        {
            if (selectedWeaponIndex < 0 || selectedWeaponIndex >= weapons.Count)
            {
                Debug.LogError($"{transform} attempted to use nonexistant selectedWeaponIndex {selectedWeaponIndex}.");
                return;
            }
            if (context.ReadValue<float>() == 1)
            {
                weapons[selectedWeaponIndex].AltFiring = true;
            }
            else
            {
                weapons[selectedWeaponIndex].AltFiring = false;
            }
        }

        protected void OnBoostPlayer(float velocity)
        {
            PlayerBody.AddForce(transform.forward * velocity);
        }
        protected void OnSwitchWeapon(int weaponNumber)
        {
            selectedWeaponIndex = weaponNumber;
        }
    }
}