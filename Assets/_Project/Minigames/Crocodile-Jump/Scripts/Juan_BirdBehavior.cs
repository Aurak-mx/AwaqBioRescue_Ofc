using UnityEngine;

public class Juan_BirdBehavior : MonoBehaviour
{
    public float velocidad;

    public float distMax;

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        // Si chocamos con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {   
            // Destruimos el enemigo
            GameObject.Destroy(this.gameObject);
            // Le quitamos una vida al jugador
            Juan_GameControl.Instance.Daño();
        }
    }

    void Update()
    {   
        // Movemos el enemigo
        this.transform.position += Vector3.left * Time.deltaTime * velocidad;

        // Si el enemigo llega al limite se destruye
        if (this.transform.position.x < distMax)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

}
