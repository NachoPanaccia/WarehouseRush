using UnityEngine;
using UnityEngine.Events;

public class CreadorDePaquetes : MonoBehaviour
{
    public UnityEvent alHacerClick;

    private void OnMouseDown()
    {
        alHacerClick?.Invoke();
    }
}
