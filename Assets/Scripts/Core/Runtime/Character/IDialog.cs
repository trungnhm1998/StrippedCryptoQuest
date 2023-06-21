using Core.Runtime.Events.ScriptableObjects.Dialogs;

namespace Core.Runtime.Character
{
    public interface IDialog
    {
        public void SetDialog(DialogsScriptableObject dialogSO);
        public string GetDialog();
    }
}