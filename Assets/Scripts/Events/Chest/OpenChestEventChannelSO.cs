using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI
{
    public class OpenChestEventChannelSO : ScriptableObject
    {
        public event UnityAction<ChestDataSO> EventRaised;

        public void Open(ChestDataSO reward)
        {
            if (reward == null)
            {
                Debug.LogWarning("A request for showing reward has been made, but not reward were provided.");
                return;
            }

            if (EventRaised == null)
            {
                Debug.LogWarning("A request for showing reward has been made, but no one listening.");
                return;
            }
            EventRaised.Invoke(reward);
        }

    }
}
