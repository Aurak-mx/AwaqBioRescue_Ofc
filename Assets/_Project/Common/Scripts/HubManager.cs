using UnityEngine;
using UnityEngine.SceneManagement;
public class HubManager : MonoBehaviour
{
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
