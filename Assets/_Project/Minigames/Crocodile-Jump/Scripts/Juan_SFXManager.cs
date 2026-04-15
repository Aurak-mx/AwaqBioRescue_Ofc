using UnityEngine;

public class Juan_SFXManager : MonoBehaviour
{
    public static Juan_SFXManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicaSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip musicaFondo;
    public AudioClip sonidoGolpe;
    public AudioClip sonidoCofre;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;
    public AudioClip sonidoBoton;
    public AudioClip cocodrilo;
    public AudioClip pajaro;
    public AudioClip cuerda;
    public AudioClip sonidoPlanta;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 🔥 opcional pero recomendado
    }

    void Start()
    {
        ReproducirMusica();
    }

    // 🎵 Música
    public void ReproducirMusica()
    {
        if (musicaSource != null && musicaFondo != null)
        {
            musicaSource.clip = musicaFondo;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }

    // 🔊 Sonido global (UI, botones, etc.)
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // 📍 Sonido con posición (enemigos, mundo)
    public void PlaySFXAtPosition(AudioClip clip, Vector3 posicion)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, posicion);
        }
    }

    // 🎯 MÉTODOS RÁPIDOS (GLOBAL)
    public void PlayGolpe() => PlaySFX(sonidoGolpe);
    public void PlayCofre() => PlaySFX(sonidoCofre);
    public void PlayCorrecto() => PlaySFX(sonidoCorrecto);
    public void PlayIncorrecto() => PlaySFX(sonidoIncorrecto);
    public void PlayBoton() => PlaySFX(sonidoBoton);

    // 🎯 MÉTODOS RÁPIDOS (CON POSICIÓN)
    public void PlayCocodrilo(Vector3 pos) => PlaySFXAtPosition(cocodrilo, pos);
    public void PlayPajaro(Vector3 pos) => PlaySFXAtPosition(pajaro, pos);
    public void PlayCuerda(Vector3 pos) => PlaySFXAtPosition(cuerda, pos);
    public void PlayPlanta(Vector3 pos) => PlaySFXAtPosition(sonidoPlanta, pos);

    // 🔥 OPCIONAL: versiones globales también
    public void PlayCocodrilo() => PlaySFX(cocodrilo);
    public void PlayPajaro() => PlaySFX(pajaro);
    public void PlayCuerda() => PlaySFX(cuerda);
    public void PlayPlanta() => PlaySFX(sonidoPlanta);
}