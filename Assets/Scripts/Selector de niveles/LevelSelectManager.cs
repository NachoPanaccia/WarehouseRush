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
            Debug.LogError("GameManager.Instance no encontrado en escena.");
            return;
        }

        // 1) Insertar niveles en el árbol
        var niveles = gm.Niveles;
        for (int i = 0; i < niveles.Count; i++)
        {
            bool desbloqueado = gm.EstaDesbloqueado(i);
            arbolNiveles.Insertar(i, niveles[i], desbloqueado);
        }

        // 2) Recorrer en orden y crear botones
        var listaOrdenada = new List<LevelTreeNode>();
        arbolNiveles.RecorrerInOrder(listaOrdenada);

        foreach (var node in listaOrdenada)
        {
            Button boton = Instantiate(levelButtonPrefab, content);
            TMP_Text texto = boton.GetComponentInChildren<TMP_Text>();

            if (texto != null)
                texto.text = node.sceneName;

            // Activar / desactivar botón según si el nivel está desbloqueado
            boton.interactable = node.desbloqueado;

            int indexCapturado = node.index;
            boton.onClick.AddListener(() => OnLevelButtonClicked(indexCapturado));
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnLevelButtonClicked(int index)
    {
        var node = arbolNiveles.Buscar(index);
        if (node == null)
        {
            Debug.LogError($"No se encontró el nodo de nivel {index} en el árbol.");
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
