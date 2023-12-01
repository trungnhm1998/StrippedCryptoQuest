using CryptoQuest.BlackSmith.Upgrade.Actions;
using CryptoQuest.BlackSmith.Upgrade.Presenters;
using CryptoQuest.BlackSmith.Upgrade.States;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.Sagas
{
    public class DebugRequestUpgradeSaga : SagaBase<RequestUpgrade>
    {
        [SerializeField] private WalletSO _walletSO;
        [SerializeField] private CurrencySO _currencySO;
        [SerializeField] private ConfigUpgradePresenter _configPresenter;
        [SerializeField] private float _delay = 1f;

        private RequestUpgrade _context;

        protected override void HandleAction(RequestUpgrade ctx)
        {
            _context = ctx;
            ActionDispatcher.Dispatch(new ShowLoading());

            Invoke(nameof(DispactResponse), _delay);
        }

        private void DispactResponse()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            
            var goldAfter = _walletSO[_currencySO].Amount - _configPresenter.GoldNeeded;

            var equipmentResponse = new UpgradeResponse.EquipmentResponseData()
            {
                equipment = new EquipmentResponse()
                {
                    id = _context.EquipmentToUpgrade.Id,
                    lv = _context.UpgradeLevel
                }
            };

            ActionDispatcher.Dispatch(new UpgradeResponsed(new UpgradeResponse()
            {
                gold = (int) goldAfter,
                data = equipmentResponse
            }));
        }
    }
}