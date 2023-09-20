using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class QuestCollideController : MonoBehaviour
    {
        [SerializeField] private AbstractObjective _objectiveToComplete;

        [SerializeField] private ECollideActionType _actionType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_actionType == ECollideActionType.OnEnter)
                _objectiveToComplete.OnComplete();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_actionType == ECollideActionType.OnExit)
                _objectiveToComplete.OnComplete();
        }

        private enum ECollideActionType
        {
            OnEnter = 0,
            OnExit = 1
        }
    }
}