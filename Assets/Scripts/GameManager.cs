using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Orden de la campaña (nombres de escenas de niveles, NO el selector)
    [SerializeField] private List<string> niveles = new() { "Nivel 0", "Nivel 1", "Nivel 2" };
    public IReadOnlyList<string> Niveles => niveles;  // para el LevelSelectManager

    // Tiempos por nivel (segundos)
    private readonly Dictionary<string, float> tiemposPorNivel = new()
    {
        { "Nivel 0", 60f },
        { "Nivel 1", 50f },
        { "Nivel 2", 240f }
    };

    private int nivelActual = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Aseguramos que el nivel 0 esté desbloqueado al menos una vez
            if (niveles.Count > 0 && !PlayerPrefs.HasKey("Nivel_0_Desbloqueado"))
            {
                PlayerPrefs.SetInt("Nivel_0_Desbloqueado", 1);
                PlayerPrefs.Save();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ================= API PRINCIPAL =================

    /// <summary>
    /// Mantiene la lógica original: ir a la escena del selector de niveles.
    /// </summary>
    public void IniciarJuego()
    {
        SceneManager.LoadScene("SelectorNiveles");
    }

    /// <summary>
    /// Nuevo juego: borra el progreso (bloquea todos los niveles menos el 0)
    /// y luego va al SelectorNiveles.
    /// </summary>
    public void NuevoJuego()
    {
        // Borrar estado de desbloqueo de todos los niveles
        for (int i = 0; i < niveles.Count; i++)
        {
            PlayerPrefs.DeleteKey($"Nivel_{i}_Desbloqueado");
        }

        // Dejar el nivel 0 desbloqueado
        if (niveles.Count > 0)
        {
            PlayerPrefs.SetInt("Nivel_0_Desbloqueado", 1);
        }

        PlayerPrefs.Save();

        // Sin nivel activo todavía
        nivelActual = -1;

        // Mantener el flujo: siempre entrar por el Selector de niveles
        SceneManager.LoadScene("SelectorNiveles");
    }

    /// <summary>
    /// Llamás esto cuando el jugador termina un nivel con éxito.
    /// Desbloquea el siguiente nivel y lo carga.
    /// </summary>
    public void NivelCompletado()
    {
        // Desbloquear el siguiente nivel si existe
        if (nivelActual + 1 < niveles.Count)
        {
            PlayerPrefs.SetInt($"Nivel_{nivelActual + 1}_Desbloqueado", 1);
            PlayerPrefs.Save();
        }

        nivelActual++;
        if (nivelActual < niveles.Count)
            SceneManager.LoadScene("SelectorNiveles");
        else
            VolverAlMenu(); // fin de campaña
    }

    public void NivelFallado() => CargarNivelActual();

    public void GoToMainMenu() => VolverAlMenu();

    public void QuitGame() => Application.Quit();

    /// <summary>
    /// Tiempo configurado para la escena actual (lo usa el LevelManager).
    /// </summary>
    public float GetTiempoParaEscenaActual()
    {
        var nombre = SceneManager.GetActiveScene().name;
        return tiemposPorNivel.TryGetValue(nombre, out var t) ? t : 0f; // 0 = sin cronómetro
    }

    /// <summary>
    /// Lo usa el LevelSelectManager: carga un nivel por índice de la lista 'niveles'.
    /// </summary>
    public void CargarNivelPorIndice(int index)
    {
        if (index < 0 || index >= niveles.Count)
        {
            Debug.LogWarning($"Índice de nivel inválido: {index}");
            return;
        }

        nivelActual = index;
        CargarNivelActual();
    }

    /// <summary>
    /// Indica si un nivel está desbloqueado (por índice).
    /// Lo usa el LevelSelectManager para habilitar / deshabilitar botones.
    /// </summary>
    public bool EstaDesbloqueado(int index)
    {
        if (index < 0 || index >= niveles.Count)
            return false;

        // El nivel 0 siempre lo consideramos desbloqueado
        if (index == 0)
            return true;

        return PlayerPrefs.GetInt($"Nivel_{index}_Desbloqueado", 0) == 1;
    }

    // ================= Helpers internos =================

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
        SceneManager.LoadScene("MenuPrincipal"); // cambiá el nombre si tu menú se llama distinto
    }
}
