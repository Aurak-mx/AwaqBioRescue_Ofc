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
        posicionInicial = transform.position;
    }

    void Update()
    {
        tiempo += Time.deltaTime * velocidad;

        // 🔄 ROTACIÓN (balanceo)
        float angulo = Mathf.Sin(tiempo) * amplitudRotacion;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        // 🌿 MOVIMIENTO EN ARCO
        float x = Mathf.Sin(tiempo) * amplitudX;
        float y = -Mathf.Cos(tiempo) * amplitudY;

        transform.position = posicionInicial + new Vector3(x, y, 0);
    }
}