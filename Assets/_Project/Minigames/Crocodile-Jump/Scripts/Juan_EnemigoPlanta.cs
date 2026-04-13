using UnityEngine;

public class Juan_EnemigoPlanta : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadPatrulla = 2f;
    public float distanciaPatrulla = 3f;
    
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

    void Start()
    {
        posicionInicial = transform.position;
        renderizador = GetComponent<SpriteRenderer>();
        animador = GetComponent<Animator>();
    }

    void Update()
    {
        if (tocandoJugador) return;

        Vector3 origenRayo = transform.position + new Vector3(0, 1f, 0); 

        RaycastHit2D hitDerecha = Physics2D.Raycast(origenRayo, Vector2.right, rangoVision, capaJugador);
        RaycastHit2D hitIzquierda = Physics2D.Raycast(origenRayo, Vector2.left, rangoVision, capaJugador);

        RaycastHit2D hit = hitDerecha.collider != null ? hitDerecha : hitIzquierda;
        if (hit.collider != null)
        {
            jugadorDetectado = true;

            bool jugadorALaDerecha = hit.collider.transform.position.x > transform.position.x;

            moviendoDerecha = jugadorALaDerecha;
            renderizador.flipX = !jugadorALaDerecha;

            Atacar();

            float distanciaAlJugador = Vector2.Distance(transform.position, hit.collider.transform.position);

            if (distanciaAlJugador <= rangoAtaque)
            {
                Juan_GameControl.Instance.PerderVida();
            }
        }
        else
        {
            jugadorDetectado = false;
            Patrullar();
        }
    }

    void Patrullar()
    {
        animador.SetBool("Caminando", true);
        animador.SetBool("Atacando", false);

        float limiteDerecho = posicionInicial.x + distanciaPatrulla;
        float limiteIzquierdo = posicionInicial.x - distanciaPatrulla;

        if (moviendoDerecha)
        {
            transform.Translate(Vector2.right * velocidadPatrulla * Time.deltaTime);
            renderizador.flipX = false; 
            if (transform.position.x >= limiteDerecho) moviendoDerecha = false;
        }
        else
        {
            transform.Translate(Vector2.left * velocidadPatrulla * Time.deltaTime);
            renderizador.flipX = true; 
            if (transform.position.x <= limiteIzquierdo) moviendoDerecha = true;
        }
    }

    void Atacar()
    {
        animador.SetBool("Atacando", true);
        animador.SetBool("Caminando", false);
        Debug.Log("¡Jugador detectado!");
    }

    void OnDrawGizmos()
    {
        // Esto es para que tú veas el rayo en la escena (color verde si está todo OK)
        Gizmos.color = Color.green;
        Vector3 direccion = moviendoDerecha ? Vector3.right : Vector3.left;
        Vector3 origenRayo = transform.position + new Vector3(0, 1f, 0);
        Gizmos.DrawLine(origenRayo, origenRayo + direccion * rangoVision);
    }

    private void OnCollisionStay2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Player"))
        {
            tocandoJugador = true;

            bool jugadorALaDerecha = colision.transform.position.x > transform.position.x;

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

    private void OnCollisionExit2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Player"))
        {
            tocandoJugador = false;
        }
    }
}