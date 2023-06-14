namespace CryptoQuest.SaveSystem
{
    public interface ISaveManager
    {
        public bool SaveData(SaveData saveData);
        public bool LoadSaveData(out SaveData saveData);
    }
}