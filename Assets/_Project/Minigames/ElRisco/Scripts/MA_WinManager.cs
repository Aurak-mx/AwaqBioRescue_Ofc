using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MA_WinManager : MonoBehaviour
{
    public TextMeshProUGUI textoPuntajeFinal;
    public Image imagenMedalla;
    public Sprite medallaBronze;
    public Sprite medallaSilver;
    public Sprite medallaGold;

    void Start()
    {
        textoPuntajeFinal.text = "Puntos: " + MA_GameData.puntajeGuardado;
        AsignarMedalla();
    }

    public void IrHome()
    {
        MA_SFXManager.instance.PlayButtonClick();
        Time.timeScale = 1f;
        MA_GameData.puntajeGuardado = 0;
        MA_GameData.vidasGuardadas = 0;
        MA_GameData.nivelGuardado = 0;
        SceneManager.LoadScene("MA_Home");
    }

    void AsignarMedalla()
    {
        if (MA_GameData.puntajeGuardado >= 1200)
        {
            imagenMedalla.sprite = medallaGold;
        }
        else if (MA_GameData.puntajeGuardado >=900)
        {
            imagenMedalla.sprite = medallaSilver;
        }
        else
        {
            imagenMedalla.sprite = medallaBronze;
        }
    }
}
