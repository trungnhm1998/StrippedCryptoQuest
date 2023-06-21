using Core.Runtime.Character;
using UnityEngine;

[RequireComponent(typeof(DialogController))]
public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogController _dialogController;

    private string _dialogData;
    public string DialogData => _dialogData;

    public void Interact()
    {
        _dialogData = _dialogController.GetDialogKey();
    }
}