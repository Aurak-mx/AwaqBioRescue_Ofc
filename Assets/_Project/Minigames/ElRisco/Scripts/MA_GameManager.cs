using UnityEngine;
using UnityEngine.SceneManagement;

public class MA_GameManager : MonoBehaviour
{
    public GameObject panelInfo1;
    public GameObject panelInfo2;


    public void GameScene()
    {
        MA_SFXManager.instance.PlayButtonClick();
        SceneManager.LoadScene("MA_GameScene");
    }
    public void HubScene()
    {
        MA_SFXManager.instance.PlayButtonClick();
        MA_SFXManager.instance.StopMusic();
        SceneManager.LoadScene("HubMinijuegos");
    }

    public void infoButton()
    {
        MA_SFXManager.instance.PlayButtonClick();
        panelInfo1.SetActive(true);
    }
    public void IrAPagina2()
    {
        MA_SFXManager.instance.PlayButtonClick();

        panelInfo1.SetActive(false);
        panelInfo2.SetActive(true);
    }
    public void CerrarInfo()
    {
        MA_SFXManager.instance.PlayButtonClick();

        panelInfo1.SetActive(false);
        panelInfo2.SetActive(false);
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
