namespace CryptoQuest.SaveSystem.SaveObjects
{
    public abstract class SaveObjectBase<TRef> : ISaveObject
    {
        public TRef RefObject { get; private set; }

        public SaveObjectBase(TRef obj)
        {
            RefObject = obj;
        }

        public abstract string Key { get; }

        public abstract string ToJson();

        public abstract bool FromJson(string json);
    }
}