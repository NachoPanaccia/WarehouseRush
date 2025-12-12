using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Panel de Opciones")]
    [SerializeField] private GameObject opcionesPanel;


    public void PlayGame() => GameManager.Instance.IniciarJuego();


    public void NewGame() => GameManager.Instance.NuevoJuego();

    public void OpenOptions()
    {
        if (opcionesPanel != null)
            opcionesPanel.SetActive(true);
    }

    public void QuitGame() => GameManager.Instance.QuitGame();
}
