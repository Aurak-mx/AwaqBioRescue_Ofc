using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

// Script para manejar la interfaz de preguntas con opciones y feedback
public class Juan_PregunaUI : MonoBehaviour
{
    
    public TextMeshProUGUI preguntaTexto; 
    public Button[] botones; 

    // Datos de la pregunta actual
    private string pregunta; 
    private string[] respuestas; 
    private int respuestaCorrecta; 

    
    private Action<bool> onRespuesta;

    // Carga la pregunta y configura los botones con las respuestas
    void CargarPregunta()
    {
        preguntaTexto.text = pregunta; // Mostrar pregunta

        for (int i = 0; i < botones.Length; i++)
        {
            int index = i; // Capturar índice para el listener

            botones[i].interactable = true; // Habilitar botón

            Image img = botones[i].GetComponent<Image>();
            img.color = Color.white; // Resetear color

            // Asignar texto de respuesta si existe
            if (respuestas != null && i < respuestas.Length)
            {
                var texto = botones[i].GetComponentInChildren<TextMeshProUGUI>();
                if (texto != null)
                    texto.text = respuestas[i];
            }

            // Limpiar y agregar listener para verificar respuesta
            botones[i].onClick.RemoveAllListeners();
            botones[i].onClick.AddListener(() => VerificarRespuesta(index));
        }
    }

    // Verifica la respuesta seleccionada y muestra feedback visual
    void VerificarRespuesta(int index)
    {
        // Deshabilitar todos los botones
        foreach (Button b in botones)
            b.interactable = false;

        bool esCorrecta = (index == respuestaCorrecta); // Verificar si es correcta

        // Cambiar colores: verde para correcta, rojo para incorrecta seleccionada
        for (int i = 0; i < botones.Length; i++)
        {
            Image img = botones[i].GetComponent<Image>();

            if (i == respuestaCorrecta)
            {
                img.color = Color.green; // Correcta
            }
            else if (i == index)
            {
                img.color = Color.red; // Incorrecta seleccionada
            }
        }

        onRespuesta?.Invoke(esCorrecta); // Invocar callback

        StartCoroutine(CerrarConDelay()); // Cerrar panel con delay
    }

    // Corutina para cerrar el panel después de un delay
    IEnumerator CerrarConDelay()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        CerrarPanel(); 
    }

    // Cierra el panel y reanuda el tiempo
    void CerrarPanel()
    {
        Time.timeScale = 1f; 
        gameObject.SetActive(false); // Ocultar panel
    }

    // Método público para configurar una nueva pregunta
    public void SetPregunta(string nuevaPregunta, string[] nuevasRespuestas, int correcta, Action<bool> callback)
    {
        pregunta = nuevaPregunta; // Asignar pregunta
        respuestas = nuevasRespuestas; // Asignar respuestas
        respuestaCorrecta = correcta; // Asignar índice correcta
        onRespuesta = callback; // Asignar callback

        CargarPregunta(); // Cargar y mostrar
    }
}