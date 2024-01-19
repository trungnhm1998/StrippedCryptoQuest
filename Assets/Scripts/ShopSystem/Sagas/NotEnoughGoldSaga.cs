using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class NotEnoughGoldAction : ActionBase { }

    public class NotEnoughGoldSaga : SagaBase<NotEnoughGoldAction>
    {
        [SerializeField] private TransactionResultPanel _resultPanel;

        protected override void HandleAction(NotEnoughGoldAction ctx)
        {
            _resultPanel.ShowFailed();
        }
    }
}