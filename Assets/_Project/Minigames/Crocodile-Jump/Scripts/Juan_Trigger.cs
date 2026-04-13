using UnityEngine;

public class Juan_Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Buscamos el componente de Respawn y lo mandamos de vuelta
            Juan_PlayerRespawn respawn = other.GetComponent<Juan_PlayerRespawn>();
            if (respawn != null)
            {
                respawn.VolverAlPuntoSeguro();
                Juan_GameControl.Instance.PerderVida();
            }
        }
    }
}
