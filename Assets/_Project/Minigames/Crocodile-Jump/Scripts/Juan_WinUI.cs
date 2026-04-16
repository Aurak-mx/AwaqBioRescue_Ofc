using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Juan_WinUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoPuntos;
    public Image imagenMedalla;

    [Header("Medallas")]
    public Sprite medallaBronce;
    public Sprite medallaPlata;
    public Sprite medallaOro;

    [Header("API")]
    public Juan_APIManager apiManager; 

    int puntos;
    int idMedalla;

    void Start()
    {
        puntos = PlayerPrefs.GetInt("PuntosFinales", 0);
        textoPuntos.text = "Puntos: " + puntos;
        AsignarMedalla();
    }

    void AsignarMedalla()
    {
        if (puntos >= 800)
        {
            imagenMedalla.sprite = medallaOro;
            idMedalla = 1;  // Oro
        }
        else if (puntos >= 400)
        {
            imagenMedalla.sprite = medallaPlata;
            idMedalla = 2;  // Plata
        }
        else
        {
            imagenMedalla.sprite = medallaBronce;
            idMedalla = 3;  // Bronce
        }

        // Enviar a la API después de asignar medalla
        int idUsuario = PlayerPrefs.GetInt("UserID", 1); 
        if (apiManager != null)
        {
            apiManager.SendPostMedalla(3, 1, idMedalla);
        }
    }

    // 🔁 Botón Reintentar
    public void Reintentar()
    {
        if (Juan_GameControl.Instance != null)
        {
            Juan_GameControl.Instance.puntos = 0;
        }
        SceneManager.LoadScene("CocodrileGameScene");
    }

    // 🏠 Botón Home
    public void IrHome()
    {
        SceneManager.LoadScene("MainMenu");
    }
}