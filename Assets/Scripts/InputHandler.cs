using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class InputHandler : MonoBehaviour
{
    public TMP_InputField agentsInput;
    public TMP_InputField stoplightTimeInput;
    public TMP_InputField carSpeedInput;

    public void OnButtonClick()
    {
        // Parse the input values and store them in the static class
        Parameters.numberOfAgents = int.Parse(agentsInput.text);
        Parameters.stoplightTime = float.Parse(stoplightTimeInput.text);
        Parameters.carSpeed = float.Parse(carSpeedInput.text);

        // Load the simulation scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}

