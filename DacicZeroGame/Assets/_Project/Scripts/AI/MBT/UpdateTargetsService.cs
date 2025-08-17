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
        public override void Task()
        {
            if (detectionSystem)
            {
                hasTarget.Value = detectionSystem.ClosestTarget != null;
                if (hasTarget.Value)
                {
                    position.Value = detectionSystem.ClosestTarget.LastKnownPosition;
                    return;
                }
                hasSound.Value = detectionSystem.ClosestSound != null;
                if (hasSound.Value)
                {
                    position.Value = detectionSystem.ClosestSound.Position;
                }
                return;
            }
            Debug.LogError($"{this} has no detection system set.");
        }
    }
}