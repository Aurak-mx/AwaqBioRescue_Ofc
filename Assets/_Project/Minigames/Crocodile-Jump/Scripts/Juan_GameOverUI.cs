using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script para manejar la pantalla de Game Over
public class Juan_GameOverUI : MonoBehaviour
{
    public void Reintentar()
    {
        if (Juan_GameControl.Instance != null)
        {
            Juan_GameControl.Instance.ResetearJuego();
        }

        SceneManager.LoadScene("CocodrileGameScene");
    }

    
    public void IrHome()
    {
        SceneManager.LoadScene("Juan_Menu"); 
    }
}