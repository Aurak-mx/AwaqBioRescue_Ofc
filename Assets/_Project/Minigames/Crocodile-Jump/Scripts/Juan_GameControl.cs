using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Clase principal que controla el flujo del juego, vidas, puntos y estado del jugador
public class Juan_GameControl : MonoBehaviour
{
    // Configuración básica del juego
    [Header("Configuración")]
    public int tiempoJuego = 60; 
    public static Juan_GameControl Instance; 
    
    // Referencias a otros componentes del juego
    [Header("Referencias")]
    public Juan_UIController UIController; 
    public Juan_SFXManager sfxManager; 
    public SpriteRenderer playerSprite; 

    // Sistema de puntuación
    [Header("Puntaje")]
    public int puntos = 0; 

    // Estado del jugador
    [Header("Estado")]
    public bool invencible = false; 

    // Inicializa el singleton y configuramos los valores iniciales al cargar el objeto
    void Awake()
    {
        // Implementar patrón singleton: destruir duplicados y preservar instancia
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject); // Persistir entre escenas

        // Inicializar vidas si no existen en PlayerPrefs
        if (!PlayerPrefs.HasKey("Vidas"))
        {
            PlayerPrefs.SetInt("Vidas", 3);
        }

        // Configurar tiempo de juego en PlayerPrefs
        PlayerPrefs.SetInt("Tiempo para perder", PlayerPrefs.GetInt("Tiempo para perder", tiempoJuego));
        SetReferences(); // Asignar referencias a componentes
    }

    // Suscribirse al evento de carga de escena al habilitar el objeto
    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    // Desuscribirse del evento al deshabilitar el objeto
    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    // Método llamado al cargar una nueva escena: resetea estado y referencias
    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        StopAllCoroutines(); // Detener todas las corutinas activas
        invencible = false; 
        ResetearJuego();
        SetReferences(); 

        // Asegurar que el sprite del jugador esté visible (para evitar errores de invisibilidad)
        if (playerSprite != null)
            playerSprite.enabled = true;
    }

    // Busca y asigna referencias a componentes necesarios en la escena
    void SetReferences()
    {
        UIController = FindFirstObjectByType<Juan_UIController>();
        sfxManager = FindFirstObjectByType<Juan_SFXManager>();

        // Buscar el sprite del jugador si no está asignado
        if (playerSprite == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                playerSprite = player.GetComponent<SpriteRenderer>();
        }

        // Iniciar el temporizador en la UI si existe
        if (UIController != null)
        {
            UIController.StartTimer();
        }
    }

    // Devuelve el número de vidas restantes desde PlayerPrefs
    public int GetVidasRestantes()
    {
        return PlayerPrefs.GetInt("Vidas");
    }

    // Método público para recibir daño (llamado por enemigos)
    public void Daño()
    {
        if (invencible) return; // Ignorar si es invencible
        PerderVida(); // Procesar pérdida de vida
    }

    // Método privado para perder vidas
    void PerderVida()
    {
        Juan_SFXManager.Instance.PlayGolpe(); // Reproducir sonido de golpe
        invencible = true; // Activar invencibilidad temporal

        int vidasActuales = GetVidasRestantes();

        if (vidasActuales > 0)
        {
            int nuevasVidas = vidasActuales - 1;
            PlayerPrefs.SetInt("Vidas", nuevasVidas); // Guardar nuevas vidas

            if (UIController != null)
                UIController.UpdateVidas(); // Actualizar UI

            if (nuevasVidas > 0)
            {
                StartCoroutine(EstadoInvencible()); // Iniciar periodo de invencibilidad
            }
            else
            {
                GameOver(); // Si no quedan vidas, fin del juego
            }
        }
    }

    // Corutina que maneja el efecto visual de invencibilidad (parpadeo del sprite)
    IEnumerator EstadoInvencible()
    {

        float tiempoEfecto = 1.5f; // Duración total del efecto
        float intervalo = 0.15f; // Intervalo entre parpadeos
        float tiempo = 0f;

        // Bucle de parpadeo durante el tiempo especificado
        while (tiempo < tiempoEfecto)
        {
            if (playerSprite != null)
                playerSprite.enabled = !playerSprite.enabled; // Alternar visibilidad

            yield return new WaitForSeconds(intervalo);
            tiempo += intervalo;
        }

        // Asegurar que el sprite esté visible al final
        if (playerSprite != null)
            playerSprite.enabled = true;

        invencible = false; // Desactivar invencibilidad
    }

    // Maneja el fin del juego: resetear valores y cargar escena de game over
    public void GameOver()
    {

        PlayerPrefs.SetInt("Vidas", 3); // Resetear vidas
        puntos = 0; // Resetear puntos

        if (UIController != null)
        {
            UIController.ActualizarPuntos(puntos); // Actualizar UI
        }

        invencible = false; // Desactivar invencibilidad
        SceneManager.LoadScene("Juan_GameOver"); // Cargar escena de game over
    }

    // Agrega puntos al total y actualiza la UI (evita puntos negativos)
    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;

        if (puntos < 0)
            puntos = 0; // No permitir puntos negativos

        if (UIController != null)
        {
            UIController.ActualizarPuntos(puntos); // Actualizar UI
        }
    }
    // Resetea el juego: vidas, puntos y cofres (usado al reiniciar)
    public void ResetearJuego()
    {
        PlayerPrefs.SetInt("Vidas", 3); // Resetear vidas
        puntos = 0; // Resetear puntos

        // Resetear cofres si el manager existe
        if (Juan_CofresManager.Instance != null)
        {
            Juan_CofresManager.Instance.ResetearCofres();
        }
    }
    
}