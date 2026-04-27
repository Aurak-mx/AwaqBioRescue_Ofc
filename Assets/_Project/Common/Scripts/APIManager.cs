using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using Newtonsoft.Json;

public class APIManager : MonoBehaviour
{
    string BaseUrl = "https://127.0.0.1:3000";

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

    public class PuntajeItem
    {
        public int id_medalla   { get; set; }
        public int mejor_puntos { get; set; }
    }

    public class MedallaUsuario
    {
        public int    id_usuario      { get; set; }
        public string medalla         { get; set; }
        public string rank            { get; set; }
        public string fecha_obtencion { get; set; }
    }

    public class PreguntaData
    {
        public string pregunta         { get; set; }
        public string opcion1          { get; set; }
        public string opcion2          { get; set; }
        public string opcion3          { get; set; }
        public string opcion4          { get; set; }
        public int    opcion_correcta  { get; set; }
    }

    private class PreguntasResponse
    {
        public bool success { get; set; }
        public List<PreguntaData> data { get; set; }
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

    // ─── PUNTAJE ────────────────────────────────────────────────

    /// <summary>
    /// Obtiene el puntaje del usuario por id. Devuelve una lista con el mejor puntaje por minijuego.
    /// Uso: GetPuntaje(28, (lista) => { ... });
    /// </summary>
    public void GetPuntaje(int idUsuario, Action<List<PuntajeItem>> onResult)
    {
        StartCoroutine(GetPuntajeCoroutine(idUsuario, onResult));
    }

    private IEnumerator GetPuntajeCoroutine(int idUsuario, Action<List<PuntajeItem>> onResult)
    {
        string endpoint = $"{BaseUrl}/puntaje/{idUsuario}";

        UnityWebRequest web = UnityWebRequest.Get(endpoint);
        web.certificateHandler = new ForceAcceptAll();
        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error API: " + web.error);
            onResult?.Invoke(new List<PuntajeItem>());
        }
        else
        {
            List<PuntajeItem> lista = new List<PuntajeItem>();
            lista = JsonConvert.DeserializeObject<List<PuntajeItem>>(web.downloadHandler.text);
            onResult?.Invoke(lista);
        }
    }

    // ─── MEDALLAS ───────────────────────────────────────────────

    /// <summary>
    /// Obtiene las medallas del usuario por id. Devuelve una lista con todas las medallas obtenidas.
    /// Uso: GetMedallas(28, (lista) => { ... });
    /// </summary>
    public void GetMedallas(int idUsuario, Action<List<MedallaUsuario>> onResult)
    {
        StartCoroutine(GetMedallasCoroutine(idUsuario, onResult));
    }

    private IEnumerator GetMedallasCoroutine(int idUsuario, Action<List<MedallaUsuario>> onResult)
    {
        string endpoint = $"{BaseUrl}/medallas/{idUsuario}";

        UnityWebRequest web = UnityWebRequest.Get(endpoint);
        web.certificateHandler = new ForceAcceptAll();
        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error API: " + web.error);
            onResult?.Invoke(new List<MedallaUsuario>());
        }
        else
        {
            List<MedallaUsuario> lista = new List<MedallaUsuario>();
            lista = JsonConvert.DeserializeObject<List<MedallaUsuario>>(web.downloadHandler.text);
            onResult?.Invoke(lista);
        }
    }

    // ─── PREGUNTAS ──────────────────────────────────────────────

    /// <summary>
    /// Obtiene las preguntas del minijuego El Risco. Devuelve una lista de PreguntaData.
    /// Uso: GetPreguntas((lista) => { ... });
    /// </summary>
    public void GetPreguntas(Action<List<PreguntaData>> onResult)
    {
        StartCoroutine(GetPreguntasCoroutine(onResult));
    }

    private IEnumerator GetPreguntasCoroutine(Action<List<PreguntaData>> onResult)
    {
        string endpoint = $"{BaseUrl}/unity/preguntas";

        UnityWebRequest web = UnityWebRequest.Get(endpoint);
        web.certificateHandler = new ForceAcceptAll();
        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error API: " + web.error);
            onResult?.Invoke(new List<PreguntaData>());
        }
        else
        {
            PreguntasResponse respuesta = JsonConvert.DeserializeObject<PreguntasResponse>(web.downloadHandler.text);
            if (respuesta != null && respuesta.success && respuesta.data != null)
                onResult?.Invoke(respuesta.data);
            else
                onResult?.Invoke(new List<PreguntaData>());
        }
    }
}