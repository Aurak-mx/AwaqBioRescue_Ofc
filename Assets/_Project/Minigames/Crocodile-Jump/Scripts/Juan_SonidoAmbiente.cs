using UnityEngine;

public class Juan_AmbientSound : MonoBehaviour
{
    public AudioClip sonido;
    public float tiempoMin = 2f;
    public float tiempoMax = 7f;

    private float timer;

    void Start()
    {
        ProgramarSiguiente();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Reproducir();
            ProgramarSiguiente();
        }
    }

    void Reproducir()
    {
        if (sonido == null) return;

        Juan_SFXManager.Instance.PlaySFXAtPosition(sonido, transform.position);
    }

    void ProgramarSiguiente()
    {
        timer = Random.Range(tiempoMin, tiempoMax);
    }
}