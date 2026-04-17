using System.Collections;
using UnityEngine;

public class Juan_EnemigoPlanta : MonoBehaviour
{
    // Configuración de movimiento: velocidad y distancia de patrulla
    [Header("Configuración de Movimiento")]
    public float velocidadPatrulla = 2f; 
    public float distanciaPatrulla = 3f; 
    
    // Configuración de ataque: rango de visión y capa del jugador
    [Header("Configuración de Ataque")]
    public float rangoVision = 4f; 
    public LayerMask capaJugador; 

    
    private Vector2 posicionInicial;
    
    public bool moviendoDerecha = true;
    
    
    private SpriteRenderer renderizador; 
    private Animator animador; 
    
    private bool jugadorDetectado = false;
    
    public float rangoAtaque = 1.5f;
    
    private bool tocandoJugador = false;

    // Método que se ejecuta al iniciar el objeto, inicializa componentes y posición
    void Start()
    {
        // Guardamos la posición inicial
        posicionInicial = transform.position;
        renderizador = GetComponent<SpriteRenderer>(); // Obtenemos el componente SpriteRenderer
        animador = GetComponent<Animator>();            // Obtenemos el componente Animator
    }

    // Método que se ejecuta cada frame
    void Update()
    {
        // Si está tocando al jugador, no se mueve
        if (tocandoJugador) return;

        // Rayo de visión para detectar al jugador
        Vector3 origenRayo = transform.position + new Vector3(0, 1f, 0); 

        // Lanzar raycasts hacia derecha e izquierda para detectar al jugador
        RaycastHit2D hitDerecha = Physics2D.Raycast(origenRayo, Vector2.right, rangoVision, capaJugador);
        RaycastHit2D hitIzquierda = Physics2D.Raycast(origenRayo, Vector2.left, rangoVision, capaJugador);

        // Selecciona el rayo que detectó al jugador (derecha o izquierda)
        RaycastHit2D hit = hitDerecha.collider != null ? hitDerecha : hitIzquierda;

        // Si el rayo detectó al jugador
        if (hit.collider != null)
        {
            // Jugador detectado, cambiamos estado
            jugadorDetectado = true;

            // Determinar dirección del jugador
            bool jugadorALaDerecha = hit.collider.transform.position.x > transform.position.x;

            // Ajustar dirección de movimiento y volteo del sprite hacia el jugador
            moviendoDerecha = jugadorALaDerecha;
            renderizador.flipX = !jugadorALaDerecha;

            // Activar animación de ataque
            Atacar();

            // Calcular distancia al jugador
            float distanciaAlJugador = Vector2.Distance(transform.position, hit.collider.transform.position);

            // Si está dentro del rango de ataque, infligir daño al jugador
            if (distanciaAlJugador <= rangoAtaque)
            {
                Juan_GameControl.Instance.Daño();
            }
        }
        else
        {
            // No se detecta al jugador, volver a patrullar
            jugadorDetectado = false;
            Patrullar();
        }
    }

    // Método para patrullar entre límites definidos
    void Patrullar()
    {
        // Activar animación de caminar y desactivar ataque
        animador.SetBool("Caminando", true);
        animador.SetBool("Atacando", false);

        // Calcular límites de patrulla basados en la posición inicial
        float limiteDerecho = posicionInicial.x + distanciaPatrulla;
        float limiteIzquierdo = posicionInicial.x - distanciaPatrulla;

        // Si se está moviendo hacia la derecha
        if (moviendoDerecha)
        {
            // Mover hacia la derecha a velocidad constante
            transform.Translate(Vector2.right * velocidadPatrulla * Time.deltaTime);
            // Mantener sprite mirando a la derecha
            renderizador.flipX = false; 

            // Si alcanza el límite derecho, cambiar dirección
            if (transform.position.x >= limiteDerecho) moviendoDerecha = false;
        }
        else
        {
            // Mover hacia la izquierda
            transform.Translate(Vector2.left * velocidadPatrulla * Time.deltaTime);
            // Voltear sprite para mirar a la izquierda
            renderizador.flipX = true; 
            // Si alcanza el límite izquierdo, cambiar dirección
            if (transform.position.x <= limiteIzquierdo) moviendoDerecha = true;
        }
    }

    // Método para activar animación de ataque
    void Atacar()
    {
        animador.SetBool("Atacando", true);
        animador.SetBool("Caminando", false);
    }

    // Método para dibujar gizmos en el editor (rayo de visión)
    void OnDrawGizmos()
    {
        // Color verde para el gizmo
        Gizmos.color = Color.green;
        // Dirección del rayo según a donde se mueve 
        Vector3 direccion = moviendoDerecha ? Vector3.right : Vector3.left;
        // Origen del rayo (desde la cabeza)
        Vector3 origenRayo = transform.position + new Vector3(0, 1f, 0);
        // Dibujar línea del rayo de visión
        Gizmos.DrawLine(origenRayo, origenRayo + direccion * rangoVision);
    }

    // Método que se ejecuta mientras hay colisión continua con otro objeto
    private void OnCollisionStay2D(Collision2D colision)
    {
        // Verificar si el objeto colisionado es el jugador
        if (colision.gameObject.CompareTag("Player"))
        {
            // Marcar que está tocando al jugador para detener movimiento
            tocandoJugador = true;

            // Determinar dirección del jugador
            bool jugadorALaDerecha = colision.transform.position.x > transform.position.x;

            // Ajustar dirección y volteo del sprite según el jugador
            if (jugadorALaDerecha)
            {
                moviendoDerecha = true;
                renderizador.flipX = false;
            }
            else
            {
                moviendoDerecha = false;
                renderizador.flipX = true;
            }
        }
    }

    // Método que se ejecuta cuando termina la colisión con otro objeto
    private void OnCollisionExit2D(Collision2D colision)
    {
        // Verificar si el objeto que dejó de colisionar es el jugador
        if (colision.gameObject.CompareTag("Player"))
        {
            // Marcar que ya no está tocando al jugador, permitir movimiento
            tocandoJugador = false;
        }
    }
}