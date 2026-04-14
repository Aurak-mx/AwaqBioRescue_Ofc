using UnityEngine;

public class Juan_CofreInteraccion : MonoBehaviour
{
    public GameObject mensajeE;
    public GameObject panelPregunta;

    public Juan_PregunaUI preguntaUI;

    [Header("Configuración de Pregunta")]
    [TextArea]
    public string pregunta;

    public string[] respuestas = new string[4];
    public int respuestaCorrecta;

    private bool playerCerca = false;
    private bool abierto = false;

    void Update()
    {
        if (playerCerca && Input.GetKeyDown(KeyCode.E) && !abierto)
        {
            AbrirCofre();
        }
    }

    void AbrirCofre()
    {
        abierto = true;

        mensajeE.SetActive(false);
        panelPregunta.SetActive(true);

        // 🔥 Mandar pregunta + callback
        preguntaUI.SetPregunta(pregunta, respuestas, respuestaCorrecta, ResultadoPregunta);

        Debug.Log("Cofre abierto");
    }

    void ResultadoPregunta(bool correcta)
    {
        if (correcta)
        {
            Debug.Log("🎁 Recompensa");
            // Aquí puedes:
            // - Dar monedas
            // - Abrir animación
            // - Sumar puntos
        }
        else
        {
            Debug.Log("❌ Fallaste");
            // Castigo opcional
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !abierto)
        {
            playerCerca = true;
            mensajeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = false;
            mensajeE.SetActive(false);
        }
    }
}