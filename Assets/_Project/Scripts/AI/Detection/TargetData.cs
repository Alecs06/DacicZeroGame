using EventBus;
using UnityEngine;
namespace Detection
{
    public struct SoundEvent : IEvent
    {
        /// <summary>
        /// How loud was the sound;
        /// </summary>
        public float Intensity;
        /// <summary>
        /// Where did the sound originate?
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// Use this so AI don't constantly investigate sounds made by their buddies.
        /// </summary>
        public int Team;
        public SoundEvent(float intensity, Vector3 position, int team)
        {
            Intensity = intensity;
            Position = position;
            Team = team;
        }
    }
    [System.Serializable]
    public struct SoundData
    {
        /// <summary>
        /// How loud was the sound;
        /// </summary>
        public float Intensity;
        /// <summary>
        /// Where did the sound originate?
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// Use this so AI don't constantly investigate sounds made by their buddies.
        /// </summary>
        public int Team;
        /// <summary>
        /// When did we hear this sound?
        /// </summary>
        public float TimeHeard;
        public SoundData(float intensity, Vector3 position, int team)
        {
            Intensity = intensity;
            Position = position;
            Team = team;
            TimeHeard = Time.time;
        }
        public SoundData(SoundEvent @event)
        {
            Intensity = @event.Intensity;
            Position = @event.Position;
            Team = @event.Team;
            TimeHeard = Time.time;
        }
    }
    [System.Serializable]
    public class TargetData
    {
        protected float awareness;
        /// <summary>
        /// Increases when the target is spotted, decreases over time.
        /// </summary>
        public float Awareness
        {
            get
            {
                return awareness;
            }
            set
            {
                awareness = Mathf.Clamp(value, 0, 1);
            }
        }
        /// <summary>
        /// When was this target last spotted?
        /// </summary>
        public float TimeLastSpotted;
        public Vector3 LastKnownPosition;
        public Transform Transform;
        public TargetData(Transform tr, float awareness = 0.5f)
        {
            Transform = tr;
            LastKnownPosition = tr.position;
            TimeLastSpotted = Time.time;
            this.awareness = awareness;
        }
    }
}