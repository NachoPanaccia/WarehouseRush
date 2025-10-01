using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Tiempo (opcional)")]
    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private float tiempoDeNivel = 0f; // 0 o negativo = desactivado
    private float tiempoRestante;

    [Header("Pausa")]
    [SerializeField] public GameObject pauseMenuUI;

    private bool nivelCompletado = false;
    public GameState currentState = GameState.Playing;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        tiempoRestante = tiempoDeNivel;
        ActualizarUI();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) TogglePause();

        if (currentState != GameState.Playing || nivelCompletado) return;

        ControlarTiempo();
        ControlarCamiones();
    }

    private void ControlarTiempo()
    {
        if (tiempoDeNivel <= 0f) return; // cronómetro desactivado

        if (tiempoRestante > 0f)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante < 0f) tiempoRestante = 0f;
            ActualizarUI();
        }
        else
        {
            // Se acabó el tiempo
            nivelCompletado = true;
            GameManager.Instance.NivelFallado();
        }
    }

    private void ControlarCamiones()
    {
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
        else if (currentState == GameState.Paused)
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
