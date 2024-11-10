using UnityEngine;
using System.Collections;
using System.IO;

public class Patrol : MonoBehaviour
{
    public Transform[] points; // Puntos A, B, C, D
    private int destPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    private bool isCrossing = false;
    private bool hasReachedEnd = false; // Nueva variable para verificar si se ha llegado al final
    private string logFilePath;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Asignar los puntos programáticamente
        points = new Transform[4];
        points[0] = GameObject.Find("PointA")?.transform;
        points[1] = GameObject.Find("PointB")?.transform;
        points[2] = GameObject.Find("PointC")?.transform;
        points[3] = GameObject.Find("PointD")?.transform;

        // Verifica si los puntos están correctamente asignados
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] == null)
            {
                Debug.LogError("Point at index " + i + " is null. Please check your assignments.");
            }
            else
            {
                Debug.Log("Point " + i + " assigned: " + points[i].name);
            }
        }

        // Define the path for the log file in Documents folder
        logFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "PatrolLog.txt");

        // Clear any existing log
        File.WriteAllText(logFilePath, "Patrol Log:\n");

        // Desactiva el frenado automático para un movimiento continuo
        agent.autoBraking = false;

        // Start patrolling
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Retorna si no hay puntos configurados, si el NPC está cruzando, o si ha llegado al final
        if (points.Length == 0 || isCrossing || hasReachedEnd)
            return;

        // Verifica si el siguiente punto es válido
        if (points[destPoint] == null)
        {
            Debug.LogError("Point at index " + destPoint + " is null.");
            return;
        }

        // Establece el destino del agente
        agent.destination = points[destPoint].position;
        Debug.Log("Agent moving to point: " + points[destPoint].name);
    }

    void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning("Agent is not on a NavMesh or is not active.");
            return;
        }

        // Verifica si el agente está cerca del destino y no ha llegado al final
        if (!isCrossing && !hasReachedEnd && agent.remainingDistance < 0.5f)
        {
            LogReachedPoint("Reached point: " + points[destPoint].name);
            // Solo avanza al siguiente punto si no es el último
            if (destPoint < points.Length - 1)
            {
                destPoint = (destPoint + 1) % points.Length;
                Debug.Log("Next destination point index: " + destPoint);
                GotoNextPoint();
            }
            else
            {
                hasReachedEnd = true;
                LogReachedPoint("Reached the last point: " + points[destPoint].name);
                agent.isStopped = true;
                agent.velocity = Vector3.zero; // Asegúrate de que la velocidad sea 0
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si el agente entra en una zona de cruce
        if (other.CompareTag("Crosswalk"))
        {
            isCrossing = true;
            agent.destination = other.transform.position; // Dirige al NPC al centro del cruce
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el agente sale de una zona de cruce
        if (other.CompareTag("Crosswalk"))
        {
            isCrossing = false;
            GotoNextPoint(); // Continúa hacia el siguiente punto después de cruzar
        }
    }

    void LogReachedPoint(string message)
    {
        // Log the message to the console
        Debug.Log(message);

        // Append the message to the log file
        try
        {
            File.AppendAllText(logFilePath, message + "\n");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to write to log file: " + e.Message);
        }
    }

    void OnDrawGizmos()
    {
        if (points.Length > 0)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                if (points[i] != null && points[i + 1] != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
                }
            }
        }
    }
}
