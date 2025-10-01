using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<string> niveles = new(); // nombres exactos en Build Settings
    private int nivelActual = -1;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    // === API de flujo ===
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

    public void NivelFallado()
    {
        // Reinicia este mismo
        CargarNivelActual();
    }

    public void GoToMainMenu() => VolverAlMenu();
    public void QuitGame() => Application.Quit();

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
