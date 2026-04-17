using UnityEngine;

// Script para plataformas móviles que transportan al jugador, ajustando su parentesco
public class Juan_PlataformaMovil : MonoBehaviour
{
    // Referencia al transform del jugador actual sobre la plataforma
    private Transform playerActual;

    // Corrige la rotación del jugador para mantenerlo derecho mientras está en la plataforma
    private void LateUpdate()
    {
        if (playerActual != null)
        {
            playerActual.rotation = Quaternion.identity; // Resetear rotación a identidad (sin rotación)
        }
    }

    // Al entrar en colisión, verifica si es el jugador y lo hace hijo de la plataforma para que se mueva con ella
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Solo si el jugador está encima (posición Y mayor)
            if (collision.transform.position.y > transform.position.y)
            {
                playerActual = collision.transform; // Asignar jugador actual
                playerActual.SetParent(transform.parent); // Hacerlo hijo del padre de la plataforma
            }
        }
    }

    // Al salir de la colisión, libera al jugador del parentesco y resetea su rotación
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform == playerActual)
            {
                collision.transform.SetParent(null); // Quitar parentesco

                collision.transform.rotation = Quaternion.identity; // Resetear rotación

                playerActual = null; // Limpiar referencia
            }
        }
    }
}