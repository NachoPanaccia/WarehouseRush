using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Nombres de escenas")]
    private string nombreNivel = "0_Nivel";
    private string nombrePuntajes = "Puntajes";

    [Header("Panel de Opciones (futuro)")]
    [SerializeField] private GameObject opcionesPanel;
    
    public void PlayGame()
    {
        SceneManager.LoadScene(nombreNivel);
    }
    
    public void OpenScores()
    {
        SceneManager.LoadScene(nombrePuntajes);
    }
    
    public void OpenOptions()
    {
        if (opcionesPanel != null)
            opcionesPanel.SetActive(true);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
