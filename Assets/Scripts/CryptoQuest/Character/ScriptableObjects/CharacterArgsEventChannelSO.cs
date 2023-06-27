using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest.Character.MonoBehaviours;
using UnityEngine.Events;

namespace CryptoQuest.Character.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Character Args Event Channel")]
    public class CharacterArgsEventChannelSO : ScriptableObject
    {
        public UnityAction<CharacterArgs> EventRaised;
        public void RaiseEvent(CharacterArgs args)
        {
            OnRaiseEvent(args);
        }

        private void OnRaiseEvent(CharacterArgs characterArgs)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(characterArgs);
        }
    }
    public class CharacterArgs
    {
        public Vector2 position;
        public CharacterBehaviour.EFacingDirection facingDirection;
    }
}