using UnityEngine;
using UnityEngine.AI;
using Weapons;
namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Shoot gun")]
    public class ShootGun : Leaf
    {
        [SerializeField] BotGun gun;
        [SerializeField] Vector3Reference target;
        [SerializeField] NavMeshAgent agent;
        [SerializeField] Transform self;
        public override void OnEnter()
        {
            base.OnEnter();
            //start firing
            gun.Firing = true;
        }
        public override NodeResult Execute()
        {
            //make the bot look at the target
            self.LookAt(new Vector3(target.Value.x, self.position.y, target.Value.z));
            //aim the gun at the target
            gun.transform.LookAt(target.Value);
            return NodeResult.running;
        }
        public override void OnExit()
        {
            base.OnExit();
            //stop firing
            gun.Firing = false;
        }
    }
}
