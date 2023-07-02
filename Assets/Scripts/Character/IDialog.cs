namespace CryptoQuest.Character
{
    public class DialogArgs { }

    public interface IDialog
    {
        public void Show();
        public void Hide();
        public void SetData(DialogArgs args);
    }
}