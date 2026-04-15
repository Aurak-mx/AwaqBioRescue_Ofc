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
    public SpriteRenderer playerSprite;

    [Header("Puntaje")]
    public int puntos = 0;

    [Header("Estado")]
    public bool invencible = false;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Inicializar vidas
        if (!PlayerPrefs.HasKey("Vidas"))
        {
            PlayerPrefs.SetInt("Vidas", 3);
        }

        PlayerPrefs.SetInt("Tiempo para perder", PlayerPrefs.GetInt("Tiempo para perder", tiempoJuego));
        SetReferences();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        // 💥 DETENER TODO
        StopAllCoroutines();

        // 🔥 RESET ESTADO
        invencible = false;

        SetReferences();

        if (playerSprite != null)
            playerSprite.enabled = true;
    }

    void SetReferences()
    {
        UIController = FindFirstObjectByType<Juan_UIController>();
        sfxManager = FindFirstObjectByType<Juan_SFXManager>();

        if (playerSprite == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                playerSprite = player.GetComponent<SpriteRenderer>();
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

    // 🔥 FUNCIÓN PRINCIPAL DE DAÑO
    public void Daño()
    {
        if (invencible) return;

        PerderVida();
    }

    void PerderVida()
    {
        invencible = true;

        int vidasActuales = GetVidasRestantes();

        if (vidasActuales > 0)
        {
            int nuevasVidas = vidasActuales - 1;
            PlayerPrefs.SetInt("Vidas", nuevasVidas);

            if (UIController != null)
                UIController.UpdateVidas();

            if (nuevasVidas > 0)
            {
                StartCoroutine(EstadoInvencible());
            }
            else
            {
                GameOver();
            }
        }
    }

    IEnumerator EstadoInvencible()
    {
        Debug.Log("¡Inicio de Invencibilidad!");

        float tiempoEfecto = 1.5f;
        float intervalo = 0.15f;
        float tiempo = 0f;

        while (tiempo < tiempoEfecto)
        {
            if (playerSprite != null)
                playerSprite.enabled = !playerSprite.enabled;

            yield return new WaitForSeconds(intervalo);
            tiempo += intervalo;
        }

        if (playerSprite != null)
            playerSprite.enabled = true;

        invencible = false;

        Debug.Log("Fin de Invencibilidad");
    }

    public void GameOver()
    {
        
        Debug.Log("Game Over");

        PlayerPrefs.SetInt("Vidas", 3);
        puntos = 0;

        if (UIController != null)
        {
            UIController.ActualizarPuntos(puntos);
        }

        invencible = false;
        SceneManager.LoadScene("Juan_GameOver");
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;

        if (puntos < 0)
            puntos = 0;

        if (UIController != null)
        {
            UIController.ActualizarPuntos(puntos);
        }
    }
    public void ResetearJuego()
    {
        PlayerPrefs.SetInt("Vidas", 3);
        puntos = 0;

        // 🔥 Reiniciar cofres
        if (Juan_CofresManager.Instance != null)
        {
            Juan_CofresManager.Instance.ResetearCofres();
        }
    }
    
}