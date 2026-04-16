using UnityEngine;
using System.Collections;

public class Juan_CofreInteraccion : MonoBehaviour
{
    
    public GameObject mensajeE;
    public GameObject panelPregunta;

    public Animator animator;
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
        // Si el jugador esta cerca, no esta abierto y presiona la tecla E
        if (playerCerca && Input.GetKeyDown(KeyCode.E) && !abierto)
        {
            // Ejecutamos su sonido
            Juan_SFXManager.Instance.PlayCofre();
            // Abrimos el cofre
            StartCoroutine(AbrirCofre());
        }
    }

    IEnumerator AbrirCofre()
    {
        // Marcamos que el cofre esta abierto
        abierto = true;

        // Desactivamos el mensaje de "E"
        mensajeE.SetActive(false);

        // Activamos el Animator
        if (animator != null)
            animator.SetBool("Abrir", true);

        // Esperamos 1 segundo para que se escuche el sonido
        yield return new WaitForSeconds(1f);

        // Abrimos el cofre
        if (Juan_CofresManager.Instance != null)
        {
            Juan_CofresManager.Instance.CofreAbierto();
        }

        // Activamos el panel de la pregunta
        panelPregunta.SetActive(true);


        // Detenemos el juego
        Time.timeScale = 0f;

        // Mostramos la pregunta con su pregunta, opciones e indice de la respuesta correcta
        preguntaUI.SetPregunta(pregunta, respuestas, respuestaCorrecta, ResultadoPregunta);
    }

    void ResultadoPregunta(bool correcta)
    {
        // Si la pregunta es correcta
        if (correcta)
        {
            if (Juan_GameControl.Instance != null)
            {
                // Sumamos 200 puntos y reproducimos el sonido
                Juan_SFXManager.Instance.PlayCorrecto();
                Juan_GameControl.Instance.SumarPuntos(200);
            }
        }
        else
        {
            if (Juan_GameControl.Instance != null)
            {   
                // Restamos 50 puntos y reproducimos el sonido
                Juan_SFXManager.Instance.PlayIncorrecto();
                Juan_GameControl.Instance.SumarPuntos(-50);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el jugador entra al area del trigger se activa el panel del mensaje
        if (other.CompareTag("Player") && !abierto)
        {
            playerCerca = true;
            mensajeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Si el jugador sale del area del trigger se desactiva el panel del mensaje
        if (other.CompareTag("Player"))
        {
            playerCerca = false;
            mensajeE.SetActive(false);
        }
    }
}