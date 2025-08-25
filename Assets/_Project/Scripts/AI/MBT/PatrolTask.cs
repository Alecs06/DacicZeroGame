using AI;
using UnityEngine;
using UnityEngine.AI;
namespace MBT
{

    [AddComponentMenu("")]
    [MBTNode("Tasks/Patrol or Idle")]
    public class PatrolTask : Leaf
    {
        [SerializeField] NavMeshAgent agent;
        [SerializeField] TacticalBrain brain;
        [SerializeField] int currentPatrolPoint = 0;
        public override void OnEnter()
        {
            base.OnEnter();
            agent.isStopped = false;
            if (brain.PatrolPoints != null && brain.PatrolPoints.Length > 0)
            {
                agent.SetDestination(brain.PatrolPoints[currentPatrolPoint].position);
            }
        }
        public override NodeResult Execute()
        {
            if (brain.PatrolPoints != null && brain.PatrolPoints.Length > 1)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentPatrolPoint = (currentPatrolPoint + 1) % brain.PatrolPoints.Length;
                    agent.SetDestination(brain.PatrolPoints[currentPatrolPoint].position);
                }
            }
            return NodeResult.running;
        }

        public override void OnExit()
        {
            base.OnExit();
            agent.isStopped = true;
        }
    }
}