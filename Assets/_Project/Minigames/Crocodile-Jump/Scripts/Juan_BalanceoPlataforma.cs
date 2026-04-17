using UnityEngine;

public class Juan_BalanceoPlataforma : MonoBehaviour
{
    [Header("Movimiento general")]
    public float velocidad = 1f;

    [Header("Rotación")]
    public float amplitudRotacion = 25f;

    [Header("Movimiento en arco")]
    public float amplitudX = 0.5f;
    public float amplitudY = 0.2f;

    private float tiempo;
    private Vector3 posicionInicial;

    void Start()
    {
        // Guardar la posición inicial
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Actualizar el tiempo
        tiempo += Time.deltaTime * velocidad;


        // Actualizar la rotación en base al tiempo
        float angulo = Mathf.Sin(tiempo) * amplitudRotacion;

        // Rotamos el objeto
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        // Actualizar la posición en base al tiempo de "x" y "y"
        float x = Mathf.Sin(tiempo) * amplitudX;
        float y = -Mathf.Cos(tiempo) * amplitudY;

        // Actualizar la posición con una nueva posición
        transform.position = posicionInicial + new Vector3(x, y, 0);
    }
}