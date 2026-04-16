using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MA_GroundManager : MonoBehaviour
{
    public float tiempo = 30f;
    public TextMeshProUGUI textoTiempo;
    public GameObject monedaPrefab;
    public List<Transform> puntosSpawn;
    public float tiempoEntreMonedas = 2f;
    public TextMeshProUGUI textoPuntaje;
    private float timerSpawn;

    void Start()
    {
        MA_SFXManager.instance.PlaySpawnGround();
        ActualizarPuntaje();
    }

    void Update()
    {
        tiempo -= Time.deltaTime;
        textoTiempo.text = "Tiempo: " + Mathf.Ceil(tiempo);

        textoPuntaje.text = "Puntos: " + MA_GameData.puntajeGuardado;

        timerSpawn -= Time.deltaTime;
        if (timerSpawn <= 0)
        {
            SpawnMoneda();
            timerSpawn = tiempoEntreMonedas;
        }

        if (tiempo <= 0)
        {
            RegresarAlJuego();
        }
    }

    void RegresarAlJuego()
    {
        SceneManager.LoadScene("MA_GameScene");
    }

    void SpawnMoneda()
    {
        if (puntosSpawn.Count == 0) return;

        int randomIndex = Random.Range(0, puntosSpawn.Count);
        Transform spawn = puntosSpawn[randomIndex];

        Instantiate(monedaPrefab, spawn.position, Quaternion.identity);

        MA_SFXManager.instance.PlayCoinSpawn();
    }
    void ActualizarPuntaje()
    {
        textoPuntaje.text = "Puntos: " + MA_GameData.puntajeGuardado;
    }

}
