using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    private List<int> puntosPorNivel = new();

    [SerializeField] private List<string> niveles = new();
    public IReadOnlyList<string> Niveles => niveles;
    private int nivelActualIndex = -1;

    public struct ResultadoNivel
    {
        public bool win;
        public int puntosNivel;
        public int puntajeTotal;
        public float tiempo;
        public int estrellas;
        public string motivoDerrota;
    }
    public ResultadoNivel ultimoResultado { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            puntosPorNivel = new List<int>(niveles.Count);
            for (int i = 0; i < niveles.Count; i++) puntosPorNivel.Add(0);
        }
        else Destroy(gameObject);
    }

    public void IniciarJuego()
    {
        score = 0;
        for (int i = 0; i < puntosPorNivel.Count; i++) puntosPorNivel[i] = 0;

        nivelActualIndex = 0;
        SceneManager.LoadScene(niveles[0]);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puntaje actual: " + score);
    }

    public void EndLevel(bool win, int puntosNivel, float tiempo, string motivo, int estrellas)
    {
        if (win)
        {
            int previo = puntosPorNivel[nivelActualIndex];
            int delta = puntosNivel - previo;

            AddScore(delta);
            puntosPorNivel[nivelActualIndex] = puntosNivel;
        }

        ultimoResultado = new ResultadoNivel

        {
            win = win,
            puntosNivel = puntosNivel,
            puntajeTotal = score,
            tiempo = tiempo,
            motivoDerrota = motivo,
            estrellas = estrellas
        };

        SceneManager.LoadScene("ResultadoNivel");
    }

    public void ReintentarNivel()
    {
        if (nivelActualIndex < 0) nivelActualIndex = 0;
        SceneManager.LoadScene(niveles[nivelActualIndex]);
    }

    public void CargarSiguienteNivel()
    {
        nivelActualIndex++;
        if (nivelActualIndex < niveles.Count)
        {
            SceneManager.LoadScene(niveles[nivelActualIndex]);
        }
        else
        {
            bool topScore = HighScoreManager.Instance.TryInsertGlobalScore(score, out int pos);

            if (topScore)
            {
                NombreRecordPopup.Instance.SolicitarNombre((nombre) => { HighScoreManager.Instance.SetNombreGlobal(pos, nombre); });
            }
            SceneManager.LoadScene("Ganar");
        }
    }

    public void ReiniciarJuegoDesdeCero() => IniciarJuego();

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void QuitGame() => Application.Quit();
}
