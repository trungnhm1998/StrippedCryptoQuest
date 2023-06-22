using System;
using Core.Runtime.Character;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using UnityEngine;

public class DialogController : MonoBehaviour, IDialog
{
    [SerializeField] private DialogsScriptableObject _dialogSO;
    private int _currentIndex = 0;

    public void SetDialogData(DialogsScriptableObject dialogSO)
    {
        _dialogSO = dialogSO;
    }

    public string GetDialogKey()
    {
        if (IsDataEmpty()) return String.Empty;

        var message = _dialogSO.Messages[GetNextIndex()];
        return message;
    }

    public int GetNextIndex()
    {
        _currentIndex = CalculateNextIndex();
        return _currentIndex;
    }

    private int CalculateNextIndex()
    {
        if (IsDataEmpty()) return 0;
        return (_currentIndex + 1) % _dialogSO.Messages.Count;
    }

    private bool IsDataEmpty()
    {
        return _dialogSO.Messages.Count == 0;
    }

    public int GetCurrentDialogIndex()
    {
        return _currentIndex;
    }
}