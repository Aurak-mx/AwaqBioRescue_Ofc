using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

public class APIManager : MonoBehaviour
{
    string BaseUrl = "https://192.168.4.251:3000";

    // ─── MODELOS ────────────────────────────────────────────────

    [Serializable]
    private class LoginRequest
    {
        public string username;
    }

    [Serializable]
    public class UsuarioResponse
    {
        public int    id_usuario;
        public string nombre;
        public string email;
        public string contrasena;
        public int    id_rol;
    }

    [Serializable]
    private class Medalla
    {
        public int id_usuario;
        public int id_medalla;
        public int id_rankmedalla;
    }

    // ─── LOGIN ──────────────────────────────────────────────────

    /// <summary>
    /// Llama al login. El callback regresa el usuario si fue exitoso, o null si falló.
    /// Uso: SendLogin("juan", "1234", (usuario) => { ... });
    /// </summary>
    public void SendLogin(string username, string password, Action<UsuarioResponse> onResult)
    {
        StartCoroutine(LoginCoroutine(username, password, onResult));
    }

    private IEnumerator LoginCoroutine(string username, string password, Action<UsuarioResponse> onResult)
    {
        string endpoint = $"{BaseUrl}/obtenerUsuarioPorNombre";

        // 1) Buscamos el usuario por nombre
        var reqBody = new LoginRequest { username = username };
        string json = JsonUtility.ToJson(reqBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest web = new UnityWebRequest(endpoint, "POST"))
        {
            web.uploadHandler   = new UploadHandlerRaw(bodyRaw);
            web.downloadHandler = new DownloadHandlerBuffer();
            web.SetRequestHeader("Content-Type", "application/json");
            web.certificateHandler = new ForceAcceptAll();

            yield return web.SendWebRequest();

            if (web.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error de red: " + web.error);
                onResult?.Invoke(null);
                yield break;
            }

            // 2) La API devuelve un array, lo envolvemos para poder deserializarlo
            string rawJson = web.downloadHandler.text;
            string wrapped = $"{{\"items\":{rawJson}}}";

            UsuarioListWrapper wrapper = JsonUtility.FromJson<UsuarioListWrapper>(wrapped);

            // 3) Si no encontró usuario
            if (wrapper.items == null || wrapper.items.Length == 0)
            {
                onResult?.Invoke(null);
                yield break;
            }

            UsuarioResponse usuario = wrapper.items[0];

            // 4) Verificamos la contraseña localmente
            if (usuario.contrasena != password)
            {
                onResult?.Invoke(null);
                yield break;
            }
            onResult?.Invoke(usuario);
        }
    }

    // Wrapper auxiliar para deserializar el array que devuelve la API
    [Serializable]
    private class UsuarioListWrapper
    {
        public UsuarioResponse[] items;
    }

    // ─── MEDALLA ────────────────────────────────────────────────

    public void SendPostMedalla(int idUsuario, int idMedalla, int idRankMedalla)
    {
        StartCoroutine(PostMedallaCoroutine(idUsuario, idMedalla, idRankMedalla));
    }

    private IEnumerator PostMedallaCoroutine(int _id_usuario, int _id_medalla, int id_rankmedalla)
    {
        string endpoint = $"{BaseUrl}/PostMedalla";

        var medalla = new Medalla
        {
            id_usuario    = _id_usuario,
            id_medalla    = _id_medalla,
            id_rankmedalla = id_rankmedalla
        };

        string json    = JsonUtility.ToJson(medalla);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest web = new UnityWebRequest(endpoint, "POST"))
        {
            web.uploadHandler   = new UploadHandlerRaw(bodyRaw);
            web.downloadHandler = new DownloadHandlerBuffer();
            web.SetRequestHeader("Content-Type", "application/json");
            web.certificateHandler = new ForceAcceptAll();

            yield return web.SendWebRequest();

            if (web.result != UnityWebRequest.Result.Success)
                Debug.LogError("Error API: " + web.error);
            else
                Debug.Log("Medalla enviada correctamente: " + web.downloadHandler.text);
        }
    }
}