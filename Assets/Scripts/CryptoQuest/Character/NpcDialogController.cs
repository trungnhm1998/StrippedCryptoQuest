using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using CryptoQuest.Characters.Events;
using UnityEngine;

namespace CryptoQuest.Characters
{
    public class NpcDialogController : MonoBehaviour, IDialog
    {
        [SerializeField] private DialogEventScriptableObject _dialogEventSO;
        [SerializeField] private DialogueScriptableObject _dialogueSO;

        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _showDialogEventSO;

        private int _currentIndex = 0;

        private int CalculateNextIndex()
        {
            if (IsDataEmpty()) return 0;
            return (_currentIndex + 1) % _dialogueSO.Lines.Count;
        }

        private bool IsDataEmpty()
        {
            return _dialogueSO.Lines.Count == 0;
        }

        public void ShowDialog()
        {
            _dialogEventSO.OnShow(_dialogueSO.GetLine(CalculateNextIndex()));
            _showDialogEventSO.RaiseEvent();
        }
    }
}