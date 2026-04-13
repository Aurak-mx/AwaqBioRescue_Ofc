using UnityEngine;

public class Juan_PlayerRespawn : MonoBehaviour
{
    private Vector2 ultimoPuntoSeguro;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // La posición inicial es el primer punto seguro
        ultimoPuntoSeguro = transform.position;
    }

    // Cada vez que el jugador toque el suelo, actualizamos su posición segura
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            foreach (ContactPoint2D contacto in collision.contacts)
            {
                // Si el contacto viene desde abajo (estás parado encima)
                if (contacto.normal.y > 0.5f)
                {
                    ultimoPuntoSeguro = transform.position + Vector3.up * 0.5f;
                    break;
                }
            }
        }
    }

    public void VolverAlPuntoSeguro()
    {
        transform.position = ultimoPuntoSeguro;
        // Importante: resetear la velocidad para que no siga cayendo al aparecer
        if (rb != null) rb.linearVelocity = Vector2.zero; 
    }
}
