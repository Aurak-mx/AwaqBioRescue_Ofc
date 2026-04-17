using UnityEngine;
using UnityEngine.SceneManagement;

public class MA_SFXManager : MonoBehaviour
{
    public static MA_SFXManager instance;
    public AudioClip musicaHome;
    public AudioClip musicaGame;
    public AudioClip musicaGround;
    public AudioClip musicaWin;
    public AudioClip clockSound;
    public float clockVolume = 1f;
    private AudioSource musicSource;
    private AudioSource sfxSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip loseLifeSound;
    public AudioClip levelUpSound;
    public AudioClip paracaidasSound;
    public AudioClip spawnGroundSound;
    public AudioClip coinSound;
    public AudioClip coinSpawnSound;
    public AudioClip buttonClickSound;
    public AudioClip gameOverSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        AudioSource[] sources = GetComponents<AudioSource>();
        musicSource = sources[0];
        sfxSource = sources[1];
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        CambiarMusica(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CambiarMusica(scene.name);
    }

    void CambiarMusica(string nombreEscena)
    {
        AudioClip nuevaMusica = null;
        bool loop = true;

        switch (nombreEscena)
        {
            case "MA_Home":
                nuevaMusica = musicaHome;
                loop = true;
                break;

            case "MA_GameScene":
                nuevaMusica = musicaGame;
                loop = true;
                break;

            case "MA_Ground":
                nuevaMusica = musicaGround;
                loop = true;
                break;

            case "MA_Win":
                nuevaMusica = musicaWin;
                loop = false;
                break;
        }

        if (nuevaMusica != null && musicSource.clip != nuevaMusica)
        {
            musicSource.clip = nuevaMusica;
            musicSource.loop = loop;
            musicSource.volume = 0.30f;
            musicSource.Play();
        }
    }

    public void PlayClock()
    {
        sfxSource.PlayOneShot(clockSound, clockVolume);
    }
    public void PlayCorrect()
    {
        sfxSource.PlayOneShot(correctSound, 0.8f);
    }

    public void PlayWrong()
    {
        sfxSource.PlayOneShot(wrongSound, 0.8f);
    }
    public void PlayLoseLife()
    {
        sfxSource.PlayOneShot(loseLifeSound, 1f);
    }
    public void PlayLevelUp()
    {
        sfxSource.PlayOneShot(levelUpSound, 1f);
    }
    public void PlayParacaidas()
    {
        sfxSource.PlayOneShot(paracaidasSound, 1.25f);
    }
    public void PlaySpawnGround()
    {
        sfxSource.PlayOneShot(spawnGroundSound, 1f);
    }
    public void PlayCoin()
    {
        sfxSource.PlayOneShot(coinSound, 0.8f);
    }
    public void PlayCoinSpawn()
    {
        sfxSource.PlayOneShot(coinSpawnSound, 0.7f);
    }
    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickSound, 1f);
    }
    public void PlayGameOver()
    {
        sfxSource.PlayOneShot(gameOverSound, 1f);
    }

}
