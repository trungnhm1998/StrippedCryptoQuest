using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class LostBattleHandler : PostBattleManager
    {
        [SerializeField] private BattleResultEventSO _battleCompletedEvent;
        [SerializeField] private int _divideGold = 2;
        [SerializeField] private AttributeWithMaxCapped[] _resetAttributes;
        private TinyMessageSubscriptionToken _lostToken;

        private void Awake()
        {
            _lostToken = BattleEventBus.SubscribeEvent<BattleLostEvent>(HandleLost);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_lostToken);
        }

        private BattleLostEvent _context;

        private void HandleLost(BattleLostEvent lostContext)
        {
            _context = lostContext;
            DecreaseGold();
            RestoreCharacter();
            AdditiveGameSceneLoader.SceneUnloaded += TeleportToClosestTownAfterSceneUnloaded;
            UnloadBattleScene();
            NotifyBattleResult(_context.Battlefield);
        }

        private void TeleportToClosestTownAfterSceneUnloaded(SceneScriptableObject scene)
        {
            AdditiveGameSceneLoader.SceneUnloaded -= TeleportToClosestTownAfterSceneUnloaded;
            if (scene != BattleSceneSO) return;

            FinishPresentationAndEnableInput();
        }

        private void NotifyBattleResult(Battlefield battlefield)
        {
            BattleResultInfo result = new()
            {
                IsWin = false, //obsolete 
                Battlefield = battlefield
            };
            _battleCompletedEvent.RaiseEvent(result);
        }

        private void DecreaseGold()
        {
            var inventoryController = ServiceProvider.GetService<IInventoryController>();
            var wallet = inventoryController.Inventory.WalletController.Wallet;

            var newGold = Mathf.FloorToInt(wallet.Gold.Amount / _divideGold);
            wallet.Gold.SetCurrencyAmount(newGold);
        }

        private void RestoreCharacter()
        {
            var party = ServiceProvider.GetService<IPartyController>();

            foreach (var character in party.Slots)
            {
                if (character.IsValid())
                {
                    ResetAttributeValue(character.HeroBehaviour);
                }
            }
        }

        private void ResetAttributeValue(HeroBehaviour hero)
        {
            for (int i = 0; i < _resetAttributes.Length; i++)
            {
                var attributeToReset = _resetAttributes[i];
                if (!hero.AttributeSystem.TryGetAttributeValue(attributeToReset.CappedAttribute,
                        out var cappedAttributeValue)) continue;
                hero.AttributeSystem.SetAttributeBaseValue(attributeToReset, cappedAttributeValue.CurrentValue);
            }
        }
    }
}