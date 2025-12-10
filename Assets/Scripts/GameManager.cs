using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Orden de la campaña (nombres de escenas)")]
    [SerializeField] private List<string> niveles = new() { "Nivel 0", "Nivel 1", "Nivel 2" };

    [Header("Tiempos por nivel (segundos)")]
    private readonly Dictionary<string, float> tiemposPorNivel = new()
    {
        { "Nivel 0", 60f },
        { "Nivel 1", 50f },
        { "Nivel 2", 40f }
    };

    private int nivelActual = 0;

    // Progreso
    private const string PREF_MAX_LEVEL = "MAX_LEVEL_UNLOCKED";
    private int nivelMaxDesbloqueado = 0;

    public IReadOnlyList<string> Niveles => niveles;
    public int NivelMaxDesbloqueado => nivelMaxDesbloqueado;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CargarProgreso();
    }

    private void CargarProgreso()
    {
        nivelMaxDesbloqueado = PlayerPrefs.GetInt(PREF_MAX_LEVEL, 0);
        if (niveles.Count > 0)
            nivelMaxDesbloqueado = Mathf.Clamp(nivelMaxDesbloqueado, 0, niveles.Count - 1);
        else
            nivelMaxDesbloqueado = 0;
    }

    private void GuardarProgreso()
    {
        PlayerPrefs.SetInt(PREF_MAX_LEVEL, nivelMaxDesbloqueado);
        PlayerPrefs.Save();
    }

    // ==== API para el selector de niveles ====

    public bool EstaDesbloqueado(int index)
    {
        return index >= 0 && index <= nivelMaxDesbloqueado && index < niveles.Count;
    }

    public void CargarNivelPorIndice(int index)
    {
        if (!EstaDesbloqueado(index))
        {
            Debug.LogWarning($"Intento de cargar nivel bloqueado: {index}");
            return;
        }

        if (index < 0 || index >= niveles.Count)
        {
            Debug.LogError($"Índice de nivel fuera de rango: {index}");
            return;
        }

        nivelActual = index;
        Time.timeScale = 1f;
        SceneManager.LoadScene(niveles[nivelActual]);
    }

    // ==== Interacción con el menú principal ====

    // Llamado por MenuPrincipal.PlayGame()
    public void IniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectorNiveles"); // nueva escena
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    // Compatibilidad con el snippet que tenías
    private void CargarNivelActual()
    {
        if (nivelActual >= 0 && nivelActual < niveles.Count)
            SceneManager.LoadScene(niveles[nivelActual]);
        else
            VolverAlMenu();
    }

    private void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    // ==== API usada por LevelManager ====

    public float GetTiempoParaEscenaActual()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        if (tiemposPorNivel.TryGetValue(escenaActual, out float t))
            return t;

        // Por si alguna escena no está en el diccionario
        return 60f;
    }

    public void NivelCompletado()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        int indiceActual = niveles.IndexOf(escenaActual);

        if (indiceActual != -1)
        {
            // Desbloqueo del siguiente nivel
            int siguiente = indiceActual + 1;
            if (siguiente < niveles.Count && siguiente > nivelMaxDesbloqueado)
            {
                nivelMaxDesbloqueado = siguiente;
                GuardarProgreso();
            }
        }

        // Después de ganar, volvemos al selector para que se vea el nuevo nivel desbloqueado
        SceneManager.LoadScene("SelectorNiveles");
    }

    public void NivelFallado()
    {
        // Podés hacer retry, ir al menú, etc. Por ahora lo mando al selector.
        SceneManager.LoadScene("SelectorNiveles");
    }
}
