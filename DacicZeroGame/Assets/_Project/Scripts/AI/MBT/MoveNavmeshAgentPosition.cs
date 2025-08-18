using MBT;
using UnityEngine;
using UnityEngine.AI;

namespace MBTExample
{
    /// <summary>
    /// Literally just the Vector3 version of MoveNavmeshAgent.
    /// </summary>
    [AddComponentMenu("")]
    [MBTNode("Tasks/Move Navmesh Agent To Position")]
    public class MoveNavmeshAgentPosition : Leaf
    {
        public Vector3Reference destination;
        public NavMeshAgent agent;
        [SerializeField] protected float stopDistance = 2;
        /// <summary>
        /// If the difference between the target position and our current destination
        /// is greater than this, recalculate path.
        /// </summary>
        [SerializeField] protected float maxError = 0.1f;

        public override void OnEnter()
        {
            agent.isStopped = false;
            agent.SetDestination(destination.Value);
        }

        public override NodeResult Execute()
        {
            //calculate difference between the target position and our current destination
            float error = Vector3.Magnitude(destination.Value - agent.destination);
            if (error > maxError)
            {
                agent.SetDestination(destination.Value);
            }

            // Check if path is ready
            if (agent.pathPending)
            {
                return NodeResult.running;
            }
            // Check if agent is very close to destination
            if (agent.remainingDistance < stopDistance)
            {
                return NodeResult.success;
            }
            // Check if there is any path (if not pending, it should be set)
            if (agent.hasPath)
            {
                return NodeResult.running;
            }
            // By default return failure
            return NodeResult.failure;
        }

        public override void OnExit()
        {
            agent.isStopped = true;
            // agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(destination.isInvalid || agent == null);
        }
    }
}