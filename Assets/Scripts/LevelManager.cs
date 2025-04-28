using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private float tiempoDeNivel = 60f;
    private float tiempoRestante;
    private bool nivelCompletado = false;
    [SerializeField] public GameObject pauseMenuUI;
    public GameState currentState = GameState.Playing;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        tiempoRestante = tiempoDeNivel;
    }

    void Update()
    {
        ControlarTiempo();
        ControlarCamiones();
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }
    public void SetGameState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Nuevo estado del juego: " + currentState);
    }

    private void ControlarTiempo()
    {
        if (currentState != GameState.Playing)
            return;

        if (nivelCompletado)
            return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            textoTiempo.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante).ToString() + "s";
        }
        else
        {
            tiempoRestante = 0;
            textoTiempo.text = "Tiempo: 0s";
            SceneManager.LoadScene("Perder");
        }
    }
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            Time.timeScale = 0f;
            SetGameState(GameState.Paused);

            if (LevelManager.Instance.pauseMenuUI != null)
                LevelManager.Instance.pauseMenuUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (currentState == GameState.Paused)
        {
            Time.timeScale = 1f;
            SetGameState(GameState.Playing);

            if (LevelManager.Instance.pauseMenuUI != null)
                LevelManager.Instance.pauseMenuUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetGameState(GameState.Playing);

        if (LevelManager.Instance.pauseMenuUI != null)
            LevelManager.Instance.pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ControlarCamiones()
    {
        if (nivelCompletado)
            return;

        if (CamionManager.Instance != null && CamionManager.Instance.CantidadCamionesActivos() == 0)
        {
            Debug.Log("🏁 No quedan camiones, nivel completado");
            nivelCompletado = true;
            GameManager.Instance.PasarAlSiguienteNivel();
        }
    }

    public void NivelCompleto()
    {
        Debug.Log("Nivel completado, avisando al GameManager...");
        GameManager.Instance.PasarAlSiguienteNivel();
    }
}
public enum GameState
{
    Playing,
    Paused,
    Victory,
    Defeat
}
