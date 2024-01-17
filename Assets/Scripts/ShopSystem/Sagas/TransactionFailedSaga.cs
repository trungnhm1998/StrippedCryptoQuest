using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class TransactionFailedAction : ActionBase { }

    public class TransactionFailedSaga : SagaBase<TransactionFailedAction>
    {
        [SerializeField] private TransactionResultPanel _resultPanel;

        protected override void HandleAction(TransactionFailedAction ctx)
        {
            _resultPanel.ShowFailed();
        }
    }
}