namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// cache target so other component could work with it
    /// </summary>
    public interface ITargeting
    {
        public Character Target { get; set; }
    }
}