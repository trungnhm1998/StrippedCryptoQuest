using System.Collections;
using CryptoQuest.Quest.Actor.Categories;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "QuestSystem/Actions/ActiveObjectAction", fileName = "ActiveObjectAction", order = 0)]
    public class ActiveObjectAction : NextAction
    {
        [SerializeField] private NormalActorSO _actor;
        [SerializeField] private ActiveActorEventChannelSO _activeActorEventChannel;

        [field: SerializeField] public float Delay { get; set; }

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(Delay);
            _activeActorEventChannel.RaiseEvent(_actor, true);
        }
    }
}