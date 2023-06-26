using Core.Runtime.Events.ScriptableObjects.Dialogs;
using CryptoQuest.Characters.Events;
using UnityEngine;

namespace CryptoQuest.Characters
{
    public class NpcDialogController : MonoBehaviour, IDialog
    {
        [SerializeField] private DialogEventScriptableObject _dialogEventSO;
        [SerializeField] private DialogsScriptableObject _dialogSO;
        private int _currentIndex = 0;

        public int GetNextIndex()
        {
            _currentIndex = CalculateNextIndex();
            return _currentIndex;
        }

        public int GetCurrentDialogIndex()
        {
            return _currentIndex;
        }

        public int CalculateNextIndex()
        {
            if (IsDataEmpty()) return 0;
            return (_currentIndex + 1) % _dialogSO.Lines.Count;
        }

        private bool IsDataEmpty()
        {
            return _dialogSO.Lines.Count == 0;
        }

        public void ShowDialog()
        {
            _dialogEventSO.OnShow(_dialogSO.GetLine(_currentIndex));
        }
    }
}