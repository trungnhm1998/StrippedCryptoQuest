using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory;
using CryptoQuest.Menus.Home.UI.CharacterList;
using CryptoQuest.UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace CryptoQuest.Tavern.UI.Tooltip
{
    public interface ITooltipHeroProvider
    {
        public HeroSpec Hero { get; }
    }

    public class UIHeroTooltip : UITooltipBase
    {
        [SerializeField] private UICharacterDetails _characterDetails;
        [SerializeField] private HeroBehaviour _heroPrefab;

        protected HeroSpec _hero;
        private HeroBehaviour _mannequinHero;
        private GameObject _selectedGameObject;

        protected override bool CanShow()
        {
            _selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (_selectedGameObject == null) return false;
            var provider = _selectedGameObject.GetComponent<ITooltipHeroProvider>();
            if (provider == null) return false;
            if (provider.Hero == null || provider.Hero.IsValid() == false) return false;
            _hero = provider.Hero;
            return true;
        }

        protected override void Init()
        {
            _mannequinHero = Instantiate(_heroPrefab);
            _mannequinHero.Init(new PartySlotSpec()
            {
                Hero = _hero
            });
            _characterDetails.InspectCharacter(_mannequinHero);
        }
    }
}