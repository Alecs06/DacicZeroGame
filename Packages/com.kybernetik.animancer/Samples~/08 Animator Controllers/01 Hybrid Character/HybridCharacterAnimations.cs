// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2024 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.AnimatorControllers
{
    /// <summary>
    /// Implements the same behaviour as <see cref="BasicCharacterAnimations"/>
    /// using a <see cref="HybridAnimancerComponent"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/character">
    /// Hybrid Character</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers/HybridCharacterAnimations
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Animator Controllers - Hybrid Character Animations")]
    [AnimancerHelpUrl(typeof(HybridCharacterAnimations))]
    // Awake before Animancer to disable the OptionalWarnings before it triggers them.
    [DefaultExecutionOrder(AnimancerComponent.DefaultExecutionOrder - 1000)]
    public class HybridCharacterAnimations : MonoBehaviour
    {
        /************************************************************************************************************************/

        public static readonly int IsMovingParameter = Animator.StringToHash("IsMoving");

        [SerializeField] private HybridAnimancerComponent _Animancer;
        [SerializeField] private ClipTransition _Action;

        private State _CurrentState;

        private enum State
        {
            NotActing,// Idle and Move can be interrupted.
            Acting,// Action can only be interrupted by itself.
        }

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Action.Events.OnEnd = UpdateMovement;

            // This sample's documentation explains why these warnings exist so we don't need them enabled.
            OptionalWarning.NativeControllerHumanoid.Disable();
            OptionalWarning.NativeControllerHybrid.Disable();
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            switch (_CurrentState)
            {
                case State.NotActing:
                    UpdateMovement();
                    UpdateAction();
                    break;

                case State.Acting:
                    UpdateAction();
                    break;
            }
        }

        /************************************************************************************************************************/

        private void UpdateMovement()
        {
            _CurrentState = State.NotActing;

            float forward = SampleInput.WASD.y;
            bool isMoving = forward > 0;

            // Native - Animator Controller assigned to the Animator.
            if (_Animancer.Animator.runtimeAnimatorController != null)
            {
                // Return to the Animator Controller by fading out Animancer's layers.
                AnimancerLayer layer = _Animancer.Layers[0];
                if (layer.TargetWeight > 0)
                    layer.StartFade(0, 0.25f);

                // Set parameters on the Animator compponent.
                _Animancer.Animator.SetBool(IsMovingParameter, isMoving);
            }
            // Hybrid - Animator Controller assigned to the HybridAnimancerComponent.
            else if (_Animancer.Controller.Controller != null)
            {
                // Return to the Animator Controller by playing the ControllerTransition.
                _Animancer.PlayController();

                // Set parameters on the ControllerState.
                _Animancer.SetBool(IsMovingParameter, isMoving);
            }
            else
            {
                Debug.LogError("No Animator Controller is assigned.", this);
            }
        }

        /************************************************************************************************************************/

        private void UpdateAction()
        {
            if (SampleInput.LeftMouseUp)
            {
                _CurrentState = State.Acting;
                _Animancer.Play(_Action);
            }
        }

        /************************************************************************************************************************/
    }
}
