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
        if (currentState != GameState.Playing || nivelCompletado) return;

        if (tiempoRestante > 0f)
        {
            tiempoRestante -= Time.deltaTime;
            textoTiempo.text = "Tiempo: " + FormatearTiempo(tiempoRestante);
        }
        else
        {
            tiempoRestante = 0f;
            textoTiempo.text = "Tiempo: 00:00:00:00";

            GameManager.Instance.EndLevel(
                win: false,
                puntosNivel: 0,
                tiempo: tiempoDeNivel,
                motivo: "Tiempo agotado",
                estrellas: 0
            );
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

        int puntosNivel = CalcularYSumarPuntaje(out int estrellas, out float tiempoUsado);

        GameManager.Instance.EndLevel(
            win: true,
            puntosNivel: puntosNivel,
            tiempo: tiempoUsado,
            motivo: string.Empty,
            estrellas: estrellas
        );
    }

    private static string FormatearTiempo(float segundos)
    {
        var t = System.TimeSpan.FromSeconds(segundos);
        
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
                             t.Hours,
                             t.Minutes,
                             t.Seconds,
                             t.Milliseconds / 10);
    }

    private int CalcularYSumarPuntaje(out int estrellas, out float tiempoUsado)
    {
        tiempoUsado = tiempoDeNivel - tiempoRestante;
        float pr = tiempoUsado / tiempoDeNivel;

        int puntos;
        if (pr <= 0.25f) // ≤ 25 %
        {
            puntos = 200; estrellas = 3;
        }
        else if (pr <= 0.5f) // >25 % y ≤50 %
        {
            puntos = 100; estrellas = 2;
        }
        else // >50 % (hasta 100 %)
        {
            puntos = 50; estrellas = 1;
        }
        
        Debug.Log($"Nivel completado en {FormatearTiempo(tiempoUsado)} → +{puntos} pts");
        return puntos;
    }
}

public enum GameState
{
    Playing,
    Paused,
    Victory,
    Defeat
}