using System;
using Core.Runtime.Character;
using UnityEngine;

[RequireComponent(typeof(DialogController))]
public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogController _dialogController;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _dialogController = GetComponent<DialogController>();
    }

    public string Interact()
    {
        return _dialogController.GetDialog();
    }
}