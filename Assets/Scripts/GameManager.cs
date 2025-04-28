using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{
    public static GameManager Instance { get; private set; }

    
    public int score = 0;
    

   
 

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
        
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puntaje actual: " + score);
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
            
            SceneManager.LoadScene(niveles[nivelActualIndex]);
        }
        else
        {
            
            SceneManager.LoadScene("Ganar"); 
        }
    }

}



