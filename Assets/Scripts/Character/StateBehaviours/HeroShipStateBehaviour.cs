using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.StateBehaviours
{
    public class HeroShipStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private VoidEventChannelSO _sailedShipEvent;
        [SerializeField] private VoidEventChannelSO _landedEvent;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _sailedShipEvent.RaiseEvent();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo,
            int layerIndex)
        {
            _landedEvent.RaiseEvent();
        }
    }
}