using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Juan_UIController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Sprite vidaGastada;
    public Image[] vidasImagenes;
    int vidas = 3;
    private Coroutine timerCoroutine;
    int tiempo;

    void Start()
    {
        tiempo = Juan_GameControl.Instance.tiempoJuego;
        vidas = PlayerPrefs.GetInt("Vidas");
        TextoActivo();
    }
    public void TextoActivo()
    {
        timerText.text = tiempo.ToString();
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
        TextoActivo();
        if (tiempo == 0)
        {
            SceneManager.LoadScene("CocodrileGameScene");
        }
        else
        {
            StartCoroutine(MatchTime());
        }
    }
}
