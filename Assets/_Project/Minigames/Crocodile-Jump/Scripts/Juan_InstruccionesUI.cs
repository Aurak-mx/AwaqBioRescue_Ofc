using UnityEngine;
using UnityEngine.SceneManagement;

// Script para manejar la pantalla de Instrucciones
public class Juan_InstruccionesUI : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;

    void Start()
    {
        if (panel1 != null && panel2 != null)
        {
            panel2.SetActive(false);
            panel1.SetActive(true);
        }
    }

    public void Siguiente()
    {
        if (panel1 != null && panel2 != null)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
        }
    }

    public void EmpezarJuegoBtn()
    {
        Time.timeScale = 1f;

        if (Juan_GameControl.Instance != null)
        {
            Juan_GameControl.Instance.ResetearJuego();
        }

        SceneManager.LoadScene("CocodrileGameScene");
    }
}