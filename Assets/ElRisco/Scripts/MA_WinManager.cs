using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MA_WinManager : MonoBehaviour
{
    public TextMeshProUGUI textoPuntajeFinal;

    void Start()
    {
        textoPuntajeFinal.text = "Puntos: " + MA_GameData.puntajeGuardado;
    }

    public void IrHome()
    {
        MA_SFXManager.instance.PlayButtonClick();
        Time.timeScale = 1f;
        MA_GameData.puntajeGuardado = 0;
        MA_GameData.vidasGuardadas = 0;
        MA_GameData.nivelGuardado = 0;
        SceneManager.LoadScene("MA_Home");
    }
}
