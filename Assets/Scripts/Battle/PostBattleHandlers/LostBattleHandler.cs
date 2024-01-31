using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Map.CheckPoint;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class LostBattleHandler : PostBattleManager
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _gold;

        protected override ResultSO.EState ResultState => ResultSO.EState.Lose;

        protected override void HandleResult()
        {
            TeleportToClosestTownAfterSceneUnloaded();
            DecreaseGold();
            RestoreCharacter();
        }

        private void TeleportToClosestTownAfterSceneUnloaded()
        {
            var checkPointController = ServiceProvider.GetService<ICheckPointController>();
            checkPointController.BackToCheckPoint();
        }

        private void DecreaseGold()
        {
            ActionDispatcher.Dispatch(new UpdateGoldAction(Mathf.FloorToInt(_wallet[_gold].Amount / 2)));
        }

        private void RestoreCharacter()
        {
            ActionDispatcher.Dispatch(new RestorePartyAction());
        }
    }
}