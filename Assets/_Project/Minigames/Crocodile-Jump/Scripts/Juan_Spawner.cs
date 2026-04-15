using System.Collections;
using UnityEngine;

public class Juan_Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minAltura;
    public float maxAltura;
    public float minSpawnTiempo;
    public float maxSpawnTiempo;
    public float distMax;

    void Start()
    {
        StartCoroutine(SpawnerTime());
    }

    IEnumerator SpawnerTime()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTiempo, maxSpawnTiempo));

        GameObject obj = Instantiate(prefab, new Vector3(
            transform.position.x,
            transform.position.y + Random.Range(minAltura, maxAltura),
            0), Quaternion.identity);

        // 👇 AHORA SÍ EXISTE obj
        Juan_BirdBehavior bird = obj.GetComponent<Juan_BirdBehavior>();
        if (bird != null)
        {
            bird.distMax = distMax;
        }

        StartCoroutine(SpawnerTime());
    }
}
