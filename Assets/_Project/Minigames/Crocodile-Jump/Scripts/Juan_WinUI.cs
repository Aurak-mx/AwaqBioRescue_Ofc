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

    int puntos;

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
        }
        else if (puntos >= 400)
        {
            imagenMedalla.sprite = medallaPlata;
        }
        else
        {
            imagenMedalla.sprite = medallaBronce;
        }
    }

    // 🔁 Botón Reintentar
    public void Reintentar()
    {
        // Reset puntos
        if (Juan_GameControl.Instance != null)
        {
            Juan_GameControl.Instance.puntos = 0;
        }

        SceneManager.LoadScene("CocodrileGameScene");
    }

    // 🏠 Botón Home
    public void IrHome()
    {
        SceneManager.LoadScene("MainMenu"); // cambia por tu escena real
    }
}