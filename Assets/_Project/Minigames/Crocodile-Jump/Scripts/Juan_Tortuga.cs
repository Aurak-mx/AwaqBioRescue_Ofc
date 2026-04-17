using UnityEngine;

// Tortuga que patrulla horizontalmente y permite al jugador montarse encima
public class Juan_Tortuga : MonoBehaviour
{
    // Configuración del movimiento de patrulla
    [Header("Ajustes de Movimiento")]
    public float velocidad = 3f; 
    public float distanciaPatrulla = 5f; 

   
    private Vector2 posicionInicial; 
    private bool moviendoDerecha = true; 
    private SpriteRenderer renderizadoSprite; 

    // Inicializa posición y sprite
    void Start()
    {
        posicionInicial = transform.position;
        renderizadoSprite = GetComponent<SpriteRenderer>();
    }

    // Actualiza el movimiento
    void Update()
    {
        
        float limiteDerecho = posicionInicial.x + distanciaPatrulla;
        float limiteIzquierdo = posicionInicial.x - distanciaPatrulla;

        
        if (moviendoDerecha)
        {
           
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            
            renderizadoSprite.flipX = false;

            
            if (transform.position.x >= limiteDerecho)
            {
                moviendoDerecha = false;
            }
        }
        else
        {
            
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);
            
            renderizadoSprite.flipX = true;

            
            if (transform.position.x <= limiteIzquierdo)
            {
                moviendoDerecha = true;
            }
        }
    }

    // Dibuja gizmos en el editor
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(posicionInicial, new Vector3(distanciaPatrulla * 2, 1f, 0f));
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(distanciaPatrulla * 2, 1f, 0f));
        }
    }
    // Al entrar en colisión, verifica si es el jugador y lo hace hijo de la tortuga
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (collision.contacts[0].normal.y < -0.5f)
            {
                collision.transform.SetParent(this.transform);
            }
        }
    }

    // Al salir de la colisión, libera al jugador del parentesco
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }


}
