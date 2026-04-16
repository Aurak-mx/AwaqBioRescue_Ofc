using UnityEngine;

public class MA_Moneda : MonoBehaviour
{
    public int puntos = 20;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MA_GameData.puntajeGuardado += puntos;

            MA_SFXManager.instance.PlayCoin();

            Destroy(gameObject);
        }
    }
}
