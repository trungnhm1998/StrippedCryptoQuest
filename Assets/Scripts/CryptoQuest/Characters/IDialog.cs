using Core.Runtime.Events.ScriptableObjects.Dialogs;

namespace CryptoQuest.Characters
{
    public interface IDialog
    {
        public void SetDialogData(DialogsScriptableObject dialogSO);
        public string GetDialogKey();
    }
}