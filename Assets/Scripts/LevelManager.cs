using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("UI (opcional)")]
    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private GameObject pauseMenuUI;

    private float tiempoDeNivel;      // viene de GameManager
    private float tiempoRestante;     // contador interno
    private bool nivelCompletado = false;
    private bool huboCamionesEnAlgúnMomento = false;

    public GameState currentState = GameState.Playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        // Pedimos el tiempo al GameManager (si no hay, usamos 0 = sin límite)
        if (GameManager.Instance != null)
        {
            tiempoDeNivel = GameManager.Instance.GetTiempoParaEscenaActual();
        }
        else
        {
            tiempoDeNivel = 0f;
            Debug.LogWarning("[LevelManager] No se encontró GameManager. El nivel no tendrá límite de tiempo.");
        }

        tiempoRestante = tiempoDeNivel;
        huboCamionesEnAlgúnMomento = false;
        nivelCompletado = false;
        currentState = GameState.Playing;

        ActualizarUI();

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Pausa con P
        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();

        // Si no estamos jugando o ya terminó el nivel, no seguimos actualizando
        if (currentState != GameState.Playing || nivelCompletado)
            return;

        // === TIEMPO DEL NIVEL ===
        if (tiempoDeNivel > 0f)
        {
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                ActualizarUI();
                nivelCompletado = true;

                if (GameManager.Instance != null)
                    GameManager.Instance.NivelFallado();

                return;
            }

            ActualizarUI();
        }

        // === CHEQUEO DE CAMIONES ===
        if (CamionManager.Instance != null)
        {
            int cant = CamionManager.Instance.CantidadCamionesActivos();
            Debug.Log($"[LevelManager] Cantidad de camiones activos al Update: {cant}");

            // Si alguna vez hubo más de 0 camiones, lo marcamos
            if (cant > 0)
                huboCamionesEnAlgúnMomento = true;

            // Solo consideramos "nivel completado" cuando:
            // 1) Alguna vez hubo camiones
            // 2) Ahora ya no queda ninguno
            if (huboCamionesEnAlgúnMomento && cant == 0)
            {
                nivelCompletado = true;

                if (GameManager.Instance != null)
                    GameManager.Instance.NivelCompletado();
            }
        }
    }

    // === UI TIEMPO ===
    private void ActualizarUI()
    {
        if (textoTiempo == null)
            return;

        if (tiempoDeNivel <= 0f)
        {
            // Sin límite de tiempo -> podés dejar vacío o poner "∞"
            textoTiempo.text = "";
            return;
        }

        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);
        textoTiempo.text = $"{minutos:00}:{segundos:00}";
    }

    // === PAUSA ===
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            Time.timeScale = 0f;
            currentState = GameState.Paused;

            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(true);

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

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // === MENÚ PRINCIPAL ===
    public void VolverAlMenuPrincipal()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GoToMainMenu();
        else
            Debug.LogWarning("[LevelManager] No hay GameManager para volver al menú.");
    }
}

public enum GameState { Playing, Paused, Victory, Defeat }
