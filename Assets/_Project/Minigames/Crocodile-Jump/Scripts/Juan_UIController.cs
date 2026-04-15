using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Juan_UIController : MonoBehaviour
{
    public Image barraTiempo;
    public Sprite vidaGastada;
    public Image[] vidasImagenes;
    int vidas = 3;
    private Coroutine timerCoroutine;
    int tiempo;
    public TextMeshProUGUI textoCofres; // arrastra en inspector
    public TextMeshProUGUI textoPuntos;

    [Header("Panel Pausa")]
    public GameObject panelPausa;

    void Start()
    {
        tiempo = Juan_GameControl.Instance.tiempoJuego;
        vidas = PlayerPrefs.GetInt("Vidas");

        barraTiempo.fillAmount = 1f; // empieza llena
        ActualizarPuntos(Juan_GameControl.Instance.puntos);
    }
    public void StartTimer()
    {
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(MatchTime());
        }
    }

    public void UpdateVidas()
    {
        int v = Juan_GameControl.Instance.GetVidasRestantes();
        
        for (int i = 0; i < vidasImagenes.Length; i++)
        {
            // Si el índice es mayor o igual a las vidas que me quedan, apago el corazón
            if (i >= v)
            {
                vidasImagenes[i].sprite = vidaGastada;
            }
        }
    }

    IEnumerator MatchTime()
    {
        yield return new WaitForSeconds(1);

        tiempo--;

        float minFill = 0.238f;
        float porcentaje = (float)tiempo / Juan_GameControl.Instance.tiempoJuego;

        barraTiempo.fillAmount = minFill + (porcentaje * (1f - minFill));

        if (tiempo == 0)
        {
            Juan_GameControl.Instance.GameOver();
        }
        else
        {
            StartCoroutine(MatchTime());
        }
    }
    public void MostrarProgresoCofres(int abiertos, int total)
    {
        textoCofres.text = abiertos + "/" + total;
    }
    public void ActualizarPuntos(int puntos)
    {
        textoPuntos.text = puntos + " pts.";
    }

    public void AbrirPausa()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f; // ⏸️ pausa el juego
    }
    public void CerrarPausa()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f; // ▶️ reanuda el juego
    }
    public int CalcularBonusTiempo()
    {
        float fill = barraTiempo.fillAmount;

        if (fill > 0.688f)
        {
            return 200;
        }
        else if (fill > 0.441f)
        {
            return 100;
        }
        else
        {
            return 50;
        }
    }
}
