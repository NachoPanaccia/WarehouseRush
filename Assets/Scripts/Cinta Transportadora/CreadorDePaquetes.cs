using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Collider), typeof(Renderer))]

public class CreadorDePaquetes : MonoBehaviour
{
    [Header("Materiales")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material cooldownMaterial;

    [Header("Evento a disparar")]
    public UnityEvent alHacerClick;
    private float cooldown = 2.5f;

    private Renderer _renderer;
    private bool _bloqueado;
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (normalMaterial != null)
            _renderer.material = normalMaterial;
    }

    private void OnMouseDown()
    {
        if (_bloqueado) return;

        alHacerClick?.Invoke();
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        _bloqueado = true;
        if (cooldownMaterial != null)
            _renderer.material = cooldownMaterial;

        yield return new WaitForSeconds(cooldown);

        if (normalMaterial != null)
            _renderer.material = normalMaterial;
        _bloqueado = false;
    }
}
