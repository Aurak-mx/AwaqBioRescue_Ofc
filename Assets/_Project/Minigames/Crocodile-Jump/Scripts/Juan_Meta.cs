using UnityEngine;
using UnityEngine.SceneManagement;

public class Juan_Meta : MonoBehaviour
{
    private bool yaActivado = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (yaActivado) return;
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