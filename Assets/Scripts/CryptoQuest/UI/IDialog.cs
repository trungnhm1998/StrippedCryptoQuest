using Core.Runtime.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI
{
    public interface IDialog
    {
        public VoidEventChannelSO ShowDialogEvent { get; set; }
        public VoidEventChannelSO HideDialogEvent { get; set; }
        public bool IsShown { get; }
        public GameObject Content { get; set; }

        public void Show();
        public void Hide();
    }
}