using CryptoQuest.Gameplay.Character;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Battle.SelectHero.Mockup
{
    public class MockupSelectHeroModel : MonoBehaviour, ISelectHeroModel
    {
        public LocalizedString Label => _mockupLabel;

        [SerializeField] private LocalizedString _mockupLabel;
        
        private void OnEnable()
        {
            SelectHeroPresenter.ConfirmSelectCharacter += ConfirmSelected;
        }

        private void OnDisable()
        {
            SelectHeroPresenter.ConfirmSelectCharacter -= ConfirmSelected;
        }

        private void ConfirmSelected(CharacterSpec spec)
        {
            Debug.Log($"Selected {spec}");
        }
    }
}