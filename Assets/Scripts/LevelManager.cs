using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Tiempo")]
    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private float tiempoDeNivel;
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
    }

    private void Update()
    {
        ControlarTiempo();
        ControlarCamiones();

        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }
    
    private void ControlarTiempo()
    {
        if (currentState != GameState.Playing || nivelCompletado)
            return;

        if (tiempoRestante > 0f)
        {
            tiempoRestante -= Time.deltaTime;
            textoTiempo.text = $"Tiempo: {Mathf.CeilToInt(tiempoRestante)}s";
        }
        else
        {
            tiempoRestante = 0f;
            textoTiempo.text = "Tiempo: 0s";
            SceneManager.LoadScene("Perder");
        }
    }
    
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            Time.timeScale = 0f;
            currentState = GameState.Paused;

            if (pauseMenuUI != null) pauseMenuUI.SetActive(true);

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

        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void ControlarCamiones()
    {
        if (nivelCompletado) return;

        if (CamionManager.Instance != null &&
            CamionManager.Instance.CantidadCamionesActivos() == 0)
        {
            CompletarNivel();
        }
    }

    
    public void NivelCompleto() => CompletarNivel();
    
    private void CompletarNivel()
    {
        if (nivelCompletado) return;
        nivelCompletado = true;

        OtorgarPuntaje();
        GameManager.Instance.PasarAlSiguienteNivel();
    }

    private void OtorgarPuntaje()
    {
        float tiempoUsado = tiempoDeNivel - tiempoRestante;
        float proporcionUsada = tiempoUsado / tiempoDeNivel;

        int puntos = 0;
        if (proporcionUsada <= 1f / 3f) puntos = 200;
        else if (proporcionUsada <= 1f / 2f) puntos = 100;
        else puntos = 50;

        GameManager.Instance.AddScore(puntos);
        Debug.Log($"Nivel completado en {tiempoUsado:0.0}s  →  +{puntos} pts");
    }
}

public enum GameState
{
    Playing,
    Paused,
    Victory,
    Defeat
}