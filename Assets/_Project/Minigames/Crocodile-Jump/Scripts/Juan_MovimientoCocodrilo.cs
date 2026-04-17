using UnityEngine;

// Script para manejar el movimiento del cocodrilo en patrulla horizontal
public class Juan_MovimientoCocodrilo : MonoBehaviour
{
    // Configuración del movimiento de patrulla
    [Header("Ajustes de Movimiento")]
    public float velocidad = 3f; 
    public float distanciaPatrulla = 5f; 

    
    private Vector2 posicionInicial; 
    private bool moviendoDerecha = true; 
    private SpriteRenderer renderizadoSprite; 

    // Inicializa la posición inicial y obtiene el componente SpriteRenderer
    void Start()
    {
        posicionInicial = transform.position; // Guardar posición de inicio
        renderizadoSprite = GetComponent<SpriteRenderer>(); // Obtener sprite para voltearlo
    }

    // Actualiza el movimiento cada frame: patrulla horizontal entre límites
    void Update()
    {
        // Calcular límites de patrulla basados en la posición inicial
        float limiteDerecho = posicionInicial.x + distanciaPatrulla;
        float limiteIzquierdo = posicionInicial.x - distanciaPatrulla;

        // Movimiento hacia la derecha
        if (moviendoDerecha)
        {
            transform.Translate(Vector2.right * velocidad * Time.deltaTime); // Mover a la derecha
            renderizadoSprite.flipX = false; // Sprite mirando a la derecha

            // Cambiar dirección al llegar al límite derecho
            if (transform.position.x >= limiteDerecho)
            {
                moviendoDerecha = false;
            }
        }
        else
        {
            // Movimiento hacia la izquierda
            transform.Translate(Vector2.left * velocidad * Time.deltaTime); // Mover a la izquierda
            renderizadoSprite.flipX = true; // Sprite mirando a la izquierda

            // Cambiar dirección al llegar al límite izquierdo
            if (transform.position.x <= limiteIzquierdo)
            {
                moviendoDerecha = true;
            }
        }
    }

    // Dibuja gizmos en el editor para visualizar el área de patrulla (solo cuando está seleccionado)
    void OnDrawGizmosSelected()
    {
        // Usar posición inicial si está en ejecución, sino la actual
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(posicionInicial, new Vector3(distanciaPatrulla * 2, 1f, 0f)); // Dibujar caja de patrulla
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(distanciaPatrulla * 2, 1f, 0f)); // Dibujar caja de patrulla
        }
    }
}