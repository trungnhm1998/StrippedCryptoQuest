namespace CryptoQuest.Quests
{
    public interface IQuestDefinition
    {
        public bool Completed { get; set; }

        public delegate void StatusChangedEvent(bool hasCompleted);

        public event StatusChangedEvent StatusChanged;
    }
}