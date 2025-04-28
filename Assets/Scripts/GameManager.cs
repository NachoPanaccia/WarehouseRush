using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{
    public static GameManager Instance { get; private set; }

    
    public int score = 0;
    public GameState currentState = GameState.Playing;

   
    [SerializeField] private GameObject pauseMenuUI;

    [SerializeField] private List<string> niveles;
    private int nivelActualIndex = -1;

    private void Awake()
    {
      
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puntaje actual: " + score);
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Nuevo estado del juego: " + currentState);
    }

    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            Time.timeScale = 0f;
            SetGameState(GameState.Paused);

            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
        else if (currentState == GameState.Paused)
        {
            Time.timeScale = 1f;
            SetGameState(GameState.Playing);

            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetGameState(GameState.Playing);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuPrincipal"); 
    }
    public void QuitGame()
    {
             Application.Quit(); 
    }
    
    public void PasarAlSiguienteNivel()
    {
        nivelActualIndex++;

        if (nivelActualIndex < niveles.Count)
        {
            Debug.Log("Cargando siguiente nivel: " + niveles[nivelActualIndex]);
            SceneManager.LoadScene(niveles[nivelActualIndex]);
        }
        else
        {
            Debug.Log("¡No hay más niveles! Cargando pantalla de victoria...");
            SceneManager.LoadScene("Ganar"); 
        }
    }

}

public enum GameState
{
    Playing,
    Paused,
    Victory,
    Defeat
}

