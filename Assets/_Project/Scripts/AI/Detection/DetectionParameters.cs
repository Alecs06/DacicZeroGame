using UnityEngine;
namespace Detection
{
    /// <summary>
    /// Parameters for detection because it is a mess in the inspector.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/DetectionParams")]
    [System.Serializable]
    public class DetectionParameters : ScriptableObject
    {
        /// <summary>
        /// How often does the update loop occur?
        /// </summary>
        [field: SerializeField] public float UpdateCooldown { get; protected set; }
        /// <summary>
        /// How much time a target needs to spend undetected before it is forgotten.
        /// </summary>
        [field: SerializeField] public float TimeToForgetTarget { get; protected set; }
        /// <summary>
        /// How fast awareness increases in an update loop in which the target is detected.
        /// </summary>
        [field: SerializeField] public float AwarenessBuildRate { get; protected set; }
        /// <summary>
        /// By how much awareness decays in each update loop.
        /// </summary>
        [field: SerializeField] public float AwarenessLossRate { get; protected set; }
        /// <summary>
        /// How long does it take for the critter to forget a sound.
        /// </summary>
        [field: SerializeField] public float TimeToForgetSound { get; protected set; }
        /// <summary>
        /// How far can we hear?
        /// </summary>
        [field: SerializeField] public float AudioRange { get; protected set; }
        /// <summary>
        /// How far can we see?
        /// </summary>
        [field: SerializeField] public float VisualRange { get; protected set; }
        /// <summary>
        /// What is our field of view?
        /// </summary>
        [field: SerializeField] public float VisualAngle { get; protected set; }
        /// <summary>
        /// What can we NOT see through?
        /// </summary>
        [field: SerializeField] public LayerMask ObstructionMask { get; protected set; } = 1 << 0;
        /// <summary>
        /// What is the range on proximity detection?
        /// </summary>
        [field: SerializeField] public float ProximityRange { get; protected set; }
    }
}
