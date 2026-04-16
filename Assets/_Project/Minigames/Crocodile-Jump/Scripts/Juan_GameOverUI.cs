using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Juan_GameOverUI : MonoBehaviour
{
    public void Reintentar()
    {
        if (Juan_GameControl.Instance != null)
        {
            Juan_GameControl.Instance.puntos = 0;
        }

        SceneManager.LoadScene("CocodrileGameScene");
    }

    // 🏠 Botón Home
    public void IrHome()
    {
        SceneManager.LoadScene("Juan_Menu"); 
    }
}