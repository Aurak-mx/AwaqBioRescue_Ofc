using UnityEngine;

public class Juan_SFXManager : MonoBehaviour
{
    public static Juan_SFXManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicaSource;
    public AudioSource sfxSource;

    [Header("Volumen")]
    [Range(0f, 1f)] public float volumenMusica = 1f;
    [Range(0f, 1f)] public float volumenSFX = 1f;

    [Header("Clips")]
    public AudioClip musicaFondo;
    public AudioClip sonidoGolpe;
    public AudioClip sonidoCofre;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;
    public AudioClip sonidoBoton;
    public AudioClip sonidoMordedura;

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

    void Start()
    {
        ReproducirMusica();
    }

    void Update()
    {
        // Aplicar volumen en tiempo real
        if (musicaSource != null)
            musicaSource.volume = volumenMusica;

        if (sfxSource != null)
            sfxSource.volume = volumenSFX;
    }

    // 🎵 Música
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

    // 🔊 Sonido global
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, volumenSFX);
        }
    }

    // 📍 Sonido con posición
    public void PlaySFXAtPosition(AudioClip clip, Vector3 posicion)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, posicion, volumenSFX);
        }
    }

    // 🎯 MÉTODOS RÁPIDOS
    public void PlayGolpe() => PlaySFX(sonidoGolpe);
    public void PlayCofre() => PlaySFX(sonidoCofre);
    public void PlayCorrecto() => PlaySFX(sonidoCorrecto);
    public void PlayIncorrecto() => PlaySFX(sonidoIncorrecto);
    public void PlayBoton() => PlaySFX(sonidoBoton);
    public void PlayMordedura() => PlaySFX(sonidoMordedura);
}