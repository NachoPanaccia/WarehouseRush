using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private float tiempoDeNivel = 60f;
    private float tiempoRestante;
    private bool nivelCompletado = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        tiempoRestante = tiempoDeNivel;
    }

    void Update()
    {
        ControlarTiempo();
        ControlarCamiones();
    }

    private void ControlarTiempo()
    {
        if (GameManager.Instance.currentState != GameState.Playing)
            return;

        if (nivelCompletado)
            return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            textoTiempo.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante).ToString() + "s";
        }
        else
        {
            tiempoRestante = 0;
            textoTiempo.text = "Tiempo: 0s";
            SceneManager.LoadScene("Perder");
        }
    }

    private void ControlarCamiones()
    {
        if (nivelCompletado)
            return;

        if (CamionManager.Instance != null && CamionManager.Instance.CantidadCamionesActivos() == 0)
        {
            Debug.Log("🏁 No quedan camiones, nivel completado");
            nivelCompletado = true;
            GameManager.Instance.PasarAlSiguienteNivel();
        }
    }

    public void NivelCompleto()
    {
        Debug.Log("Nivel completado, avisando al GameManager...");
        GameManager.Instance.PasarAlSiguienteNivel();
    }
}
