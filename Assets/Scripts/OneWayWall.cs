using UnityEngine;
using UnityEngine.AI;

public class OneWayWall : MonoBehaviour
{
    public enum PassableDirection
    {
        PositiveX,
        NegativeX,
        PositiveZ,
        NegativeZ
    }

    public PassableDirection passableDirection;
    public float pushForce = 10f;

    private Vector3 GetPassableNormal()
    {
        switch (passableDirection)
        {
            case PassableDirection.PositiveX:
                return Vector3.right;
            case PassableDirection.NegativeX:
                return Vector3.left;
            case PassableDirection.PositiveZ:
                return Vector3.forward;
            case PassableDirection.NegativeZ:
                return Vector3.back;
            default:
                return Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // Assuming the NavAgent is on the Player
        {
            NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                Vector3 collisionNormal = collision.contacts[0].normal;
                Vector3 passableNormal = GetPassableNormal();

                if (Vector3.Dot(collisionNormal, passableNormal) > 0)
                {
                    // Agent is trying to pass from the correct side
                    Vector3 pushDirection = passableNormal;
                    agent.Move(pushDirection * pushForce * Time.deltaTime);
                    
                    // Temporarily disable the NavMeshAgent's obstacle avoidance
                    float originalRadius = agent.radius;
                    agent.radius = 0.01f;
                    
                    // Re-enable the original radius after a short delay
                    StartCoroutine(ResetAgentRadius(agent, originalRadius));
                }
            }
        }
    }

    private System.Collections.IEnumerator ResetAgentRadius(NavMeshAgent agent, float originalRadius)
    {
        yield return new WaitForSeconds(0.5f);  // Adjust this delay as needed
        agent.radius = originalRadius;
    }
}