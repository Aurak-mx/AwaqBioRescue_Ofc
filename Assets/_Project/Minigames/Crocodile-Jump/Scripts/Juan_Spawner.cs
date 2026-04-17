using System.Collections;
using UnityEngine;

// Spawnea objetos (pájaros) en posiciones aleatorias con intervalos variables
public class Juan_Spawner : MonoBehaviour
{
    public GameObject prefab; // Prefab a instanciar
    public float minAltura; 
    public float maxAltura; 
    public float minSpawnTiempo; 
    public float maxSpawnTiempo; 
    public float distMax; 

    // Inicia la corutina de spawn al comenzar
    void Start()
    {
        StartCoroutine(SpawnerTime());
    }

    // Corutina que espera un tiempo aleatorio, spawnea el objeto y se repite
    IEnumerator SpawnerTime()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTiempo, maxSpawnTiempo)); // Espera un tiempo aleatorio

        GameObject obj = Instantiate(prefab, new Vector3(
            transform.position.x,
            transform.position.y + Random.Range(minAltura, maxAltura),
            0), Quaternion.identity);

        // Configura el comportamiento si es un pájaro
        Juan_BirdBehavior bird = obj.GetComponent<Juan_BirdBehavior>();
        if (bird != null)
        {
            bird.distMax = distMax;
        }

        StartCoroutine(SpawnerTime()); // Reinicia la corutina
    }
}
