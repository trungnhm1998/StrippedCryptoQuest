using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Hero.ChangeClass
{
    public class MainHeroClassChanger : MonoBehaviour
    {
        [SerializeField] private Origin _mainOrigin;
        [SerializeField] private UnitSO _newHeroClassParameter;
        [SerializeField] private VoidEventChannelSO _changeClassEvent;
        [SerializeField] private PartyManager _partyManager;

        private HeroSpec _heroSpec;

        private void OnEnable()
        {
            _changeClassEvent.EventRaised += ChangeClassHero;
        }

        private void OnDisable()
        {
            _changeClassEvent.EventRaised -= ChangeClassHero;
        }

        private void ChangeClassHero()
        {
            foreach (var member in _partyManager.Slots)
            {
                if (!member.IsValid()) continue;
                if (member.Spec.Hero.Origin != _mainOrigin) continue;
                RemoveEquippingItems(member.HeroBehaviour);
                UpdateHeroStatsAndClass(member.Spec.Hero);
                return;
            }
        }

        private void RemoveEquippingItems(HeroBehaviour heroBehaviour)
        {
            var equipmentsController = heroBehaviour.GetComponent<EquipmentsController>();
            equipmentsController.UnequipAll(); // use this for unequip gems/skills/effects
        }

        private void UpdateHeroStatsAndClass(HeroSpec hero)
        {
            hero.Stats = _newHeroClassParameter.Stats;
            hero.Class = _newHeroClassParameter.Class;
            hero.Experience = 0;
        }
    }
}