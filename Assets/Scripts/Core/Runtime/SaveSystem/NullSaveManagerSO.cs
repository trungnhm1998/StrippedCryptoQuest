namespace Core.Runtime.SaveSystem
{
    public class NullSaveManagerSO : SaveManagerSO
    {
        public const string DefaultPlayerName = "Null Player";

        public override bool Save(SaveData saveData)
        {
            return true;
        }

        public override bool Load(out SaveData saveData)
        {
            saveData = new SaveData()
            {
                playerName = DefaultPlayerName,
            };
            return true;
        }
    }
}