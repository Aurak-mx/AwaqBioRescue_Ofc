using UnityEngine;

public class MA_CambioZona : MonoBehaviour
{
    public Transform nuevaPosicionCamara;
    public MA_PlatformManager manager;

    public Transform nuevoRespawn;

    public GameObject border;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.ResetJugadorPlataforma();

            manager.DestruirLevelActual();

            Camera.main.transform.position = new Vector3(
                nuevaPosicionCamara.position.x,
                nuevaPosicionCamara.position.y,
                Camera.main.transform.position.z
            );
            manager.puntoRespawn = nuevoRespawn;

            manager.SiguienteRonda();

            border.SetActive(true);

            Destroy(gameObject);
        }
    }
}
