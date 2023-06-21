using System;
using Core.Runtime.Character;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using UnityEngine;

[RequireComponent(typeof(NPC))]
public class DialogController : MonoBehaviour, IDialog
{
    [SerializeField] private DialogsScriptableObject _dialogSO;
    private int _currentIndex = 0;

    public void SetDialog(DialogsScriptableObject dialogSO)
    {
        _dialogSO = dialogSO;
    }

    public string GetDialog()
    {
        if (_dialogSO.messages.Count == 0) return String.Empty;

        var currentMessage = _dialogSO.messages[_currentIndex];

        _currentIndex++;

        if (_currentIndex > _dialogSO.messages.Count)
        {
            _currentIndex = 0;
        }

        return currentMessage;
    }
}