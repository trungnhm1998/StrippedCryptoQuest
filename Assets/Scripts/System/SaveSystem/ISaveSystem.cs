namespace CryptoQuest.SaveSystem
{
    public interface ISaveSystem
    {
        SaveData SaveData { get; set; }
        string PlayerName { get; set; }

        bool SaveObject(ISaveObject jObject);
        bool LoadObject(ISaveObject jObject);

        bool Save();
        bool Load();
    }
}