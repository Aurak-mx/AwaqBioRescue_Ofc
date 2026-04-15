using UnityEngine;

public class Juan_BirdBehavior : MonoBehaviour
{
    public float velocidad;

    public float distMax;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Destroy(this.gameObject);
            Juan_GameControl.Instance.PerderVida();
        }
    }

    void Update()
    {
        this.transform.position += Vector3.left * Time.deltaTime * velocidad;

        if (this.transform.position.x < distMax)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

}
