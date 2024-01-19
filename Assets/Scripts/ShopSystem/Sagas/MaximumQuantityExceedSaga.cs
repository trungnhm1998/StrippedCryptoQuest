using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class MaximumQuantityExceedAction : ActionBase { }

    public class MaximumQuantityExceedSaga : SagaBase<MaximumQuantityExceedAction>
    {
        [SerializeField] private LocalizedString _maximumExceedMessage;
        [SerializeField] private TransactionResultPanel _resultPanel;

        protected override void HandleAction(MaximumQuantityExceedAction ctx)
        {
            _resultPanel.ShowDialog(_maximumExceedMessage);
        }
    }
}