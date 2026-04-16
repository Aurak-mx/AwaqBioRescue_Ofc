using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

// Clase para enviar la medalla al servidor
public class Juan_APIManager : MonoBehaviour
{

    // Funcion para enviar la medalla con la API y parametros
    public void SendPostMedalla(int idUsuario, int idMedalla, int idRankMedalla)
    {
        StartCoroutine(PostMedallaCoroutine(idUsuario, idMedalla, idRankMedalla));
    }

    // Funcion para enviar la medalla
    private IEnumerator PostMedallaCoroutine(int _id_usuario, int _id_medalla, int id_rankmedalla)
    {
        string url = "https://localhost:3000/PostMedalla";

        // Creamos el objeto de la medalla
        var medalla = new Medalla
        {
            id_usuario = _id_usuario,
            id_medalla = _id_medalla,
            id_rankmedalla = id_rankmedalla
        };


        // Convertimos el objeto a JSON
        string json = JsonUtility.ToJson(medalla);
        // Convertimos el JSON a bytes
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);


        // Crea una solicitud POST a la URL
        using (UnityWebRequest web = new UnityWebRequest(url, "POST"))
        {
            
            // Definimos el tipo de contenido y el cuerpo de la solicitud
            web.uploadHandler = new UploadHandlerRaw(bodyRaw);

            // Prepara para recibir la respuesta del servidor en un buffer de memoria.
            web.downloadHandler = new DownloadHandlerBuffer();

            // Establece el encabezado HTTP indicando que los datos enviados son en formato JSON.
            web.SetRequestHeader("Content-Type", "application/json");

            // Le asignamos el certificado handler
            web.certificateHandler = new ForceAcceptAll();

            // Envia la solicitud
            yield return web.SendWebRequest();


            // Verifica si la solicitud fue exitosa o no
            if (web.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error API: " + web.error);
            }
            else
            {
                Debug.Log("Medalla enviada correctamente: " + web.downloadHandler.text);
            }
        }
    }
}