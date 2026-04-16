using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Juan_MenuUI : MonoBehaviour
{
    public void JugarBtn()
    {
        SceneManager.LoadScene("Juan_Instrucciones");
    }
    public void ExitBtn()
    {
        SceneManager.LoadScene("MinigamesMenu");
    }
}
