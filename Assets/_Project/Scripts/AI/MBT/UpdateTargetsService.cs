using Detection;
using UnityEngine;
namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Service/Update Targets Service")]
    public class UpdateTargetsService : Service
    {
        [SerializeField] protected DetectionSystem detectionSystem;
        [SerializeField] protected Vector3Reference position;
        [SerializeField] protected BoolReference hasTarget, hasSound;
        [SerializeField] protected FloatReference targetAwareness;
        public override void Task()
        {
            if (detectionSystem)
            {
                hasTarget.Value = detectionSystem.ClosestTarget != null;
                hasSound.Value = detectionSystem.ClosestSound != null;
                if (hasTarget.Value)
                {
                    //targets have priority over sounds; we will prioritize going
                    //after them over investigating noises
                    position.Value = detectionSystem.ClosestTarget.LastKnownPosition;
                    targetAwareness.Value = detectionSystem.ClosestTarget.Awareness;
                    return;
                }
                //reset target awareness if we do not have a target
                targetAwareness.Value = 0;
                if (hasSound.Value)
                {
                    position.Value = ((SoundData)detectionSystem.ClosestSound).Position;
                }
                return;
            }
            Debug.LogError($"{this} has no detection system set.");
        }
    }
}