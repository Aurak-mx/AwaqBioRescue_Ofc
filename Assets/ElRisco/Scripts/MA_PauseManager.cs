using UnityEngine;
using UnityEngine.SceneManagement;

public class MA_PauseManager : MonoBehaviour
{
    public GameObject panelPausa;

    private bool estaPausado = false;

    public void TogglePause()
    {
        MA_SFXManager.instance.PlayButtonClick();
        if (estaPausado)
            Reanudar();
        else
            Pausar();
    }

    public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        estaPausado = true;
    }

    public void Reanudar()
    {
        MA_SFXManager.instance.PlayButtonClick();
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        estaPausado = false;
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
