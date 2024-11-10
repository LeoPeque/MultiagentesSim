using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject Agente1;
    public GameObject Agente2;
    public GameObject Agente3;
    public GameObject prefab;
    private bool created = false;

    public GameObject endScreen;

    public TextMeshProUGUI Resultado1;
    public TextMeshProUGUI Resultado2;
    public TextMeshProUGUI Resultado3;

    public void ReiniciarSimulacion(){
        SceneManager.LoadScene("Parametros");
    }

    // Update is called once per frame
    void Update()
    {
        if(Agente1.GetComponent<NavAgent>().isFinished == true && Agente2.GetComponent<NavAgent>().isFinished == true && Agente3.GetComponent<NavAgent>().isFinished == true && created == false){
                GameObject canvas = Instantiate(prefab);
                Transform endScreen = canvas.transform.GetChild(0);
                endScreen.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Agente 1 \n\nTiempo: " + Agente1.GetComponent<NavAgent>().elapsedTime + " segundos \nDistancia: " + Agente1.GetComponent<NavAgent>().elapsedTime * 1.4 + "m";
                endScreen.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Agente 2 \n\nTiempo: " + Agente2.GetComponent<NavAgent>().elapsedTime + " segundos \nDistancia: " + Agente2.GetComponent<NavAgent>().elapsedTime * 1.4 + "m";
                endScreen.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Agente 3 \n\nTiempo: " + Agente3.GetComponent<NavAgent>().elapsedTime + " segundos \nDistancia: " + Agente3.GetComponent<NavAgent>().elapsedTime * 1.4 + "m";
                created = true;
        }
    }
}
