using UnityEngine;

public class MA_Paracaidas : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MA_SFXManager.instance.PlayParacaidas();
            
            MA_PlatformManager manager = Object.FindFirstObjectByType<MA_PlatformManager>();
            manager.ActivarParacaidas();

            Destroy(gameObject);
        }
    }
}
