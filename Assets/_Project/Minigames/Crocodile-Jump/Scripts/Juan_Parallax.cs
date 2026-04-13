using UnityEngine;

public class Juan_Parallax : MonoBehaviour
{
    private float lenght;
    private float starpos;
    public float parallaxEffect;
    void Start()
    {
        starpos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void LateUpdate()
    {
        float temp = Camera.main.transform.position.x * (1 - parallaxEffect);
        float dist = Camera.main.transform.position.x * parallaxEffect;
        transform.position = new Vector3(
            starpos + dist, transform.position.y, transform.position.z);

        if(temp > starpos + lenght)
        {
            starpos += lenght;
        }
        else if(temp < starpos - lenght)
        {
            starpos -= lenght;
        }
    }
}
