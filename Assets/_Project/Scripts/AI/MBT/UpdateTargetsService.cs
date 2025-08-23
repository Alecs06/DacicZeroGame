using AI;
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
        [SerializeField] protected BoolReference hasPosition;
        [SerializeField] protected FloatReference targetAwareness;
        [SerializeField] protected TacticalBrain tacticalBrain;
        public override void Task()
        {
            if (detectionSystem)
            {
                if (detectionSystem.ClosestTarget != null)
                {
                    //targets have priority over sounds; we will prioritize going
                    //after them over investigating noises
                    position.Value = detectionSystem.ClosestTarget.LastKnownPosition;
                    targetAwareness.Value = detectionSystem.ClosestTarget.Awareness;
                    hasPosition.Value = true;
                    detectionSystem.transform.LookAt(position.Value);
                    return;
                }
                //reset target awareness if we do not have a target
                targetAwareness.Value = 0;
                if (tacticalBrain.RequestedPosition != null)
                {
                    position.Value = tacticalBrain.RequestedPosition.Value;
                    hasPosition.Value = true;
                    return;
                }
                if (detectionSystem.ClosestSound != null)
                {
                    position.Value = detectionSystem.ClosestSound.Value.Position;
                    detectionSystem.transform.LookAt(position.Value);
                    hasPosition.Value = true;
                    return;
                }
                hasPosition.Value = false;
                return;
            }
            Debug.LogError($"{this} has no detection system set.");
        }
    }
}