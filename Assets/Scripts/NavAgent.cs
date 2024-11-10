using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour
{
    
    private NavMeshAgent agent;
    public string agentnumber;
    public List<List<int>> stepsForAgent;
    private int counterForMovement = 0;

    [SerializeField] private Transform target;
    public float elapsedTime;
    public float delayTime;
    private bool isTiming;
    public bool isFinished;

    private bool isCollidingWithWall = false;
    private Vector3 lastPosition;
    private float stuckThreshold = 0.1f;
    private float stuckCheckInterval = 0.5f;
    private float lastStuckCheck;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        elapsedTime = 0f;
        isTiming = true;
        agent.speed = Parameters.carSpeed;
        lastPosition = transform.position;
        lastStuckCheck = Time.time;
    }

    // sorprendentemente, no hay metodo built-in para checar las listas de listas y si es que están vacias
    public static bool IsListEmpty(List<List<int>> listOfLists)
    {
        if (listOfLists == null || listOfLists.Count == 0)
        {
            return true;
        }

        foreach (var innerList in listOfLists)
        {
            if (innerList != null && innerList.Count > 0)
            {
                return false;
            }
        }

        return true;
    }

    void Update()
    {
        agent.SetDestination(target.position);
        if (!agent.isStopped)
        {
            // checamos si ya se tiene la instancia, si myData ya tiene datos y que no hayamos hecho esto anteriormente
            if (IntersectionSimulation.Intersection != null && IntersectionSimulation.Intersection.myData.steps != null && IsListEmpty(stepsForAgent))
            {
                // si está vacía, buscamos los pasos del agente
                if (IsListEmpty(stepsForAgent))
                {
                    if (IntersectionSimulation.Intersection.myData.steps.ContainsKey(agentnumber))
                    {
                        // está será la lista de los pasos a seguir para el agente, en donde cada elemento se ve así [x, z]
                        // si se le asigna el agente 3, guardará los pasos para el agente 3 dados por el API
                        // stepData = Intersection.Instance.myData.Steps[key];
                        Debug.Log("steps are saved into agent: " + agentnumber);
                        stepsForAgent = IntersectionSimulation.Intersection.myData.steps[agentnumber];
                    }
                    else
                    {
                        Debug.LogError("Step Key not found: " + agentnumber);
                    }
                }
            } 

            // si ya tenemos pasos y no hemos terminado con los pasos
            if (!IsListEmpty(stepsForAgent) && isFinished == false)
            {
                List<int> lastStep = stepsForAgent[stepsForAgent.Count - 1];
                Vector3 lastPosition = new Vector3(lastStep[0], agent.transform.position.y, lastStep[1]);
                agent.destination = lastPosition;
            
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    // Optionally, you can add any logic here if needed when the agent reaches the destination
                    Debug.Log("Agent has reached the final destination.");
                    isFinished = true;
                    
                }
            }
        }

        if (isTiming)
        {
            elapsedTime += Time.deltaTime;

            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                isTiming = false;
                Debug.Log("Agent reached the destination in " + elapsedTime + " seconds.");
                isFinished = true;
            }
        }

        if (Time.time - lastStuckCheck > stuckCheckInterval)
        {
            CheckIfStuck();
            lastStuckCheck = Time.time;
        }

        if (Time.time - lastStuckCheck > stuckCheckInterval)
        {
            CheckIfStuck();
            lastStuckCheck = Time.time;
        }

        if (isCollidingWithWall)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

    }

    private void CheckIfStuck()
    {
        if (Vector3.Distance(transform.position, lastPosition) < stuckThreshold)
        {
            isCollidingWithWall = false; 
        }
        lastPosition = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isCollidingWithWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isCollidingWithWall = false;
        }
    }
}