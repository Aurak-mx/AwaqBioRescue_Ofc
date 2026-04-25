using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LoginUI : MonoBehaviour
{
    public APIManager apiManager;
    public TMP_InputField inputUsuario;
    public TMP_InputField inputPassword;

    public TextMeshProUGUI errorText;

    public void OnClickLogin()
    {
        apiManager.SendLogin(inputUsuario.text, inputPassword.text, (usuario) =>
        {
            if (usuario == null)
            {
                // Mostrar mensaje de error en pantalla
                errorText.gameObject.SetActive(true);
                errorText.text = "Usuario o contraseña incorrectos";
                return;
            }

            // Guardar sesión y cargar la siguiente escena
            PlayerPrefs.SetInt("id_usuario", usuario.id_usuario);
            PlayerPrefs.SetString("usuario", usuario.nombre);

            UnityEngine.SceneManagement.SceneManager.LoadScene("HubMinijuegos");
        });
    }

    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            OnClickLogin();
        }
    }
}