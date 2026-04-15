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

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        cofresAbiertos = 0;
        ActivarCofreAleatorio();
    }

    void ActivarCofreAleatorio()
    {
        // Apagar todos
        foreach (GameObject c in cofresPadre)
        {
            c.SetActive(false);
        }

        // Elegir uno random
        int index = Random.Range(0, cofresPadre.Length);
        GameObject cofreSeleccionado = cofresPadre[index];

        cofreSeleccionado.SetActive(true);

        // Contar cofres reales
        totalCofres = cofreSeleccionado.GetComponentsInChildren<Juan_CofreInteraccion>().Length;
        cofresAbiertos = 0;

        ActualizarUI();
    }

    public void CofreAbierto()
    {
        cofresAbiertos++;
        ActualizarUI();

        Debug.Log($"Cofres: {cofresAbiertos}/{totalCofres}");

        if (cofresAbiertos >= totalCofres)
        {
            Debug.Log("🎉 ¡Ganaste!");
        }
    }

    void ActualizarUI()
    {
        if (uiController != null)
        {
            uiController.MostrarProgresoCofres(cofresAbiertos, totalCofres);
        }
    }
    public void ResetearCofres()
    {
        StopAllCoroutines();

        ActivarCofreAleatorio();
    }
}