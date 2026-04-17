using UnityEngine;

// Gestor de audio: música de fondo y efectos de sonido (SFX)
public class Juan_SFXManager : MonoBehaviour
{
    public static Juan_SFXManager Instance; // Singleton para acceso global

    // Fuentes de audio
    [Header("Audio Sources")]
    public AudioSource musicaSource; 
    public AudioSource sfxSource;

    // Controles de volumen
    [Header("Volumen")]
    [Range(0f, 1f)] public float volumenMusica = 1f;
    [Range(0f, 1f)] public float volumenSFX = 1f;

    // Clips de audio disponibles
    [Header("Clips")]
    public AudioClip musicaFondo; // Música de fondo
    public AudioClip sonidoGolpe; // Sonido de golpe
    public AudioClip sonidoCofre; // Sonido de cofre
    public AudioClip sonidoCorrecto; // Sonido de respuesta correcta
    public AudioClip sonidoIncorrecto; // Sonido de respuesta incorrecta
    public AudioClip sonidoBoton; // Sonido de botón
    public AudioClip sonidoMordedura; // Sonido de mordedura

    // Inicializa el singleton y lo preserva entre escenas
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Inicia la reproducción de música al comenzar
    void Start()
    {
        ReproducirMusica();
    }

    // Actualiza los volúmenes en cada frame
    void Update()
    {
        if (musicaSource != null)
            musicaSource.volume = volumenMusica;

        if (sfxSource != null)
            sfxSource.volume = volumenSFX;
    }

    // Reproduce la música de fondo en loop
    public void ReproducirMusica()
    {
        if (musicaSource != null && musicaFondo != null)
        {
            musicaSource.clip = musicaFondo;
            musicaSource.loop = true;
            musicaSource.volume = volumenMusica;
            musicaSource.Play();
        }
    }

    // Reproduce un SFX una vez
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, volumenSFX);
        }
    }

    // Reproduce un SFX en una posición específica
    public void PlaySFXAtPosition(AudioClip clip, Vector3 posicion)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, posicion, volumenSFX);
        }
    }

    // Métodos convenientes para SFX específicos
    public void PlayGolpe() => PlaySFX(sonidoGolpe);
    public void PlayCofre() => PlaySFX(sonidoCofre);
    public void PlayCorrecto() => PlaySFX(sonidoCorrecto);
    public void PlayIncorrecto() => PlaySFX(sonidoIncorrecto);
    public void PlayBoton() => PlaySFX(sonidoBoton);
    public void PlayMordedura() => PlaySFX(sonidoMordedura);
}