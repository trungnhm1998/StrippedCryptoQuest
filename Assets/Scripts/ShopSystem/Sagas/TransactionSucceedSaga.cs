using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class TransactionSucceedAction : ActionBase { }

    public class TransactionSucceedSaga : SagaBase<TransactionSucceedAction>
    {
        [SerializeField] private TransactionResultPanel _resultPanel;

        protected override void HandleAction(TransactionSucceedAction ctx)
        {
            _resultPanel.ShowSuccess();
        }
    }
}