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
        if (playerCerca && Input.GetKeyDown(KeyCode.E) && !abierto)
        {
            Juan_SFXManager.Instance.PlayCofre();
            StartCoroutine(AbrirCofre());
        }
    }

    IEnumerator AbrirCofre()
    {
        abierto = true;

        mensajeE.SetActive(false);

        // 🎬 Animación
        if (animator != null)
            animator.SetBool("Abrir", true);

        // ⏱️ Esperar animación
        yield return new WaitForSeconds(1f);

        // 👇 CONTAR COFRE AQUÍ (IMPORTANTE)
        if (Juan_CofresManager.Instance != null)
        {
            Juan_CofresManager.Instance.CofreAbierto();
        }

        // 🧠 Mostrar pregunta
        panelPregunta.SetActive(true);

        // 🧊 Pausar juego
        Time.timeScale = 0f;

        preguntaUI.SetPregunta(pregunta, respuestas, respuestaCorrecta, ResultadoPregunta);

        Debug.Log("Cofre abierto");
    }

    void ResultadoPregunta(bool correcta)
    {
        if (correcta)
        {
            if (Juan_GameControl.Instance != null)
            {
                Juan_SFXManager.Instance.PlayCorrecto();
                Juan_GameControl.Instance.SumarPuntos(200);
            }
        }
        else
        {
            if (Juan_GameControl.Instance != null)
            {
                Juan_SFXManager.Instance.PlayIncorrecto();
                Juan_GameControl.Instance.SumarPuntos(-50);
            }
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