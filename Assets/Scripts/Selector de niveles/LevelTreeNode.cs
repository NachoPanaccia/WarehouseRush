using UnityEngine;

[System.Serializable]
public class LevelTreeNode
{
    public int index;
    public string sceneName;
    public bool desbloqueado;

    public LevelTreeNode izquierda;
    public LevelTreeNode derecha;

    public LevelTreeNode(int index, string sceneName, bool desbloqueado)
    {
        this.index = index;
        this.sceneName = sceneName;
        this.desbloqueado = desbloqueado;
    }
}
