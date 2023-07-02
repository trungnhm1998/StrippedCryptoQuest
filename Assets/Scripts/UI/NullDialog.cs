using CryptoQuest.Character;

namespace CryptoQuest.UI
{
    public class NullDialog : IDialog
    {
        private static NullDialog _instance;

        public static IDialog Instance => _instance ??= new NullDialog();

        public void Show() { }

        public void Hide() { }
        public void SetData(DialogArgs args) { }
    }
}