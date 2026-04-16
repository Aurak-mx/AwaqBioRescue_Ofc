using UnityEngine;

// Script para manejar daño de objetos solidos escenas
public class Juan_Trigger : MonoBehaviour
{
    // Cuando el jugador colisiona con el trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Si el jugador entra al area del trigger vuelve al punto seguro y daño
            Juan_PlayerRespawn respawn = other.GetComponent<Juan_PlayerRespawn>();
            if (respawn != null)
            {
                respawn.VolverAlPuntoSeguro();
                Juan_GameControl.Instance.Daño();
            }
        }
    }
}
