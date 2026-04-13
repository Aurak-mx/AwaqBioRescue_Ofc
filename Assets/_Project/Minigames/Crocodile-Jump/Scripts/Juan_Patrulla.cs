using UnityEngine;

public class Juan_Patrulla : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 3f;         // Qué tan rápido nada
    public float distanciaPatrulla = 5f; // Distancia máxima a cada lado

    private Vector2 posicionInicial;
    private bool moviendoDerecha = true;
    private SpriteRenderer renderizadoSprite;

    void Start()
    {
        // Guardamos la posición donde empieza el cocodrilo
        posicionInicial = transform.position;
        // Obtenemos la referencia al SpriteRenderer para voltearlo
        renderizadoSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. Calculamos los límites de patrulla basados en la posición inicial
        float limiteDerecho = posicionInicial.x + distanciaPatrulla;
        float limiteIzquierdo = posicionInicial.x - distanciaPatrulla;

        // 2. Controlamos la dirección y el movimiento
        if (moviendoDerecha)
        {
            // Mover a la derecha
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            // Voltear el sprite para que mire a la derecha
            renderizadoSprite.flipX = false;

            // Si llegamos al límite derecho, cambiamos de dirección
            if (transform.position.x >= limiteDerecho)
            {
                moviendoDerecha = false;
            }
        }
        else
        {
            // Mover a la izquierda
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);
            // Voltear el sprite para que mire a la izquierda
            renderizadoSprite.flipX = true;

            // Si llegamos al límite izquierdo, cambiamos de dirección
            if (transform.position.x <= limiteIzquierdo)
            {
                moviendoDerecha = true;
            }
        }
    }

    // Opcional: Dibuja los límites en la ventana de Scene para referencia visual
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
}
