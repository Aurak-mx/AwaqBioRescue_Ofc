using System.Collections;
using UnityEngine;

public class Juan_Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minAltura;
    public float maxAltura;
    public float minSpawnTiempo;
    public float maxSpawnTiempo;

    void Start()
    {
        StartCoroutine(SpawnerTime());
    }

    IEnumerator SpawnerTime()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTiempo, maxSpawnTiempo));

        Instantiate(prefab, new Vector3(transform.position.x,
            transform.position.y + Random.Range(minAltura, maxAltura), 0), Quaternion.identity);

        StartCoroutine(SpawnerTime());
    }
}
