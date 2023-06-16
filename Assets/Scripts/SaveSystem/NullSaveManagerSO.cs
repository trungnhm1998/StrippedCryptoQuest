using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public class NullSaveManagerSO : SaveManagerSO
    {
        public const string DEFAULT_PLAYER_NAME = "Null Player";

        public override bool Save(SaveData saveData)
        {
            return true;
        }

        public override bool Load(out SaveData saveData)
        {
            saveData = new SaveData()
            {
                playerName = DEFAULT_PLAYER_NAME,
            };
            return true;
        }
    }
}