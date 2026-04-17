using UnityEngine;
using UnityEngine.SceneManagement;

public class MA_GameOverManager : MonoBehaviour
{
    public GameObject panelGameOver;

    public void MostrarGameOver()
    {
        panelGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Reintentar()
    {
        MA_SFXManager.instance.PlayButtonClick();
        Time.timeScale = 1f;
        MA_GameData.puntajeGuardado = 0;
        MA_GameData.vidasGuardadas = 0;
        MA_GameData.nivelGuardado = 0;
        SceneManager.LoadScene("MA_GameScene");
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
