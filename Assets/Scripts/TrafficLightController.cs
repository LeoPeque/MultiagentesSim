using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public GameObject wallPrefab;
    public Transform wallSpawnPoint;

    private GameObject currentWall;

    public enum LightState { Red, Yellow, Green }
    private LightState currentState;

    public void SetLightState(LightState state)
    {
        currentState = state;
        UpdateLights();
        UpdateWall();
    }

    private void UpdateLights()
    {
        redLight.SetActive(currentState == LightState.Red);
        yellowLight.SetActive(currentState == LightState.Yellow);
        greenLight.SetActive(currentState == LightState.Green);
    }

    private void UpdateWall()
    {
        if (currentState == LightState.Red)
        {
            if (currentWall == null)
            {
                currentWall = Instantiate(wallPrefab, wallSpawnPoint.position, wallSpawnPoint.rotation);
            }
        }
        else
        {
            if (currentWall != null)
            {
                Destroy(currentWall);
                currentWall = null;
            }
        }
    }
}