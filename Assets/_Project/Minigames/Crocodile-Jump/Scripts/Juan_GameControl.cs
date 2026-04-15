using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Juan_GameControl : MonoBehaviour
{
    [Header("Configuración")]
    public int tiempoJuego = 60;
    public static Juan_GameControl Instance;
    
    [Header("Referencias")]
    public Juan_UIController UIController;
    public Juan_SFXManager sfxManager;
    public SpriteRenderer playerSprite; // Arrastra al Jugador aquí en el Inspector

    [Header("Estado")]
    public bool estaPerdiendoVida = false;
    private float ultimoDaño = -10f;
    public float cooldownDaño = 1.5f;

    [Header("Puntaje")]
    public int puntos = 0;

    public void Awake()
    {
        // 1. Patrón Singleton: Evita duplicados al recargar escenas
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // 2. Solo reseteamos vidas si NO existen (primera vez que se abre el juego)
        if (!PlayerPrefs.HasKey("Vidas"))
        {
            PlayerPrefs.SetInt("Vidas", 3);
        }

        PlayerPrefs.SetInt("Tiempo para perder", PlayerPrefs.GetInt("Tiempo para perder", tiempoJuego));
        SetReferences();
    }

    // Se ejecuta cada vez que el script se activa (útil al cambiar escenas)
    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    // Esta función arregla el problema de que el tiempo se quede en 60
    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        estaPerdiendoVida = false; // <--- AGREGA ESTA LÍNEA
        SetReferences();
        
        // Si tenías el parpadeo activo, nos aseguramos de encender el sprite
        if (playerSprite != null) playerSprite.enabled = true; 
    }

    void SetReferences()
    {
        // Buscamos la UI de la nueva escena
        UIController = FindFirstObjectByType<Juan_UIController>();
        sfxManager = FindFirstObjectByType<Juan_SFXManager>();

        // Buscamos al jugador para el parpadeo si no está asignado
        if (playerSprite == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) playerSprite = player.GetComponent<SpriteRenderer>();
        }

        if (UIController != null)
        {
            UIController.StartTimer();
        }
    }

    public int GetVidasRestantes()
    {
        return PlayerPrefs.GetInt("Vidas");
    }

   public void PerderVida()
    {
        // 🔒 BLOQUEO POR TIEMPO (CLAVE)
        if (Time.time < ultimoDaño + cooldownDaño) return;

        ultimoDaño = Time.time;

        if (estaPerdiendoVida) return;

        estaPerdiendoVida = true;

        int vidasActuales = GetVidasRestantes();

        if (vidasActuales > 0)
        {
            int nuevasVidas = vidasActuales - 1;
            PlayerPrefs.SetInt("Vidas", nuevasVidas);
            
            if (UIController != null) UIController.UpdateVidas();

            if (nuevasVidas > 0)
            {
                StartCoroutine(EstadoInvencible());
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            estaPerdiendoVida = false; 
            GameOver();
        }
    }

    IEnumerator EstadoInvencible()
    {
        estaPerdiendoVida = true;
        Debug.Log("¡Inicio de Invencibilidad!");

        // Parpadeo durante 1.5 segundos
        float tiempoEfecto = 1.5f;
        float intervaloParpadeo = 0.15f;
        float transcurrido = 0f;

        while (transcurrido < tiempoEfecto)
        {
            if (playerSprite != null) 
                playerSprite.enabled = !playerSprite.enabled;
            
            yield return new WaitForSeconds(intervaloParpadeo);
            transcurrido += intervaloParpadeo;
        }

        // Aseguramos que el sprite quede visible al terminar
        if (playerSprite != null) playerSprite.enabled = true;
        
        estaPerdiendoVida = false;
        Debug.Log("Fin de Invencibilidad");
    }

   public void GameOver()
    {
        Debug.Log("Game Over");

        PlayerPrefs.SetInt("Vidas", 3);

        // 👇 RESET DE PUNTOS
        puntos = 0;

        if (UIController != null)
        {
            UIController.ActualizarPuntos(puntos);
        }

        estaPerdiendoVida = false;

        SceneManager.LoadScene("Juan_GameOver");
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;

        // Evitar negativos (opcional)
        if (puntos < 0)
            puntos = 0;

        if (UIController != null)
        {
            UIController.ActualizarPuntos(puntos);
        }
    }
}