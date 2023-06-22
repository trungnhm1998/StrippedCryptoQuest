using Core.Runtime.Events.ScriptableObjects.Dialogs;

namespace Core.Runtime.Character
{
    public interface IDialog
    {
        public void SetDialogData(DialogsScriptableObject dialogSO);
        public string GetDialogKey();
    }
}