using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NombreRecordPopup : MonoBehaviour
{
    public static NombreRecordPopup Instance { get; private set; }

    [SerializeField] private GameObject root;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button btnOk;
    private Action<string> onConfirm;

    private void Awake()
    {
        Instance = this;
        root.SetActive(false);
        btnOk.onClick.AddListener(() =>
        {
            string n = string.IsNullOrWhiteSpace(input.text) ? "Anon" : input.text;
            onConfirm?.Invoke(n);
            root.SetActive(false);
        });
    }

    public void SolicitarNombre(Action<string> callback)
    {
        onConfirm = callback;
        input.text = "";
        root.SetActive(true);
        input.ActivateInputField();
    }
}
