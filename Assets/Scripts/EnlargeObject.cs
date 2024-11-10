using UnityEngine;
using UnityEngine.ProBuilder;
using System.Collections.Generic;
using System.Linq;

public class EnlargeObject : MonoBehaviour
{
    public float widthFactor = 1.1f;
    public float lengthFactor = 1.1f;
    public float heightFactor = 1.1f;
    public float uniformFactor = 1.1f;

    public KeyCode widthKey = KeyCode.X;
    public KeyCode lengthKey = KeyCode.Y;
    public KeyCode heightKey = KeyCode.Z;
    public KeyCode uniformKey = KeyCode.U;

    private ProBuilderMesh pbMesh;

    private void Start()
    {
        pbMesh = GetComponent<ProBuilderMesh>();
        if (pbMesh == null)
        {
            Debug.LogError("ProBuilderMesh component not found on this object!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(widthKey))
        {
            ResizeObjectInPlace(widthFactor, 1f, 1f);
        }
        else if (Input.GetKeyDown(lengthKey))
        {
            ResizeObjectInPlace(1f, lengthFactor, 1f);
        }
        else if (Input.GetKeyDown(heightKey))
        {
            ResizeObjectInPlace(1f, 1f, heightFactor);
        }
        else if (Input.GetKeyDown(uniformKey))
        {
            ResizeObjectInPlace(uniformFactor, uniformFactor, uniformFactor);
        }
    }

    private void ResizeObjectInPlace(float xFactor, float yFactor, float zFactor)
    {
        if (pbMesh != null)
        {
            IList<Vector3> verticesList = pbMesh.positions;
            Vector3[] vertices = verticesList.ToArray();
            Vector3 center = CalculateCenter(vertices);

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 directionFromCenter = vertices[i] - center;
                directionFromCenter.x *= xFactor;
                directionFromCenter.y *= yFactor;
                directionFromCenter.z *= zFactor;
                vertices[i] = center + directionFromCenter;
            }

            pbMesh.positions = new List<Vector3>(vertices);
            pbMesh.ToMesh();
            pbMesh.Refresh();

            Debug.Log($"Object resized: Width:{xFactor}, Length:{yFactor}, Height:{zFactor}");
        }
    }

    private Vector3 CalculateCenter(Vector3[] vertices)
    {
        if (vertices.Length == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (Vector3 vertex in vertices)
        {
            sum += vertex;
        }
        return sum / vertices.Length;
    }
}

//CHATGPTO

// using UnityEngine;
// using UnityEngine.ProBuilder;
// using System.Collections.Generic;
// using System.Linq;

// public class EnlargeObject : MonoBehaviour
// {
//     public float lengthFactor = 1.5f; // Scale factor for the X-axis
//     public float widthFactor = 1.5f;  // Scale factor for the Z-axis
//     public float heightFactor = 1.5f; // Scale factor for the Y-axis
//     public bool uniformScale = true;  // If true, scales all dimensions uniformly
//     public KeyCode enlargeKey = KeyCode.E; // Key to trigger enlargement

//     private ProBuilderMesh pbMesh;

//     private void Start()
//     {
//         // Get the ProBuilderMesh component
//         pbMesh = GetComponent<ProBuilderMesh>();

//         if (pbMesh == null)
//         {
//             Debug.LogError("ProBuilderMesh component not found on this object!");
//         }
//     }

//     private void Update()
//     {
//         // Check if the enlargeKey is pressed
//         if (Input.GetKeyDown(enlargeKey))
//         {
//             EnlargeObjectInPlace();
//         }
//     }

//     private void EnlargeObjectInPlace()
//     {
//         if (pbMesh != null)
//         {
//             // Get the current vertex positions
//             IList<Vector3> verticesList = pbMesh.positions;
//             Vector3[] vertices = verticesList.ToArray();

//             // Calculate the center of the object manually
//             Vector3 center = CalculateCenter(vertices);

//             // Scale each vertex relative to the center
//             for (int i = 0; i < vertices.Length; i++)
//             {
//                 Vector3 direction = vertices[i] - center;
//                 if (uniformScale)
//                 {
//                     // Uniform scaling on all axes
//                     vertices[i] = center + direction * lengthFactor;
//                 }
//                 else
//                 {
//                     // Non-uniform scaling for each axis
//                     vertices[i] = center + new Vector3(
//                         direction.x * lengthFactor,
//                         direction.y * heightFactor,
//                         direction.z * widthFactor
//                     );
//                 }
//             }

//             // Apply the new vertex positions
//             pbMesh.positions = new List<Vector3>(vertices);

//             // Rebuild the mesh
//             pbMesh.ToMesh();
//             pbMesh.Refresh();
//         }
//     }

//     private Vector3 CalculateCenter(Vector3[] vertices)
//     {
//         if (vertices.Length == 0)
//             return Vector3.zero;

//         Vector3 sum = Vector3.zero;
//         foreach (Vector3 vertex in vertices)
//         {
//             sum += vertex;
//         }
//         return sum / vertices.Length;
//     }
// }
