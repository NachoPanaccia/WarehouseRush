using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform content;          // panel con VerticalLayoutGroup
    [SerializeField] private Button levelButtonPrefab;   // prefab de botón con TMP_Text

    private LevelBinarySearchTree arbolNiveles = new();

    private void Start()
    {
        var gm = GameManager.Instance;
        if (gm == null)
        {
            Debug.LogError("No hay GameManager en la escena.");
            return;
        }

        // 1) Construimos el árbol con todos los niveles
        var niveles = gm.Niveles;
        for (int i = 0; i < niveles.Count; i++)
        {
            bool desbloqueado = gm.EstaDesbloqueado(i);
            arbolNiveles.Insertar(i, niveles[i], desbloqueado);
        }

        // 2) Obtenemos los nodos ordenados por índice (in-order)
        List<LevelTreeNode> orden = new();
        arbolNiveles.ObtenerEnOrden(orden);

        // 3) Creamos un botón por cada nodo
        foreach (var node in orden)
        {
            Button btn = Instantiate(levelButtonPrefab, content);
            TMP_Text texto = btn.GetComponentInChildren<TMP_Text>();

            if (texto != null)
                texto.text = $"{node.sceneName} {(node.desbloqueado ? "" : "(Bloqueado)")}".Trim();

            btn.interactable = node.desbloqueado;

            int indiceCaptura = node.index;  // evitar el closure raro

            btn.onClick.AddListener(() => OnLevelButtonClicked(indiceCaptura));
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnLevelButtonClicked(int index)
    {
        // Acá usamos el ÁRBOL para buscar el nivel (lo importante para la consigna)
        LevelTreeNode node = arbolNiveles.Buscar(index);

        if (node == null)
        {
            Debug.LogError($"Nivel {index} no encontrado en el árbol.");
            return;
        }

        if (!node.desbloqueado)
        {
            Debug.Log($"Nivel {index} está bloqueado.");
            return;
        }

        GameManager.Instance.CargarNivelPorIndice(index);
    }

    public void VolverAlMenuPrincipal()
    {
        GameManager.Instance.GoToMainMenu();
    }
}
