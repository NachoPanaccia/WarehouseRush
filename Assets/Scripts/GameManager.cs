using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int score = 0;
    
    [SerializeField] private List<string> niveles = new();
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
        }
        else Destroy(gameObject);
    }
    
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puntaje actual: " + score);
    }
    
    public void EndLevel(bool win, int puntosNivel, float tiempo, string motivo, int estrellas)
    {
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
        SceneManager.LoadScene(niveles[nivelActualIndex]);
    }

    public void CargarSiguienteNivel()
    {
        nivelActualIndex++;
        if (nivelActualIndex < niveles.Count)
            SceneManager.LoadScene(niveles[nivelActualIndex]);
        else
            SceneManager.LoadScene("Ganar");
    }

    public void ReiniciarJuegoDesdeCero()
    {
        score = 0;
        nivelActualIndex = 0;
        SceneManager.LoadScene(niveles[0]);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void QuitGame() => Application.Quit();
}
