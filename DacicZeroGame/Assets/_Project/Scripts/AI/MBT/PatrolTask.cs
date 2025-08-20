using UnityEngine;
using UnityEngine.AI;
namespace MBT
{

    [AddComponentMenu("")]
    [MBTNode("Tasks/Patrol or Idle")]
    public class PatrolTask : Leaf
    {
        [SerializeField] NavMeshAgent agent;
        [SerializeField] Transform[] patrolPoints;
        [SerializeField] int currentPatrolPoint;
        public override void OnEnter()
        {
            base.OnEnter();
            agent.isStopped = false;
            if (patrolPoints.Length > 0)
            {
                currentPatrolPoint = 0;
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);

            }
        }
        public override NodeResult Execute()
        {
            if (patrolPoints.Length > 1)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
                    agent.SetDestination(patrolPoints[currentPatrolPoint].position);
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