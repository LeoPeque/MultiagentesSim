using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;

public class IntersectionSimulation : MonoBehaviour
{
    [Header("Agents")]
    [SerializeField] private GameObject Agente1;
    [SerializeField] private GameObject Agente2;
    [SerializeField] private GameObject Agente3;

    [Header("Objectives")]
    [SerializeField] private GameObject Meta1;
    [SerializeField] private GameObject Meta2;
    [SerializeField] private GameObject Meta3;

    private string apiUrlPost = "https://gansitospeligrosos.uc.r.appspot.com/gansitosapi/initialize_model/";
    private string apiUrlGet = "https://gansitospeligrosos.uc.r.appspot.com/gansitosapi/move_agent/";

    public DataObject myData;

    // creamos una instancia para que sea accesible en todo el código
    public static IntersectionSimulation Intersection { get; private set; }


    private void Awake()
    {
        // Ensure only one instance of this class exists
        if (Intersection == null)
        {
            Intersection = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Method to convert Vector2[] to List<float[]>
    private List<int[]> ConvertToIntList(Vector2[] vectors)
    {
        List<int[]> list = new List<int[]>();
        foreach (var vec in vectors)
        {
            list.Add(new int[] { Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y) });
        }
        return list;
    }

    public IEnumerator SendAgentsData(int numberOfAgents, Vector2[] initialPositions, Vector2[] objectives)
    {
        // Create the data structure
        AgentData agentsData = new AgentData
        {
            numberOfAgents = numberOfAgents,
            initialPositions = ConvertToIntList(initialPositions),
            objectives = ConvertToIntList(objectives)
        };

        // Serialize to JSON using Newtonsoft.Json
        string jsonData = JsonConvert.SerializeObject(agentsData);
        Debug.Log("Sending JSON: " + jsonData);

        // Create the POST request
        UnityWebRequest request = new UnityWebRequest(apiUrlPost, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Successfully sent data to API");
            Debug.Log("Response: " + request.responseCode);

            // Get the response JSON
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Response JSON: " + jsonResponse);

            // Deserialize the response JSON to a DataObject
            myData = JsonConvert.DeserializeObject<DataObject>(jsonResponse);
            Debug.Log("Steps obtained: " + myData.steps["1"][0]);
            Debug.Log("Status: " + myData.status);
        }
    }

    // Example usage
    void Start()
    {
        // Ensure that the 'z' component is used for the y-coordinate in Vector2
        Vector2[] initialPositions = {
            new Vector2(Mathf.CeilToInt(Agente1.transform.position.x), Mathf.CeilToInt(Agente1.transform.position.z)),
            new Vector2(Mathf.CeilToInt(Agente2.transform.position.x), Mathf.CeilToInt(Agente2.transform.position.z)),
            new Vector2(Mathf.CeilToInt(Agente3.transform.position.x), Mathf.CeilToInt(Agente3.transform.position.z))
        };

        Vector2[] objectives = {
            new Vector2(Mathf.CeilToInt(Meta1.transform.position.x), Mathf.CeilToInt(Meta1.transform.position.z)),
            new Vector2(Mathf.CeilToInt(Meta2.transform.position.x), Mathf.CeilToInt(Meta2.transform.position.z)),
            new Vector2(Mathf.CeilToInt(Meta3.transform.position.x), Mathf.CeilToInt(Meta3.transform.position.z))
        };

        StartCoroutine(SendAgentsData(Parameters.numberOfAgents, initialPositions, objectives));

    }

}

[System.Serializable]
public class DataObject
{
    public Dictionary<string, List<List<int>>> steps;
    public string status;
}

