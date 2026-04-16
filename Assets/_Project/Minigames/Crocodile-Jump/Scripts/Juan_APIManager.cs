using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class Juan_APIManager : MonoBehaviour
{

    public void SendPostMedalla(int idUsuario, int idMedalla, int idRankMedalla)
    {
        StartCoroutine(PostMedallaCoroutine(idUsuario, idMedalla, idRankMedalla));
    }

    private IEnumerator PostMedallaCoroutine(int _id_usuario, int _id_medalla, int id_rankmedalla)
    {
        string url = "https://localhost:3000/PostMedalla";

        var medalla = new Medalla
        {
            id_usuario = _id_usuario,
            id_medalla = _id_medalla,
            id_rankmedalla = id_rankmedalla
        };

        string json = JsonUtility.ToJson(medalla);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest web = new UnityWebRequest(url, "POST"))
        {
            web.uploadHandler = new UploadHandlerRaw(bodyRaw);
            web.downloadHandler = new DownloadHandlerBuffer();
            web.SetRequestHeader("Content-Type", "application/json");
            web.certificateHandler = new ForceAcceptAll();

            yield return web.SendWebRequest();

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