using UnityEngine;
using UnityEngine.SceneManagement;

public class MA_GameManager : MonoBehaviour
{
    public void GameScene()
    {
        MA_SFXManager.instance.PlayButtonClick();
        SceneManager.LoadScene("MA_GameScene");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
