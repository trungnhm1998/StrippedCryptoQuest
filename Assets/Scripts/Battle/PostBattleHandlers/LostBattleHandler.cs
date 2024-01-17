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
        [SerializeField] private BattleResultEventSO _battleLostEvent;
        [SerializeField] private AttributeWithMaxCapped[] _resetAttributes;

        protected override ResultSO.EState ResultState => ResultSO.EState.Lose;

        protected override void HandleResult()
        {
            TeleportToClosestTownAfterSceneUnloaded();
            _battleLostEvent.RaiseEvent(_battleBus.CurrentBattlefield);
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