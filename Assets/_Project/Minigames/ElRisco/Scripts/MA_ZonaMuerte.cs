using UnityEngine;

public class MA_ZonaMuerte : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        MA_PlatformManager manager = Object.FindFirstObjectByType<MA_PlatformManager>();

        if (manager.TieneParacaidas())
        {
            manager.UsarParacaidas();
        }
        else
        {
            manager.PerderVida();
        }
    }
}

}
