using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoresPanelController : MonoBehaviour
{
    [Header("Panel raíz")]
    [SerializeField] private GameObject panelScores;

    [Header("Secciones")]
    [SerializeField] private GameObject panelBestScores;
    [SerializeField] private GameObject panelBestTimes;

    [Header("Contenedores Scroll")]
    [SerializeField] private Transform contentScores;
    [SerializeField] private Transform contentTimes;

    [Header("Otros UI")]
    [SerializeField] private TMP_Dropdown dropdownLevels;
    [SerializeField] private GameObject filaPrefab;
    [SerializeField] private Button btnBestScores;
    [SerializeField] private Button btnBestTimes;

    void Awake()
    {
        panelScores.SetActive(false);

        btnBestScores.onClick.AddListener(ShowBestScores);
        btnBestTimes.onClick.AddListener(ShowBestTimes);

        dropdownLevels.onValueChanged.AddListener(ShowTimesForLevel);
    }

    #region Apertura / cierre

    public void Open()
    {
        panelScores.SetActive(true);

        dropdownLevels.ClearOptions();
        dropdownLevels.AddOptions(new List<string>(GameManager.Instance.Niveles));
        dropdownLevels.value = 0;

        ShowBestScores();
    }

    public void Close()
    {
        panelScores.SetActive(false);
    }

    #endregion

    #region Pestaña “Mejores Puntajes”

    private void ShowBestScores()
    {
        panelBestScores.SetActive(true);
        panelBestTimes.SetActive(false);

        PopulateList(
            HighScoreManager.Instance.GetGlobalScores(),
            contentScores,
            isTime: false);
    }

    #endregion

    #region Pestaña “Mejores Tiempos”

    private void ShowBestTimes()
    {
        panelBestScores.SetActive(false);
        panelBestTimes.SetActive(true);

        ShowTimesForLevel(dropdownLevels.value);
    }

    private void ShowTimesForLevel(int optionIdx)
    {
        int buildIdx = optionIdx + 1;                     // saltar la escena 0
        PopulateList(
            HighScoreManager.Instance.GetLevelTimes(buildIdx),
            contentTimes,
            isTime: true);
    }


    #endregion

    #region Helpers genéricos

    private void PopulateList(List<Record> list, Transform container, bool isTime)
    {
        foreach (Transform t in container) Destroy(t.gameObject);

        if (list == null || list.Count == 0)
        {
            CreateRow(container, "-", "—", "Sin registros");
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            var rec = list[i];
            string val = isTime ? FormatTime(rec.valor) : rec.valor.ToString("N0");
            CreateRow(container, (i + 1).ToString(), rec.nombre, val);
        }
    }

    private void CreateRow(Transform parent, string colA, string colB, string colC)
    {
        var row = Instantiate(filaPrefab, parent);
        var texts = row.GetComponentsInChildren<TMP_Text>();
        texts[0].text = colA;
        texts[1].text = colB;
        texts[2].text = colC;
    }

    private static string FormatTime(float seconds)
    {
        var ts = System.TimeSpan.FromSeconds(seconds);
        return $"{ts.Minutes:00}:{ts.Seconds:00}:{ts.Milliseconds / 10:00}";
    }

    #endregion
}
