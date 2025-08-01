using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }
    private const int MaxRecords = 10;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    private string PathLevel(int levelIdx)
    {
        string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(levelIdx));

        sceneName = sceneName.Replace(' ', '_');

        return Path.Combine(Application.persistentDataPath, $"TopTimes_{sceneName}.json");
    }

    private string PathGlobal() => Path.Combine(Application.persistentDataPath, "TopScores_Global.json");

    public bool TryInsertLevelTime(int levelIdx, float t, bool menorEsMejor, out int pos)
    {
        var list = Load(PathLevel(levelIdx));
        bool ok = InsertRecord(list, t, menorEsMejor, out pos);
        if (ok) Save(PathLevel(levelIdx), list);
        return ok;
    }

    public bool TryInsertGlobalScore(int puntos, out int pos)
    {
        var list = Load(PathGlobal());
        bool ok = InsertRecord(list, puntos, menor: false, out pos);
        if (ok) Save(PathGlobal(), list);
        return ok;
    }

    public void SetNombreLevel(int levelIdx, int pos, string nombre)
    {
        var list = Load(PathLevel(levelIdx));
        if (ActualizarNombre(list, pos, nombre))
            Save(PathLevel(levelIdx), list);
    }

    public void SetNombreGlobal(int pos, string nombre)
    {
        var list = Load(PathGlobal());
        if (ActualizarNombre(list, pos, nombre))
            Save(PathGlobal(), list);
    }

    public List<Record> GetLevelTimes(int levelIdx) => Load(PathLevel(levelIdx));
    public List<Record> GetGlobalScores() => Load(PathGlobal());

    private List<Record> Load(string path)
    {
        if (!File.Exists(path)) return new List<Record>();
        var wrap = JsonUtility.FromJson<RecordListWrapper>(File.ReadAllText(path));
        return wrap.records ?? new List<Record>();
    }

    private void Save(string path, List<Record> list)
    {
        var wrap = new RecordListWrapper { records = list };
        File.WriteAllText(path, JsonUtility.ToJson(wrap, true));
    }

    private bool InsertRecord(List<Record> list, float val, bool menor, out int pos)
    {
        list.Add(new Record { nombre = "???", valor = val });

        if (menor)
            QuickSorter.QuickSort(list, (a, b) => a.valor.CompareTo(b.valor));
        else
            QuickSorter.QuickSort(list, (a, b) => b.valor.CompareTo(a.valor));

        if (list.Count > MaxRecords) list.RemoveAt(list.Count - 1);

        pos = list.FindIndex(r => r.nombre == "???");
        return pos != -1;
    }

    private bool ActualizarNombre(List<Record> list, int pos, string nombre)
    {
        if (pos < 0 || pos >= list.Count) return false;
        var rec = list[pos];
        rec.nombre = nombre;
        list[pos] = rec;
        return true;
    }
}
