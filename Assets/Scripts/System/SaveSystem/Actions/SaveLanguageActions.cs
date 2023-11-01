using CryptoQuest.Language;

namespace CryptoQuest.System.SaveSystem.Actions
{
    public class SaveLanguageAction : SaveActionBase<LanguageManager>
    {
        public SaveLanguageAction(LanguageManager obj): base(obj)
        {
        }
    }

    public class LoadLanguageAction : SaveActionBase<LanguageManager>
    {
        public LoadLanguageAction(LanguageManager obj): base(obj)
        {
        }
    }

    public class SaveLanguageCompletedAction : SaveCompletedActionBase
    {
        public SaveLanguageCompletedAction(bool result) : base(result)
        {
        }
    }

    public class LoadLanguageCompletedAction : SaveCompletedActionBase
    {
        public LoadLanguageCompletedAction(bool result) : base(result)
        {
        }
    }
}