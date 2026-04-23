using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class PlayerStatsManager : MonoBehaviour
{
    public TextMeshProUGUI txtPuntaje; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GetPuntaje()); 
    }

    IEnumerator GetPuntaje()
    {
        string JSONurl = "https://127.0.0.1:5005/puntaje/3"; 

        UnityWebRequest web = UnityWebRequest.Get(JSONurl); 
        web.certificateHandler = new ForceAcceptAll(); 
        yield return web.SendWebRequest(); 

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error API: " + web.error); 
        }
        else
        {
            List<PuntajeData> puntajeList = JsonConvert.DeserializeObject<List<PuntajeData>>(web.downloadHandler.text);
            
            if (puntajeList.Count > 0 && puntajeList[0].puntaje_total != null)
            {
                txtPuntaje.text = "Tótems Actuales: " + puntajeList[0].puntaje_total; 
            }
            else
            {
                txtPuntaje.text = "Tótems Actuales: 0"; 
            }
        }
    }
}


[System.Serializable]
public class PuntajeData
{
    public int? puntaje_total;
}