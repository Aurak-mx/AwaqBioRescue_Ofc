using UnityEngine;

public class Juan_CofresManager : MonoBehaviour
{
    public static Juan_CofresManager Instance;

    [Header("Padres de cofres")]
    public GameObject[] cofresPadre;

    [Header("UI")]
    public Juan_UIController uiController;

    int totalCofres;
    int cofresAbiertos;

   // Implementacion de singleton para que solo exista una instancia de la clase 
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Si ya existe una instancia, destruimos la que estaba
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Inicializamos los cofres
        cofresAbiertos = 0;
        ActivarCofreAleatorio();
    }

    // Funcion para activar un cofre aleatorio de los presets de cofres
    void ActivarCofreAleatorio()
    {
        // Desactivamos todos los presets de cofres
        foreach (GameObject c in cofresPadre)
        {
            c.SetActive(false);
        }

        // Escojemos un numero aleatorio y activamos el cofre correspondiente
        int index = Random.Range(0, cofresPadre.Length);
        GameObject cofreSeleccionado = cofresPadre[index];

        cofreSeleccionado.SetActive(true);

        // Obtenemos el total de cofres y el total de cofres abiertos
        totalCofres = cofreSeleccionado.GetComponentsInChildren<Juan_CofreInteraccion>().Length;
        cofresAbiertos = 0;

        // Actualizamos la UI
        ActualizarUI();
    }

    // Funcion para cuando abrimos un cofre
    public void CofreAbierto()
    {
        // Incrementamos el total de cofres abiertos y actualizamos la UI
        cofresAbiertos++;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (uiController != null)
        {
            // Llamamos a la UI para mostrar el progreso de los cofres abiertos
            uiController.MostrarProgresoCofres(cofresAbiertos, totalCofres);
        }
    }
    public void ResetearCofres()
    {
        // Detenemos todos los coroutines
        StopAllCoroutines();

        // Reiniciamos los cofres
        ActivarCofreAleatorio();
    }
}