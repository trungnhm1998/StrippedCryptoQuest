using System;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public interface ISaver
    {
        void Init(ISaveHandler saveHandler);
        void RegistEvents();
        void UnregistEvents();
    }

    [Serializable]
    public abstract class SaverBase : ISaver
    {
        protected ISaveHandler _saveHandler;
        protected SaveSystemSO _saveSystem => _saveHandler.SaveSystem;

        public virtual void Init(ISaveHandler saveHandler)
        {
            _saveHandler = saveHandler;
        }

        public abstract void RegistEvents();
        public abstract void UnregistEvents();
    }
}