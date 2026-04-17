using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script para manejar la pantalla de Menu
public class Juan_MenuUI : MonoBehaviour
{
    public void JugarBtn()
    {
        SceneManager.LoadScene("Juan_Instrucciones");
    }
    public void ExitBtn()
    {
        SceneManager.LoadScene("HubMinijuegos");
    }
}
