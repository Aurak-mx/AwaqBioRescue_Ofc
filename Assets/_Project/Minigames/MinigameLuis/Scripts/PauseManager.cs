using UnityEngine;
using UnityEngine.SceneManagement;

// Maneja la pausa del juego
// Se activa con el boton de engranaje en la esquina superior derecha del HUD
public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;           // Panel con los botones de reanudar, reiniciar y menu
    public GameObject rulesPanel1;          // Panel de instrucciones #1 (mismo de MenuGame4)
    public GameObject rulesPanel2;          // Panel de instrucciones #2 (mismo de MenuGame4)
    public QuestionManager questionManager; // Para verificar que el juego no haya terminado

    private bool isPaused = false;

    // Alterna entre pausar y reanudar el juego
    // Se conecta al boton de engranaje en el Inspector
    public void TogglePause()
    {
        if (questionManager.IsGameEnded()) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            AudioManager.instance.PlayBoton(); // Sonido al abrir el menu de pausa
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }

    // Reanuda el juego y oculta el menu de pausa
    public void ResumeGame()
    {
        AudioManager.instance.PlayBoton(); // Sonido al reanudar
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    // Desde el menu de pausa: oculta la pausa y muestra el panel de instrucciones #1
    // El juego sigue pausado (Time.timeScale se queda en 0)
    public void OpenRules1()
    {
        AudioManager.instance.PlayBoton();
        pausePanel.SetActive(false);
        rulesPanel1.SetActive(true);
    }

    // Avanza del panel de instrucciones #1 al #2
    public void OpenRules2()
    {
        AudioManager.instance.PlayBoton();
        rulesPanel1.SetActive(false);
        rulesPanel2.SetActive(true);
    }

    // Cierra los paneles de instrucciones y vuelve al menu de pausa
    // El usuario decide si reanuda o no desde la pausa
    public void CloseRules()
    {
        AudioManager.instance.PlayBoton();
        rulesPanel1.SetActive(false);
        rulesPanel2.SetActive(false);
        pausePanel.SetActive(true);
    }

    // Reinicia la escena actual desde cero
    public void RestartGame()
    {
        AudioManager.instance.PlayBoton();
        Time.timeScale = 1f;
        Invoke("CargarEscena", 0.2f);
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Carga el menu principal
    public void GoToMenu()
    {
        AudioManager.instance.PlayBoton(); // Sonido al ir al menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("HubMinijuegos");
    }
}