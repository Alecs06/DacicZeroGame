using AI;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode(name = "Tasks/Send alert")]
    public class SendAlert : Leaf
    {
        [SerializeField] protected TacticalBrain brain;
        protected Vector3? oldPos;
        [SerializeField] protected Vector3Reference vector3;
        public override void OnEnter()
        {
            base.OnEnter();
            oldPos = null;
        }
        public override NodeResult Execute()
        {
            if (oldPos == null || Vector3.Magnitude(oldPos.Value - vector3.Value) > 0.1f)
            {
                oldPos = vector3.Value;
                brain.SendAlert(oldPos.Value);
            }
            return NodeResult.running;
        }
    }
}
