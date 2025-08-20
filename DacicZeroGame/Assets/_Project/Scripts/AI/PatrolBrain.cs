using UnityEngine;

public class PatrolBrain : MonoBehaviour
{
    public Transform[] PatrolPoints;
    private void OnDrawGizmos()
    {
        if (PatrolPoints.Length > 1)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < PatrolPoints.Length - 1; i++)
            {
                if (PatrolPoints[i] != null && PatrolPoints[i + 1] != null)
                {
                    Gizmos.DrawLine(PatrolPoints[i].position, PatrolPoints[i + 1].position);
                }
            }
            Gizmos.DrawLine(PatrolPoints[PatrolPoints.Length - 1].position, PatrolPoints[0].position);
        }
    }
}
