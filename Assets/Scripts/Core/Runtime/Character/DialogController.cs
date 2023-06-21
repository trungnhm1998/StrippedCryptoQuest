using System;
using Core.Runtime.Character;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using UnityEngine;

public class DialogController : MonoBehaviour, IDialog
{
    [SerializeField] private DialogsScriptableObject _dialogSO;
    private int _currentIndex = 0;

    public void SetDialog(DialogsScriptableObject dialogSO)
    {
        _dialogSO = dialogSO;
    }


    public string GetDialogKey()
    {
        var isDataEmpty = _dialogSO.Messages.Count == 0;
        if (isDataEmpty) return String.Empty;

        var message = _dialogSO.Messages[GetNextIndex()];

        return message;
    }

    public int GetNextIndex()
    {
        _currentIndex++;

        if (_currentIndex >= _dialogSO.Messages.Count)
        {
            _currentIndex = 0;
        }

        return _currentIndex;
    }
}