using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Nivel 0"); 
    }

    public void QuitGame()
    {
      
        Application.Quit();
    }
}
