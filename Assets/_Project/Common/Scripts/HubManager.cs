using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class HubManager : MonoBehaviour
{
    void Start()
    {
        textoUsuario.text = PlayerPrefs.GetString("usuario");
    }

    public TextMeshProUGUI textoUsuario;
    public void GoToElRisco()
    {
        SceneManager.LoadScene("MA_Home");
    }

    public void GoToCrocodileJump()
    {
        SceneManager.LoadScene("Juan_Menu");
    }

    public void GoToMinigameMarcelo()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToMinigameLuis()
    {
        SceneManager.LoadScene("Game4");
    }
}
