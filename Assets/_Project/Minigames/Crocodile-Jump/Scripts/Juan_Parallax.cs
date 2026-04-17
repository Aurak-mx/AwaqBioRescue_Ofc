using UnityEngine;

// Script para crear efecto de parallax en fondos, simulando profundidad
public class Juan_Parallax : MonoBehaviour
{
    // Variables privadas para calcular el movimiento
    private float lenght; // Ancho del sprite para repetir el fondo
    private float starpos; // Posición inicial en X
    public float parallaxEffect; // Factor de parallax (0 = fijo, 1 = sigue la cámara)
    // Inicializa la posición y el ancho del sprite
    void Start()
    {
        starpos = transform.position.x; // Guardar posición inicial
        lenght = GetComponent<SpriteRenderer>().bounds.size.x; // Obtener ancho del sprite
    }

    // Actualiza la posición del fondo para crear el efecto parallax y repetir el sprite
    void LateUpdate()
    {
        // Calcular distancia basada en la posición de la cámara y el efecto parallax
        float temp = Camera.main.transform.position.x * (1 - parallaxEffect);
        float dist = Camera.main.transform.position.x * parallaxEffect;
        // Mover el fondo según la distancia calculada
        transform.position = new Vector3(
            starpos + dist, transform.position.y, transform.position.z);

        // Repetir el sprite hacia la derecha si la cámara se aleja
        if(temp > starpos + lenght)
        {
            starpos += lenght;
        }
        // Repetir el sprite hacia la izquierda si la cámara se aleja
        else if(temp < starpos - lenght)
        {
            starpos -= lenght;
        }
    }
}
