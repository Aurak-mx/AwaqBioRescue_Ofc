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
                collision.transform.SetParent(null);
                playerActual = null;
            }
        }
    }
}