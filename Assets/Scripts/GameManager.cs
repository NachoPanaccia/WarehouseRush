using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    private List<int> puntosPorNivel = new();

    [SerializeField] private List<string> niveles = new();   // mismos nombres que en Build Settings
    private List<int> buildIndices = new();
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
        public bool nuevoRecord;
        public int recordLevelIdx;
        public int recordPos;
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

        buildIndices.Clear();
        foreach (var n in niveles)
            buildIndices.Add(SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{n}.unity"));
    }

    public int BuildIndexDeOpcion(int opcion) => buildIndices[opcion];

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

    public void EndLevel(bool win, int puntosNivel, float tiempo, string motivo, int estrellas, bool nuevoRecord = false, int recordLevelIdx = -1, int recordPos = -1)
    {
        if (win)
        {
            int previo = puntosPorNivel[nivelActualIndex];
            int delta = puntosNivel - previo;

            AddScore(delta);
            puntosPorNivel[nivelActualIndex] = puntosNivel;
            Debug.Log(" cargando next level ");
        }

        ultimoResultado = new ResultadoNivel
        {
            win = win,
            puntosNivel = puntosNivel,
            puntajeTotal = score,
            tiempo = tiempo,
            motivoDerrota = motivo,
            estrellas = estrellas,
            nuevoRecord = nuevoRecord,
            recordLevelIdx = recordLevelIdx,
            recordPos = recordPos
        };
        Debug.Log($"EndLevel ⇒ win:{win}, tiempo:{tiempo} – cargando ResultadoNivel");
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
