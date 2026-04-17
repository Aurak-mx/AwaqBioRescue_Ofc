using UnityEngine;

public class MA_CambioZonaBorde : MonoBehaviour
{
    public MA_PlatformManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.ResetJugadorPlataforma();

            manager.DestruirLevelActual();
            
            manager.SiguienteRonda();

            Destroy(gameObject);
        }
    }
}
