using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MA_PlatformManager : MonoBehaviour
{
    public List<GameObject> plataformas;
    public float tiempoLimite = 5f;

    public GameObject jugador;

    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoPregunta;
    public TextMeshProUGUI[] textosRespuestas;

    public Image[] corazones;
    private int vidas;

    public Transform puntoRespawn;

    private float tiempoActual;
    private Coroutine contadorCoroutine;

    private int nivelActual = 0;

    string[] preguntas = {
        "¿Capital de Francia?",
        "¿Capital de España?",
        "¿Capital de Italia?",
        "¿Capital de Alemania?",
        "¿Capital de Japón?"
    };
    string[,] respuestas = {
        { "Madrid", "Paris", "Roma", "Berlin" },
        { "Sevilla", "Barcelona", "Madrid", "Valencia" },
        { "Turin", "Milan", "Napoles", "Roma" },
        { "Munich", "Berlin", "Hamburgo", "Frankfurt" },
        { "Tokio", "Osaka", "Kioto", "Nagoya" }
    };
    int[] respuestasCorrectas = { 1, 2, 3, 1, 0};
    private int plataformaCorrecta = 1;
    private int plataformaJugador = -1;
    public List<GameObject> plataformasIniciales;
    public List<GameObject> levelBlocks;
    private int puntaje = 0;
    public TextMeshProUGUI textoPuntaje;
    private int rachaCorrectas = 0;
    public GameObject paracaidasPrefab;
    public List<Transform> spawnParacaidasPorNivel;
    private bool tieneParacaidas = false;
    private bool clockPlayed = false;
    private List<int> ordenPreguntas = new List<int>();



    void Start()
    {
        for (int i = 0; i < preguntas.Length; i++)
        {
            ordenPreguntas.Add(i);
        }

        MezclarPreguntas();

        nivelActual = MA_GameData.nivelGuardado;
        puntaje = MA_GameData.puntajeGuardado;
        vidas = MA_GameData.vidasGuardadas;
        
        if (MA_GameData.vidasGuardadas > 0)
        {
            vidas = MA_GameData.vidasGuardadas;
        }
        else
        {
            vidas = corazones.Length;
        }


        ActualizarPuntaje();

        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].enabled = i < vidas;
        }


        StartGame();


    }

    void Update()
    {
        if (tiempoActual > 0)
        {
            tiempoActual -= Time.deltaTime;
            textoTiempo.text = "Tiempo: " + Mathf.Ceil(tiempoActual);
        }
         if (tiempoActual <= 5f && !clockPlayed && nivelActual < 5)
        {
            MA_SFXManager.instance.PlayClock();
            clockPlayed = true;
        }
    }

    public void StartGame()
    {
        clockPlayed = false;

        tiempoActual = tiempoLimite;

        int preguntaIndex = ordenPreguntas[nivelActual];

        textoPregunta.text = preguntas[preguntaIndex];

        List<int> indices = new List<int>() { 0, 1, 2, 3 };

        for (int i = 0; i < indices.Count; i++)
        {
            int rand = Random.Range(0, indices.Count);
            int temp = indices[i];
            indices[i] = indices[rand];
            indices[rand] = temp;
        }

        for (int i = 0; i < textosRespuestas.Length; i++)
        {
            textosRespuestas[i].text = respuestas[preguntaIndex, indices[i]];
        }

        int correctaOriginal = respuestasCorrectas[preguntaIndex];

        for (int i = 0; i < indices.Count; i++)
        {
            if (indices[i] == correctaOriginal)
            {
                plataformaCorrecta = i;
                break;
            }
        }

        contadorCoroutine = StartCoroutine(Contador());

        ActualizarPuntaje();

        ConfigurarMovimientoPlataformas();

    }

    void ActualizarPuntaje()
    {
        textoPuntaje.text = "Puntos: " + puntaje;
    }


    IEnumerator Contador()
    {
        yield return new WaitForSeconds(tiempoLimite);

        jugador.GetComponent<MA_PlayerControl>().enabled = false;

        for (int i = 0; i < plataformas.Count; i++)
        {
            MA_MovimientoPlataforma mov = plataformas[i].GetComponent<MA_MovimientoPlataforma>();
            if (mov != null)
                mov.regresar = true;
        }

        yield return new WaitForSeconds(1.5f);

        ResolverPlataformas();
    }

    void ResolverPlataformas()
    {
        if (plataformaJugador != plataformaCorrecta)
        {
            MA_SFXManager.instance.PlayWrong();

            jugador.transform.SetParent(null);

            for (int i = 0; i < plataformas.Count; i++)
            {
                plataformas[i].GetComponent<Collider2D>().enabled = false;
            }

            for (int i = 0; i < plataformasIniciales.Count; i++)
            {
                plataformasIniciales[i].GetComponent<Collider2D>().enabled = false;
            }
        }

        else
        {
            MA_SFXManager.instance.PlayCorrect();

            puntaje += 200;
            MA_GameData.puntajeGuardado = puntaje;

            rachaCorrectas++;


            ActualizarPuntaje();

            if (rachaCorrectas == 3)
            {
                SpawnParacaidas();
                rachaCorrectas = 0; 
            }

            for (int i = 0; i < levelBlocks.Count; i++)
            {
                if (levelBlocks[i] != null)
                {
                    levelBlocks[i].SetActive(false);
                }
            }

            
            NuevaRonda();
        }
    }



    public void JugadorEnPlataforma(int id)
    {
        plataformaJugador = id;
        Debug.Log("Jugador en plataforma: " + id);
    }
    public void SiguienteRonda()
    {
        clockPlayed = false;
        nivelActual++;

        MA_SFXManager.instance.PlayLevelUp();
        
        if (nivelActual >= 5)
        {
            MA_GameData.puntajeGuardado = puntaje;
            MA_GameData.vidasGuardadas = vidas;

            Time.timeScale = 1f;

            SceneManager.LoadScene("MA_Win");
            return;
        }

        for (int i = 0; i < levelBlocks.Count; i++)
        {
            if (levelBlocks[i] != null)
            {
                levelBlocks[i].SetActive(true);
            }
        }

        ConfigurarMovimientoPlataformas();

        ResetNivel();

        StartGame();

    }

    public void ResetJugadorPlataforma()
    {
        plataformaJugador = -1;
    }

    public void ResetGame()
    {
        if (contadorCoroutine != null)
            StopCoroutine(contadorCoroutine);

        tiempoActual = tiempoLimite;
        textoTiempo.text = "Tiempo: " + tiempoLimite;

        jugador.transform.position = puntoRespawn.position;

        Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;

        jugador.GetComponent<MA_PlayerControl>().enabled = true;


        for (int i = 0; i < plataformas.Count; i++)
        {
            Collider2D col = plataformas[i].GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;

            MA_MovimientoPlataforma mov = plataformas[i].GetComponent<MA_MovimientoPlataforma>();
            if (mov != null)
            {
                mov.detener = false;
                mov.regresar = false;
            }
        }

        plataformasIniciales[nivelActual].GetComponent<Collider2D>().enabled = true;


        for (int i = 0; i < corazones.Length; i++)
            corazones[i].enabled = true;


        StartGame();
    }

    public void PerderVida()
    {
        if (vidas <= 0) return;

        vidas--;

        MA_SFXManager.instance.PlayLoseLife();

        corazones[vidas].enabled = false;

        rachaCorrectas = 0;
        
        ResetNivel();

        if (vidas <= 0)
        {
            MA_SFXManager.instance.PlayGameOver();
            
            MA_GameOverManager gm = Object.FindFirstObjectByType<MA_GameOverManager>();
            if (gm != null)
            {
                gm.MostrarGameOver();
            }          
        }

        puntaje -= 100;        
        if (puntaje < 0) puntaje = 0;

        MA_GameData.puntajeGuardado = puntaje;

        ActualizarPuntaje();
    }
    void ResetNivel()
    {
        clockPlayed = false;
        if (levelBlocks[nivelActual] != null)
        {
            levelBlocks[nivelActual].SetActive(true);
        }

        tiempoActual = tiempoLimite;
        textoTiempo.text = "Tiempo: " + tiempoLimite;

        for (int i = 0; i < plataformas.Count; i++)
        {
            Collider2D col = plataformas[i].GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;

            MA_MovimientoPlataforma mov = plataformas[i].GetComponent<MA_MovimientoPlataforma>();
            if (mov != null)
            {
                mov.detener = false;
                mov.regresar = false;
            }
        }

        for (int i = 0; i < plataformasIniciales.Count; i++)
        {
            Collider2D col = plataformasIniciales[i].GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;
        }

        jugador.transform.position = puntoRespawn.position;

        Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;

        jugador.transform.SetParent(null);
        jugador.GetComponent<MA_PlayerControl>().enabled = true;

        plataformaJugador = -1;

        if (contadorCoroutine != null)
            StopCoroutine(contadorCoroutine);

        contadorCoroutine = StartCoroutine(Contador());
    }

    void NuevaRonda()
    {
        textoTiempo.text = "Tiempo: -"; 

        if (contadorCoroutine != null)
            StopCoroutine(contadorCoroutine);

       
        jugador.GetComponent<MA_PlayerControl>().enabled = true;

        
        for (int i = 0; i < plataformas.Count; i++)
        {
            Collider2D col = plataformas[i].GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;

            MA_MovimientoPlataforma mov = plataformas[i].GetComponent<MA_MovimientoPlataforma>();
            if (mov != null)
            {
                mov.detener = true;
                mov.regresar = false;
            }
        }

        plataformasIniciales[nivelActual].GetComponent<Collider2D>().enabled = true;

        plataformaJugador = -1;
    }
    public void DestruirLevelActual()
    {
        if (nivelActual < levelBlocks.Count && levelBlocks[nivelActual] != null)
        {
            Destroy(levelBlocks[nivelActual]);
        }
    }
    public void SalirDePlataforma()
    {
        plataformaJugador = -1;
    }

    void SpawnParacaidas()
    {
        if (nivelActual < spawnParacaidasPorNivel.Count)
        {
            Transform spawn = spawnParacaidasPorNivel[nivelActual];
            Instantiate(paracaidasPrefab, spawn.position, Quaternion.identity);
        }
    }

    public void ActivarParacaidas()
    {
        tieneParacaidas = true;
    }
    public bool TieneParacaidas()
    {
        return tieneParacaidas;
    }

   public void UsarParacaidas()
    {
        tieneParacaidas = false;

        MA_GameData.nivelGuardado = nivelActual;
        MA_GameData.puntajeGuardado = puntaje;
        MA_GameData.vidasGuardadas = vidas;

        plataformaJugador = -1;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MA_Ground");
    }

    public int GetNivelActual()
    {
        return nivelActual;
    }
    void ConfigurarMovimientoPlataformas()
    {
        for (int i = 0; i < plataformas.Count; i++)
        {
            MA_MovimientoPlataforma mov = plataformas[i].GetComponent<MA_MovimientoPlataforma>();

            if (mov != null)
            {
                switch (nivelActual)
                {
                    case 0:
                        mov.tipoMovimiento = MA_MovimientoPlataforma.TipoMovimiento.Vertical;
                        break;

                    case 1:
                        mov.tipoMovimiento = MA_MovimientoPlataforma.TipoMovimiento.Horizontal;
                        break;

                    case 2:
                        mov.tipoMovimiento = MA_MovimientoPlataforma.TipoMovimiento.Horizontal2;
                        break;

                    case 3:
                        mov.tipoMovimiento = MA_MovimientoPlataforma.TipoMovimiento.Circular;
                        break;

                    case 4:
                        mov.tipoMovimiento = MA_MovimientoPlataforma.TipoMovimiento.Diagonal;
                        break;
                }
            }
        }
    }
    void MezclarPreguntas()
    {
        for (int i = 0; i < ordenPreguntas.Count; i++)
        {
            int randomIndex = Random.Range(0, ordenPreguntas.Count);
            int temp = ordenPreguntas[i];
            ordenPreguntas[i] = ordenPreguntas[randomIndex];
            ordenPreguntas[randomIndex] = temp;
        }
    }
}
