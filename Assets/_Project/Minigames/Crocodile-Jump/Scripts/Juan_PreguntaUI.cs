using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Juan_PregunaUI : MonoBehaviour
{
    public TextMeshProUGUI preguntaTexto;
    public Button[] botones;

    // 🔒 Datos internos (ya no se editan en inspector)
    private string pregunta;
    private string[] respuestas;
    private int respuestaCorrecta;

    private Action<bool> onRespuesta;

    void CargarPregunta()
    {
        preguntaTexto.text = pregunta;

        for (int i = 0; i < botones.Length; i++)
        {
            int index = i;

            // 🔄 Reset botón
            botones[i].interactable = true;

            Image img = botones[i].GetComponent<Image>();
            img.color = Color.white;

            // 📝 Asignar texto
            if (respuestas != null && i < respuestas.Length)
            {
                var texto = botones[i].GetComponentInChildren<TextMeshProUGUI>();
                if (texto != null)
                    texto.text = respuestas[i];
            }

            // 🔘 Asignar evento
            botones[i].onClick.RemoveAllListeners();
            botones[i].onClick.AddListener(() => VerificarRespuesta(index));
        }
    }

    void VerificarRespuesta(int index)
    {
        // 🔒 Bloquear botones
        foreach (Button b in botones)
            b.interactable = false;

        bool esCorrecta = (index == respuestaCorrecta);

        // 🎨 Feedback visual
        for (int i = 0; i < botones.Length; i++)
        {
            Image img = botones[i].GetComponent<Image>();

            if (i == respuestaCorrecta)
            {
                img.color = Color.green; // correcta
            }
            else if (i == index)
            {
                img.color = Color.red; // la que elegiste mal
            }
        }

        Debug.Log(esCorrecta ? "Correcto" : "Incorrecto");

        // 📢 Avisar al cofre
        onRespuesta?.Invoke(esCorrecta);

        // ⏱️ Cerrar después de un momento
        StartCoroutine(CerrarConDelay());
    }

    IEnumerator CerrarConDelay()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        CerrarPanel();
    }

    void CerrarPanel()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    // 🔥 Método que llama el cofre
    public void SetPregunta(string nuevaPregunta, string[] nuevasRespuestas, int correcta, Action<bool> callback)
    {
        pregunta = nuevaPregunta;
        respuestas = nuevasRespuestas;
        respuestaCorrecta = correcta;
        onRespuesta = callback;

        CargarPregunta();
    }
}