using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // States of the game
    public enum GameState { Playing, Paused, GameOver , Victory}
    public GameState currentState;
    public bool IsInDialogue { get; private set; } = false;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject victoryPanel;

    [Header("HUD UI")]
    [SerializeField] private int cantidadBoleadoras = 0;
    public HUDManager hudManager;

    [SerializeField] private FMODUnity.EventReference musica_menuPausa;
    private FMOD.Studio.EventInstance musicaPausaInstance;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        // make sure theres no two gamemanager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // do not destroy when switch scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentState = GameState.Playing; // initial state

        // make sure both panels are hidden at start
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

    }

    private void Update()
    {
        // pausing when press esc (only if not game over)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7") && currentState != GameState.GameOver)
        {
            TogglePause();
        }

        //TEST boleadoras UI
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    AddBoleadora();
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    RemoveBoleadora();
        //}
    }

    public void AddBoleadora()
    {
        if (cantidadBoleadoras < 3)
        {
            cantidadBoleadoras++;
            hudManager.ActualizarBoleadoras(cantidadBoleadoras);
        }
    }

    public void RemoveBoleadora()
    {
        if (cantidadBoleadoras > 0)
        {
            cantidadBoleadoras--;
            hudManager.ActualizarBoleadoras(cantidadBoleadoras);
        }
    }

    public int GetAmountBoleadoras()
    {
        return cantidadBoleadoras;
    }

    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            if (pausePanel != null) pausePanel.SetActive(true);

            // Pausar música de fondo y reproducir música de pausa
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PauseMusic();

                FindFirstObjectByType<PlayerHealth>()?.PausarLowLifeSFX();

                musicaPausaInstance = FMODUnity.RuntimeManager.CreateInstance(musica_menuPausa);
                musicaPausaInstance.start();
            }

            foreach (var zona in ZoneSFX.zonasActivas)
            {
                zona.PausarSfx();
            }
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);

            if (musicaPausaInstance.isValid())
            {
                musicaPausaInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                musicaPausaInstance.release();
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ResumeMusic();
                FindFirstObjectByType<PlayerHealth>()?.ReanudarLowLifeSFX();
            }

            foreach (var zona in ZoneSFX.zonasActivas)
            {
                zona.ReanudarSfx();
            }
        }
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f; // this pause the game
        Debug.Log("Fin del juego.");

        FindFirstObjectByType<PlayerHealth>()?.DetenerLowLifeSFX();

        // show game over panel
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void Victory()
    {
        currentState = GameState.Victory;
        Time.timeScale = 0f; // this pause the game
        Debug.Log("Fin del juego.");

        FindFirstObjectByType<PlayerHealth>()?.DetenerLowLifeSFX();

        // show game over panel
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    // this called by retry button
    public void RetryLevel()
    {
        Time.timeScale = 1f; // unpause before reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // this called by main menu button
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        // Detener música pausa si sigue activa
        if (musicaPausaInstance.isValid())
        {
            musicaPausaInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicaPausaInstance.release();
            musicaPausaInstance.clearHandle();
        }

        // Detener música general
        AudioManager.Instance?.StopMusic();

        // Resetear lowLife si está sonando
        FindFirstObjectByType<PlayerHealth>()?.DetenerLowLifeSFX();

        SceneManager.LoadScene("MainMenu"); // asegurate que este nombre esté bien
    }

    // this called by quit button
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

    public void SetDialogueState(bool isInDialogue)
        {
            IsInDialogue = isInDialogue;    
        }
}