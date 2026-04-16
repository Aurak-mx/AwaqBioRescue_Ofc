using UnityEngine;

// Reproduce sonidos ambientales de forma aleatoria en intervalos variables
public class Juan_AmbientSound : MonoBehaviour
{
    public AudioClip sonido; 
    public float tiempoMin = 2f; 
    public float tiempoMax = 7f;

    private float timer; // Temporizador para el próximo sonido

    // Inicializa el temporizador al comenzar
    void Start()
    {
        ProgramarSiguiente();
    }

    // Cuenta el tiempo y reproduce el sonido cuando llegue a cero
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Reproducir();
            ProgramarSiguiente();
        }
    }

    // Reproduce el sonido en la posición del objeto
    void Reproducir()
    {
        if (sonido == null) return;

        Juan_SFXManager.Instance.PlaySFXAtPosition(sonido, transform.position);
    }

    // Programa el próximo intervalo aleatorio
    void ProgramarSiguiente()
    {
        timer = Random.Range(tiempoMin, tiempoMax);
    }
}