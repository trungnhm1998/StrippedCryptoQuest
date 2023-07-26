using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.NPC.Chest.Events
{
    public class ChestEventChannelSO : ScriptableObject
    {
        public event UnityAction<ChestData> Opened;

        public void Open(ChestData reward)
        {
            if (reward == null)
            {
                Debug.LogWarning("Open chest event raised, but no reward were provided.");
                return;
            }

            if (Opened == null)
            {
                Debug.LogWarning("Open chest event were raised, but no one listening.");
                return;
            }
            Opened.Invoke(reward);
        }

    }
}
