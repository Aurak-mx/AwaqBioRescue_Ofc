using UnityEngine;
using UnityEngine.SceneManagement;

// Script para manejar la meta
public class Juan_Meta : MonoBehaviour
{
    private bool yaActivado = false;
    // Cuando el jugador llega a la meta
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (yaActivado) return;
        // Si el jugador llega a la meta
        if (other.CompareTag("Player"))
        {
            yaActivado = true;
            if (Juan_GameControl.Instance != null)
            {
                int puntosActuales = Juan_GameControl.Instance.puntos;

                int bonus = 0;

                if (Juan_GameControl.Instance.UIController != null)
                {
                    bonus = Juan_GameControl.Instance.UIController.CalcularBonusTiempo();
                }

                int total = puntosActuales + bonus;

                PlayerPrefs.SetInt("PuntosFinales", total);
            }
            SceneManager.LoadScene("Juan_Win");
        }
    }
}