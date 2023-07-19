namespace CryptoQuest.System.Settings
{
    public interface IStringValidator
    {
        public EValidation Validate(string input);
    }
}