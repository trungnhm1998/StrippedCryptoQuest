using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    public class ActorChecker : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private ActiveActorEventChannelSO _activeActorEventChannel;
        [SerializeField] private ActorSO _actor;

        private void OnEnable() => _activeActorEventChannel.EventRaised += ActiveActor;
        private void OnDisable() => _activeActorEventChannel.EventRaised -= ActiveActor;

        private void ActiveActor(ActorSO currentActor, bool isActive)
        {
            if (_actor == null) return;

            if (currentActor != _actor) return;

            if (gameObject != null) _gameObject.SetActive(isActive);
        }
    }
}