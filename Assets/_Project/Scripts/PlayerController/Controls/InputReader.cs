using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;
namespace PlayerController
{
    public interface IInputReader
    {
        Vector2 Direction { get; }
        void EnablePlayerActions();
    }

    [CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/PlayerController/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions, IInputReader
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2> Look = delegate { };
        public event UnityAction Jump = delegate { };
        public event UnityAction Interact = delegate { };
        public event UnityAction<bool> Crouch = delegate { };
        public event UnityAction<bool> Sprint = delegate { };
        public event UnityAction<InputAction.CallbackContext> Fire = delegate { };
        public event UnityAction<InputAction.CallbackContext> AltFire = delegate { };
        public PlayerControls inputActions;
        public Vector2 Direction => inputActions.Player.Move.ReadValue<Vector2>();
        public Vector2 LookDirection => inputActions.Player.Look.ReadValue<Vector2>();
        public string InteractKey => inputActions.Player.Interact.GetBindingDisplayString();
        public void EnablePlayerActions()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
            }
            inputActions.Player.SetCallbacks(this);
            inputActions.Enable();
        }
        public void DisablePlayerActions()
        {
            if (inputActions != null)
            {
                inputActions.Player.SetCallbacks(null);
                inputActions.Disable();
            }
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>());
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Jump.Invoke();
            }
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Interact.Invoke();
            }
        }
        public void OnFire(InputAction.CallbackContext context)
        {
            Fire.Invoke(context);
        }

        public void OnAltFire(InputAction.CallbackContext context)
        {
            AltFire.Invoke(context);
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started)
                Crouch.Invoke(true);
            else if (context.canceled)
                Crouch.Invoke(false);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started)
                Sprint.Invoke(true);
            else if (context.canceled)
                Sprint.Invoke(false);
        }
    }
}