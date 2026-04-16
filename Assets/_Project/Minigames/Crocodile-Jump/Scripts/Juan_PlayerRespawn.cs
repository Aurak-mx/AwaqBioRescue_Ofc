using UnityEngine;

// Script para manejar el respawn del jugador en el último punto seguro
public class Juan_PlayerRespawn : MonoBehaviour
{
    
    private Vector2 ultimoPuntoSeguro;
    
    private Rigidbody2D rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtener Rigidbody
        ultimoPuntoSeguro = transform.position; // Punto inicial como seguro
    }

    // Al colisionar con suelo, actualiza el punto seguro si está encima
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            foreach (ContactPoint2D contacto in collision.contacts)
            {
                // Verificar si el contacto es desde arriba (normal Y > 0.5)
                if (contacto.normal.y > 0.5f)
                {
                    ultimoPuntoSeguro = transform.position + Vector3.up * 0.5f; // Guardar posición ligeramente arriba
                    break;
                }
            }
        }
    }

    // Método público para respawnear al último punto seguro
    public void VolverAlPuntoSeguro()
    {
        transform.position = ultimoPuntoSeguro; // Mover a la posición segura
        if (rb != null) rb.linearVelocity = Vector2.zero; // Resetear velocidad
    }
}
