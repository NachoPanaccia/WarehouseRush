using System.Collections.Generic;
using UnityEngine;

public class LevelBinarySearchTree
{
    private LevelTreeNode raiz;

    public void Insertar(int index, string sceneName, bool desbloqueado)
    {
        raiz = InsertarRec(raiz, index, sceneName, desbloqueado);
    }

    private LevelTreeNode InsertarRec(LevelTreeNode actual, int index, string sceneName, bool desbloqueado)
    {
        if (actual == null)
            return new LevelTreeNode(index, sceneName, desbloqueado);

        if (index < actual.index)
            actual.izquierda = InsertarRec(actual.izquierda, index, sceneName, desbloqueado);
        else
            actual.derecha = InsertarRec(actual.derecha, index, sceneName, desbloqueado);

        return actual;
    }

    public LevelTreeNode Buscar(int index)
    {
        return BuscarRec(raiz, index);
    }

    private LevelTreeNode BuscarRec(LevelTreeNode actual, int index)
    {
        if (actual == null) return null;

        if (index == actual.index) return actual;
        if (index < actual.index) return BuscarRec(actual.izquierda, index);
        return BuscarRec(actual.derecha, index);
    }

    public void ObtenerEnOrden(List<LevelTreeNode> lista)
    {
        lista.Clear();
        InOrderRec(raiz, lista);
    }

    private void InOrderRec(LevelTreeNode actual, List<LevelTreeNode> lista)
    {
        if (actual == null) return;

        InOrderRec(actual.izquierda, lista);
        lista.Add(actual);
        InOrderRec(actual.derecha, lista);
    }
}
