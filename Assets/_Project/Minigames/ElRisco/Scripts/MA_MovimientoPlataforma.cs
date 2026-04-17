using UnityEngine;

public class MA_MovimientoPlataforma : MonoBehaviour
{
    public float radio = 2f;
    public float velocidad = 2f;

    public bool detener = false;
    public bool regresar = false;
    
    private Vector3 centro;
    private float angulo;
    public int idPlataforma;

    public enum TipoMovimiento
    {
        Vertical,
        Horizontal,
        Horizontal2,
        Circular,
        Diagonal
    }

    public TipoMovimiento tipoMovimiento;

    void Start()
    {
        centro = transform.position;
        angulo = Random.Range(0f, Mathf.PI * 2f);

        radio = Random.Range(1.5f, 2f);
        velocidad = Random.Range(1f, 2f);
    }

    void Update()
    {
        if (regresar)
        {
            transform.position = Vector3.Lerp(transform.position, centro, Time.deltaTime * 5f);

            if (Vector3.Distance(transform.position, centro) < 0.01f)
            {
                transform.position = centro;
                regresar = false;
                detener = true;
            }
            return;
        }

        if (detener) return;

        angulo += velocidad * Time.deltaTime;

        switch (tipoMovimiento)
        {
            case TipoMovimiento.Vertical:
                transform.position = centro + new Vector3(0, Mathf.Sin(angulo) * radio, 0);
                break;

            case TipoMovimiento.Horizontal:
                transform.position = centro + new Vector3(Mathf.Sin(angulo*1.2f) * (radio * 0.40f), 0, 0);
                break;
            
            case TipoMovimiento.Horizontal2:
                transform.position = centro + new Vector3(Mathf.Sin(angulo*2f) * (radio * 0.40f), 0, 0);
                break;

            case TipoMovimiento.Circular:
                float x = Mathf.Cos(angulo*1.3f) * radio*0.35f;
                float y = Mathf.Sin(angulo*1.3f) * radio;
                transform.position = centro + new Vector3(x, y, 0);
                break;

            case TipoMovimiento.Diagonal:
                float d = Mathf.Sin(angulo) * radio;
                transform.position = centro + new Vector3(d, d, 0);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            Object.FindFirstObjectByType<MA_PlatformManager>().JugadorEnPlataforma(idPlataforma);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);

            Object.FindFirstObjectByType<MA_PlatformManager>().SalirDePlataforma();
        }
    }

}
