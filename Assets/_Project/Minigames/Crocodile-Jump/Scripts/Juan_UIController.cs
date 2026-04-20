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
    public TextMeshProUGUI textoCofres; 
    public TextMeshProUGUI textoPuntos;

    [Header("Panel Pausa")]
    public GameObject panelPausa;

    void Start()
    {
        tiempo = Juan_GameControl.Instance.tiempoJuego;
        vidas = PlayerPrefs.GetInt("Vidas");

        barraTiempo.fillAmount = 1f; 
        ActualizarPuntos(Juan_GameControl.Instance.puntos);
    }
    public void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); 
        }

        timerCoroutine = StartCoroutine(MatchTime());
    }

    public void UpdateVidas()
    {
        int v = Juan_GameControl.Instance.GetVidasRestantes();
        
        for (int i = 0; i < vidasImagenes.Length; i++)
        {
            
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

        float minFill = 0f;
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
        Juan_SFXManager.Instance.PlayBoton();
        Time.timeScale = 0f; 
    }
    public void CerrarPausa()
    {
        panelPausa.SetActive(false);
        Juan_SFXManager.Instance.PlayBoton();
        Time.timeScale = 1f; 
    }
    public int CalcularBonusTiempo()
    {
        float fill = barraTiempo.fillAmount;

        if (fill > 0.679f)
        {
            return 200;
        }
        else if (fill > 0.325f)
        {
            return 100;
        }
        else
        {
            return 50;
        }
    }
    public void ReiniciarBtn()
    {
        Time.timeScale = 1f;
        Juan_GameControl.Instance.ResetearJuego();
        SceneManager.LoadScene("CocodrileGameScene");
    }
    public void IrHomeBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Juan_Menu");
    }
    public void IrInstrucciones()
    {
        Time.timeScale = 1f;

        if (Juan_GameControl.Instance != null)
        {
            Juan_GameControl.Instance.invencible = false;
            Juan_GameControl.Instance.puntos = 0;
            vidas = 3;
            Juan_GameControl.Instance.StopAllCoroutines(); 
        }

        SceneManager.LoadScene("Juan_Instrucciones");
    }
}
