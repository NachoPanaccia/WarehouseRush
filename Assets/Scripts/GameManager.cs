using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Orden de la campaña:
    [SerializeField] private List<string> niveles = new() { "Nivel 0", "Nivel 1", "Nivel 2" };

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
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    // === API ===
    public void IniciarJuego()
    {
        nivelActual = 0;
        CargarNivelActual();
    }

    public void NivelCompletado()
    {
        nivelActual++;
        if (nivelActual < niveles.Count) CargarNivelActual();
        else VolverAlMenu(); // fin de campaña
    }

    public void NivelFallado() => CargarNivelActual();

    public void GoToMainMenu() => VolverAlMenu();
    public void QuitGame() => Application.Quit();

    public float GetTiempoParaEscenaActual()
    {
        var nombre = SceneManager.GetActiveScene().name;
        return tiemposPorNivel.TryGetValue(nombre, out var t) ? t : 0f; // 0 = sin cronómetro
    }

    // === Helpers ===
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
        SceneManager.LoadScene("MenuPrincipal"); // ajustá el nombre si difiere
    }
}
