using Core.Runtime.Events.ScriptableObjects.Dialogs;

namespace CryptoQuest.Characters
{
    public class NullDialog : IDialog
    {
        private static NullDialog _instance;

        public static IDialog Instance => _instance ??= new NullDialog();

        public void SetDialogData(DialogsScriptableObject dialogSO) { }

        public string GetDialogKey() => string.Empty;
    }
}