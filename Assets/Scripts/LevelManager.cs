using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("UI (opcional)")]
    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private GameObject pauseMenuUI;

    private float tiempoDeNivel;     // viene de GameManager
    private float tiempoRestante;    // contador interno
    private bool nivelCompletado = false;

    public GameState currentState = GameState.Playing;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        tiempoDeNivel = GameManager.Instance ? GameManager.Instance.GetTiempoParaEscenaActual() : 0f;
        tiempoRestante = tiempoDeNivel;
        ActualizarUI();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) TogglePause();
        if (currentState != GameState.Playing || nivelCompletado) return;

        // Tiempo (si está activo)
        if (tiempoDeNivel > 0f)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                ActualizarUI();
                nivelCompletado = true;
                GameManager.Instance.NivelFallado();
                return;
            }
            ActualizarUI();
        }

        // Chequeo de camiones
        if (CamionManager.Instance != null &&
            CamionManager.Instance.CantidadCamionesActivos() == 0)
        {
            nivelCompletado = true;
            GameManager.Instance.NivelCompletado();
        }
    }

    private void ActualizarUI()
    {
        if (!textoTiempo) return;
        if (tiempoDeNivel <= 0f) { textoTiempo.text = ""; return; }

        var t = System.TimeSpan.FromSeconds(tiempoRestante);
        textoTiempo.text = $"Tiempo: {t.Minutes:00}:{t.Seconds:00}:{t.Milliseconds / 10:00}";
    }

    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            Time.timeScale = 0f;
            currentState = GameState.Paused;
            if (pauseMenuUI) pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void VolverAlMenuPrincipal() => GameManager.Instance.GoToMainMenu();
}

public enum GameState { Playing, Paused, Victory, Defeat }
