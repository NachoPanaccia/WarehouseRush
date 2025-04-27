using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private float tiempoDeNivel = 6f;
    private float tiempoRestante;

    void Start()
    {
        tiempoRestante = tiempoDeNivel;
    }

    void Update()
    {
        ControlarTiempo();
    }

    private void ControlarTiempo()
    {
        if (GameManager.Instance.currentState != GameState.Playing)
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

}
