using UnityEngine;

public class Juan_ControlCamara : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    public Transform objetivo;          // El jugador (Cocodrilo)
    public Vector3 desfase = new Vector3(0, 0, -10); // Distancia de la cámara

    [Header("Límites Horizontales (X)")]
    public float xMinima = -10f;        // Límite izquierdo del mapa
    public float xMaxima = 50f;         // Límite derecho del mapa

    [Header("Límites Verticales (Y)")]
    public float yMinima = 0f;          // Límite inferior del mapa
    public float yMaxima = 5f;          // Límite superior del mapa

    void LateUpdate()
    {
        if (objetivo != null)
        {
            // 1. Calculamos la posición donde la cámara "querría" estar
            float xDeseada = objetivo.position.x + desfase.x;
            float yDeseada = objetivo.position.y + desfase.y;

            // 2. Aplicamos los límites usando Mathf.Clamp
            // Esto obliga al valor a quedarse entre el mínimo y el máximo definido
            float xLimitada = Mathf.Clamp(xDeseada, xMinima, xMaxima);
            float yLimitada = Mathf.Clamp(yDeseada, yMinima, yMaxima);

            // 3. Aplicamos la posición final a la cámara
            transform.position = new Vector3(xLimitada, yLimitada, desfase.z);
        }
    }
}