using UnityEngine;
using System.Collections.Generic;

public class TrafficLightManager : MonoBehaviour
{
    public List<TrafficLightController> trafficLights = new List<TrafficLightController>();
    
    public float greenDuration;
    public float yellowDuration;

    private int currentGreenIndex = -1;
    private float timer;

    void Start()
    {
        if (trafficLights.Count > 0)
        {
            SetNextGreenLight();
            greenDuration = Parameters.stoplightTime;
            yellowDuration = Parameters.stoplightTime;
        }
    }

    void Update()
    {
        if (currentGreenIndex == -1) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SetNextGreenLight();
        }
    }

    private void SetNextGreenLight()
    {
        if (currentGreenIndex != -1)
        {
            trafficLights[currentGreenIndex].SetLightState(TrafficLightController.LightState.Yellow);
            timer = yellowDuration;
            StartCoroutine(SetRedAfterDelay(currentGreenIndex, yellowDuration));
        }

        currentGreenIndex = (currentGreenIndex + 1) % trafficLights.Count;

        trafficLights[currentGreenIndex].SetLightState(TrafficLightController.LightState.Green);
        timer = greenDuration;

        for (int i = 0; i < trafficLights.Count; i++)
        {
            if (i != currentGreenIndex)
            {
                trafficLights[i].SetLightState(TrafficLightController.LightState.Red);
            }
        }
    }

    private System.Collections.IEnumerator SetRedAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        trafficLights[index].SetLightState(TrafficLightController.LightState.Red);
    }
}