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
        // Obtenemos los puntos finales
        puntos = PlayerPrefs.GetInt("PuntosFinales", 0);
        // Actualizamos la UI
        textoPuntos.text = "Puntos: " + puntos;
        AsignarMedalla();
    }

    // Funcion para cambiar la imagen de la medalla
    void AsignarMedalla()
    {
        // Dependiendo de la cantidad de puntos obtenidos se asignara una medalla
        if (puntos >= 800)
        {
            imagenMedalla.sprite = medallaOro;
            idMedalla = 1;  
        }
        else if (puntos >= 400)
        {
            imagenMedalla.sprite = medallaPlata;
            idMedalla = 2;  
        }
        else
        {
            imagenMedalla.sprite = medallaBronce;
            idMedalla = 3;  
        }

        // Enviamos la medalla al servidor
        int idUsuario = PlayerPrefs.GetInt("UserID", 1); 
        if (apiManager != null)
        {
            apiManager.SendPostMedalla(3, 1, idMedalla);
        }
    }

    // Boton para reintentar el juego
    public void Reintentar()
    {
        if (Juan_GameControl.Instance != null)
        {
            Juan_GameControl.Instance.ResetearJuego();
        }
        SceneManager.LoadScene("CocodrileGameScene");
    }

    // Boton para ir a la pantalla principal
    public void IrHome()
    {
        SceneManager.LoadScene("Juan_Menu");
    }
}