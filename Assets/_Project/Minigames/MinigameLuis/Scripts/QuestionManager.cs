using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Maneja toda la logica de preguntas, respuestas, y los paneles de ganar/perder
public class QuestionManager : MonoBehaviour
{
    [Header("Panel Preguntas")]
    public GameObject questionCanvas;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public Image timerBar;
    public GameObject button1Object;
    public GameObject button2Object;

    [Header("Panel GameOver")]
    public GameObject gameOverPanel;
    public GameObject gameOverButton1; // Boton reintentar
    public GameObject gameOverButton2; // Boton menu

    [Header("Panel Win")]
    public GameObject winPanel;
    public GameObject winButton1;      // Boton siguiente nivel
    public GameObject winButton2;      // Boton menu
    public TextMeshProUGUI totemText;
    public GameObject medalOro;
    public GameObject medalPlata;
    public GameObject medalBronce;

    [Header("Configuracion")]
    public float timeLimit = 10f;
    public float delayToShow = 1.5f;

    [Header("Referencias")]
    public ChainGrabSystem chainSystem;
    public GameTimer gameTimer;

    [Header("API")]
    public APIManager apiManager;

    int idMedalla;

    private List<Question> questionBank = new List<Question>();
    private Question currentQuestion;
    private bool correctIsButton1;
    private float timer;
    private bool timerRunning = false;
    private bool gameEnded = false;
    private bool questionActive = false;

    void Start()
    {
        LoadQuestions();
        questionCanvas.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        Invoke("ShowQuestion", delayToShow);
    }

    void Update()
    {
        if (!timerRunning) return;

        timer -= Time.deltaTime;
        timerBar.fillAmount = timer / timeLimit;

        if (timer <= 0)
        {
            timerRunning = false;
            OnAnswer(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        gameTimer.StopTimer();
        CancelInvoke();
        questionCanvas.SetActive(false);

        AudioManager.instance.PlayPerder(); // Sonido de derrota

        gameOverPanel.SetActive(true);

        gameOverButton1.GetComponent<Button>().onClick.RemoveAllListeners();
        gameOverButton2.GetComponent<Button>().onClick.RemoveAllListeners();
        gameOverButton1.GetComponent<Button>().onClick.AddListener(RetryLevel);
        gameOverButton2.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    public void ShowWin()
    {
        if (gameEnded) return;
        gameEnded = true;
        gameTimer.StopTimer();
        CancelInvoke();
        questionCanvas.SetActive(false);

        AudioManager.instance.PlayGanar(); // Sonido de victoria

        float timeRemaining = gameTimer.GetCurrentTime();
        medalOro.SetActive(false);
        medalPlata.SetActive(false);
        medalBronce.SetActive(false);

        if (timeRemaining >= 120f)
        {
            medalOro.SetActive(true);
            totemText.text = "1200 Totems";
            idMedalla = 1;
        }
        else if (timeRemaining >= 60f)
        {
            medalPlata.SetActive(true);
            totemText.text = "850 Totems";
            idMedalla = 2;
        }
        else
        {
            medalBronce.SetActive(true);
            totemText.text = "600 Totems";
            idMedalla = 3;
        }

        if (apiManager != null)
        {
            apiManager.SendPostMedalla(PlayerPrefs.GetInt("id_usuario"), 3, idMedalla);
        }

        winPanel.SetActive(true);

        winButton1.GetComponent<Button>().onClick.RemoveAllListeners();
        winButton2.GetComponent<Button>().onClick.RemoveAllListeners();
        winButton1.GetComponent<Button>().onClick.AddListener(NextLevel);
        winButton2.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    public void GoToMenu()
    {
        AudioManager.instance.PlayBoton(); // Sonido al ir al menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuGame4");
    }

    void NextLevel()
    {
        AudioManager.instance.PlayBoton();
        Time.timeScale = 1f;
        // SceneManager.LoadScene("SiguienteEscena");
        Debug.Log("Siguiente Nivel");
    }

    void RetryLevel()
    {
        AudioManager.instance.PlayBoton();
        Time.timeScale = 1f;
        Invoke("CargarEscena", 0.2f); // Espera 0.2s para que suene el boton
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void LoadQuestions()
    {
        questionBank.Add(new Question { questionText = "¿Qué anota un pasante?", correctAnswer = "Datos", wrongAnswer = "Chistes" });
        questionBank.Add(new Question { questionText = "¿Qué se monitorea en campo?", correctAnswer = "Fauna", wrongAnswer = "Semáforos" });
        questionBank.Add(new Question { questionText = "¿Qué protege una reserva?", correctAnswer = "Hábitat", wrongAnswer = "Asfalto" });
        questionBank.Add(new Question { questionText = "¿Qué usa un biólogo en campo?", correctAnswer = "Bitácora", wrongAnswer = "Control" });
        questionBank.Add(new Question { questionText = "¿Qué necesita una muestra?", correctAnswer = "Etiqueta", wrongAnswer = "Brillo" });
        questionBank.Add(new Question { questionText = "¿Qué estudia la ecología?", correctAnswer = "Relaciones", wrongAnswer = "Monedas" });
        questionBank.Add(new Question { questionText = "¿Qué ayuda a conservar?", correctAnswer = "Cuidado", wrongAnswer = "Ruido" });
        questionBank.Add(new Question { questionText = "¿Qué se identifica en campo?", correctAnswer = "Especies", wrongAnswer = "Marcas" });
        questionBank.Add(new Question { questionText = "¿Qué se evita en reserva?", correctAnswer = "Basura", wrongAnswer = "Sombra" });
        questionBank.Add(new Question { questionText = "¿Qué protege AWAQ?", correctAnswer = "Naturaleza", wrongAnswer = "Concreto" });
        questionBank.Add(new Question { questionText = "¿Qué mide un termómetro?", correctAnswer = "Temperatura", wrongAnswer = "Altura" });
        questionBank.Add(new Question { questionText = "¿Qué indica una huella?", correctAnswer = "Presencia", wrongAnswer = "Clima" });
        questionBank.Add(new Question { questionText = "¿Qué produce una planta?", correctAnswer = "Oxígeno", wrongAnswer = "Humo" });
        questionBank.Add(new Question { questionText = "¿Qué afecta un incendio?", correctAnswer = "Hábitat", wrongAnswer = "Marea" });
        questionBank.Add(new Question { questionText = "¿Qué contamina un río?", correctAnswer = "Desechos", wrongAnswer = "Piedras" });
        questionBank.Add(new Question { questionText = "¿Qué protege el suelo?", correctAnswer = "Raíces", wrongAnswer = "Vidrio" });
        questionBank.Add(new Question { questionText = "¿Qué requiere el muestreo?", correctAnswer = "Orden", wrongAnswer = "Velocidad" });
        questionBank.Add(new Question { questionText = "¿Qué registra una cámara trampa?", correctAnswer = "Fauna", wrongAnswer = "Nubes" });
        questionBank.Add(new Question { questionText = "¿Qué animal es mamífero?", correctAnswer = "Jaguar", wrongAnswer = "Iguana" });
        questionBank.Add(new Question { questionText = "¿Qué vive en humedales?", correctAnswer = "Anfibios", wrongAnswer = "Camellos" });
        questionBank.Add(new Question { questionText = "¿Qué indica biodiversidad alta?", correctAnswer = "Variedad", wrongAnswer = "Sequedad" });
        questionBank.Add(new Question { questionText = "¿Qué se revisa en monitoreo?", correctAnswer = "Cambios", wrongAnswer = "Ofertas" });
        questionBank.Add(new Question { questionText = "¿Qué altera un ecosistema?", correctAnswer = "Contaminación", wrongAnswer = "Neblina" });
        questionBank.Add(new Question { questionText = "¿Qué ayuda a reforestar?", correctAnswer = "Nativas", wrongAnswer = "Plásticas" });
        questionBank.Add(new Question { questionText = "¿Qué indica erosión?", correctAnswer = "Desgaste", wrongAnswer = "Sombra" });
        questionBank.Add(new Question { questionText = "¿Qué busca conservar AWAQ?", correctAnswer = "Hábitat", wrongAnswer = "Cemento" });
        questionBank.Add(new Question { questionText = "¿Qué debe tener un registro?", correctAnswer = "Fecha", wrongAnswer = "Dibujo" });
        questionBank.Add(new Question { questionText = "¿Qué revela una pluma?", correctAnswer = "Ave", wrongAnswer = "Reptil" });
        questionBank.Add(new Question { questionText = "¿Qué grupo bioindica agua sana?", correctAnswer = "Anfibios", wrongAnswer = "Gatos" });
        questionBank.Add(new Question { questionText = "¿Qué se cuida en una práctica?", correctAnswer = "Protocolo", wrongAnswer = "Moda" });
        questionBank.Add(new Question { questionText = "¿Qué especie llega primero tras disturbio?", correctAnswer = "Pionera", wrongAnswer = "Doméstica" });
        questionBank.Add(new Question { questionText = "¿Qué relación beneficia a ambos?", correctAnswer = "Mutualismo", wrongAnswer = "Parasitismo" });
        questionBank.Add(new Question { questionText = "¿Qué mide la humedad?", correctAnswer = "Higrómetro", wrongAnswer = "Barómetro" });
        questionBank.Add(new Question { questionText = "¿Qué se evita al muestrear?", correctAnswer = "Sesgo", wrongAnswer = "Silencio" });
        questionBank.Add(new Question { questionText = "¿Qué especie no es nativa?", correctAnswer = "Exótica", wrongAnswer = "Endémica" });
        questionBank.Add(new Question { questionText = "¿Qué indica un bioindicador?", correctAnswer = "Calidad", wrongAnswer = "Velocidad" });
        questionBank.Add(new Question { questionText = "¿Qué cadena inicia con plantas?", correctAnswer = "Trófica", wrongAnswer = "Metálica" });
        questionBank.Add(new Question { questionText = "¿Qué organismo descompone materia?", correctAnswer = "Hongo", wrongAnswer = "Halcón" });
        questionBank.Add(new Question { questionText = "¿Qué requiere una observación válida?", correctAnswer = "Evidencia", wrongAnswer = "Suerte" });
        questionBank.Add(new Question { questionText = "¿Qué protege mejor un hábitat?", correctAnswer = "Restauración", wrongAnswer = "Pavimento" });
    }

    public void ShowQuestion()
    {
        if (gameEnded) return;

        if (questionBank.Count == 0)
        {
            Debug.Log("Se acabaron las preguntas");
            return;
        }

        button1Object.SetActive(true);
        button2Object.SetActive(true);
        timerBar.gameObject.SetActive(true);
        questionText.fontSize = 27;

        button1Object.GetComponent<Button>().onClick.RemoveAllListeners();
        button2Object.GetComponent<Button>().onClick.RemoveAllListeners();
        button1Object.GetComponent<Button>().onClick.AddListener(OnButton1Click);
        button2Object.GetComponent<Button>().onClick.AddListener(OnButton2Click);

        int rand = Random.Range(0, questionBank.Count);
        currentQuestion = questionBank[rand];
        questionBank.RemoveAt(rand);

        correctIsButton1 = Random.value > 0.5f;
        questionText.text = currentQuestion.questionText;

        if (correctIsButton1)
        {
            button1Text.text = currentQuestion.correctAnswer;
            button2Text.text = currentQuestion.wrongAnswer;
        }
        else
        {
            button1Text.text = currentQuestion.wrongAnswer;
            button2Text.text = currentQuestion.correctAnswer;
        }

        questionCanvas.SetActive(true);
        timer = timeLimit;
        timerRunning = true;
        questionActive = true;

        AudioManager.instance.PlayPregunta(); // Sonido al mostrar nueva pregunta
    }

    public void OnButton1Click() { OnAnswer(correctIsButton1); }
    public void OnButton2Click() { OnAnswer(!correctIsButton1); }

    void OnAnswer(bool correct)
    {
        if (gameEnded) return;
        if (chainSystem.IsMoving()) return;

        questionActive = false;
        timerRunning = false;
        questionCanvas.SetActive(false);

        if (correct)
        {
            AudioManager.instance.PlayCorrecto(); // Sonido respuesta correcta
            chainSystem.MoveUp();
            Invoke("ShowQuestion", 2f);
        }
        else
        {
            AudioManager.instance.PlayIncorrecto(); // Sonido respuesta incorrecta
            if (chainSystem.IsAtBottom())
                ShowGameOver();
            else
            {
                chainSystem.MoveDown();
                Invoke("ShowQuestion", 2f);
            }
        }
    }

    public void PauseQuestion()
    {
        timerRunning = false;
        questionCanvas.SetActive(false);
    }

    public void ResumeQuestion()
    {
        if (!gameEnded)
        {
            questionCanvas.SetActive(true);
            timerRunning = true;
        }
    }

    public void IrAlJuego()
    {
        SceneManager.LoadScene("Game4");
    }
    public bool IsQuestionActive() { return questionActive; }
    public bool IsGameEnded() { return gameEnded; }
}
