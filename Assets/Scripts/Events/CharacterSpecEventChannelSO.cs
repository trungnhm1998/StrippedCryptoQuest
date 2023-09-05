using CryptoQuest.Gameplay.Character;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Character Spec Event Channel")]
    public class CharacterSpecEventChannelSO : ScriptableObject
    {
        public UnityAction<CharacterSpec> EventRaised;

        public void RaiseEvent(CharacterSpec character)
        {
            this.CallEventSafely<CharacterSpec>(EventRaised, character);
        }
    }
}