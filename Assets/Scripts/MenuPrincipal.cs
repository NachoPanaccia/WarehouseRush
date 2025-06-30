using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private ScoresPanelController scoresCtrl;

    [Header("Panel de Opciones")] //no implementado
    [SerializeField] private GameObject opcionesPanel;

    public void PlayGame()
    {
        GameManager.Instance.IniciarJuego();
    }

    public void OpenScores()
    {
        scoresCtrl.Open();
    }

    public void OpenOptions()
    {
        if (opcionesPanel != null)
            opcionesPanel.SetActive(true);
    }

    public void QuitGame() => GameManager.Instance.QuitGame();
}