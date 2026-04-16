using UnityEngine;

public class Juan_ControlCamara : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    public Transform objetivo;          
    public Vector3 desfase = new Vector3(0, 0, -10); 

    [Header("Límites Horizontales (X)")]
    public float xMinima = -10f;        
    public float xMaxima = 50f;        

    [Header("Límites Verticales (Y)")]
    public float yMinima = 0f;          
    public float yMaxima = 5f;          

    void LateUpdate()
    {
        if (objetivo != null)
        {
            // Calcular la posición deseada
            // El objeto en este caso es el jugador
            float xDeseada = objetivo.position.x + desfase.x;
            float yDeseada = objetivo.position.y + desfase.y;

            
            // Limitar la posición en que se puede mover la camara
            float xLimitada = Mathf.Clamp(xDeseada, xMinima, xMaxima);
            float yLimitada = Mathf.Clamp(yDeseada, yMinima, yMaxima);

            // Movemos la camara
            transform.position = new Vector3(xLimitada, yLimitada, desfase.z);
        }
    }
}