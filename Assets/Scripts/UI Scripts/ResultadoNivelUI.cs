using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultadoNivelUI : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelDerrota;
    
    [Header("Textos Victoria")]
    [SerializeField] private TMP_Text vicTitulo;
    [SerializeField] private TMP_Text vicPuntosNivel;
    [SerializeField] private TMP_Text vicPuntajeTotal;
    [SerializeField] private TMP_Text vicTiempo;
    [SerializeField] private Image[] estrellas;
    
    [Header("Textos Derrota")]
    [SerializeField] private TMP_Text derTitulo;
    [SerializeField] private TMP_Text derPuntajeTotal;
    [SerializeField] private TMP_Text derMotivo;
    
    [Header("Botones")]
    [SerializeField] private Button btnReintentar;
    [SerializeField] private Button btnSiguienteNivel;
    [SerializeField] private Button btnReiniciarJuego;
    [SerializeField] private Button btnMenuPrincipal;

    private void Start()
    {
        var res = GameManager.Instance.ultimoResultado;

        if (res.win)
        {
            panelVictoria.SetActive(true);
            panelDerrota.SetActive(false);

            vicTitulo.text = "¡Ganaste!";
            vicPuntosNivel.text = $"+{res.puntosNivel} pts";
            vicPuntajeTotal.text = $"Puntaje total: {res.puntajeTotal}";
            vicTiempo.text = $"Tiempo: {FormatearTiempo(res.tiempo)}";

            for (int i = 0; i < estrellas.Length; i++)
                estrellas[i].enabled = i < res.estrellas;

            btnSiguienteNivel.gameObject.SetActive(true);
            btnReiniciarJuego.gameObject.SetActive(false);

            if (res.nuevoRecord && NombreRecordPopup.Instance != null)
            {
                NombreRecordPopup.Instance.SolicitarNombre(
                    n => HighScoreManager.Instance
                            .SetNombreLevel(res.recordLevelIdx, res.recordPos, n));
            }
        }
        else
        {
            panelVictoria.SetActive(false);
            panelDerrota.SetActive(true);

            derTitulo.text = "Perdiste";
            derPuntajeTotal.text = $"Puntaje total: {res.puntajeTotal}";
            derMotivo.text = $"Motivo: {res.motivoDerrota}";

            btnSiguienteNivel.gameObject.SetActive(false);
            btnReiniciarJuego.gameObject.SetActive(true);
        }

        btnReintentar.onClick.AddListener(GameManager.Instance.ReintentarNivel);
        btnMenuPrincipal.onClick.AddListener(GameManager.Instance.GoToMainMenu);
        if (btnSiguienteNivel.gameObject.activeSelf)
            btnSiguienteNivel.onClick.AddListener(GameManager.Instance.CargarSiguienteNivel);
        if (btnReiniciarJuego.gameObject.activeSelf)
            btnReiniciarJuego.onClick.AddListener(GameManager.Instance.ReiniciarJuegoDesdeCero);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private static string FormatearTiempo(float segundos)
    {
        var t = System.TimeSpan.FromSeconds(segundos);
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
                             t.Hours, t.Minutes, t.Seconds, t.Milliseconds / 10);
    }
}
