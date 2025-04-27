using UnityEngine;
using UnityEngine.SceneManagement;

public class PerderGanar : MonoBehaviour
{
    public void Volver()
    {
        SceneManager.LoadScene("MenuPrincipal"); 
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;                  
    }

}
