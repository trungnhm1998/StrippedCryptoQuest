namespace IndiGames.Core.SaveSystem
{
    public class NullSaveManagerSO : SaveManagerSO
    {
        private SaveData saveDate;

        public override bool Save(SaveData saveData)
        {
            this.saveDate = saveData;
            return true;
        }

        public override bool Load(out SaveData saveData)
        {
            saveData = this.saveDate;
            return true;
        }
    }
}