using UnityEngine;

public class Juan_PlataformaMovil : MonoBehaviour
{
    private Transform playerActual;

    private void LateUpdate()
    {
        if (playerActual != null)
        {
            // 🔒 Mantener al jugador sin rotación
            playerActual.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                playerActual = collision.transform;
                playerActual.SetParent(transform.parent);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform == playerActual)
            {
                // Desvincular al jugador de la plataforma
                collision.transform.SetParent(null);

                // Reiniciar rotación al salir de la plataforma
                collision.transform.rotation = Quaternion.identity;

                playerActual = null;
            }
        }
    }
}