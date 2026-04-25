using UnityEngine;

// Reproduce musica de fondo en loop para la escena donde se coloque.
// Adjuntar a un GameObject vacio en la escena y asignar el AudioClip en el Inspector.
public class MusicaFondo : MonoBehaviour
{
    [Header("Musica")]
    public AudioClip clip;
    [Range(0f, 1f)] public float volumen = 0.5f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volumen;
    }

    void Start()
    {
        if (clip != null)
            audioSource.Play();
    }
}
