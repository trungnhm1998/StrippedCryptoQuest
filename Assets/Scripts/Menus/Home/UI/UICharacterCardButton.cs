using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Menus.Home.UI
{
    public class UICharacterCardButton : MultiInputButton
    {
        public static UnityAction<UICharacterCardButton> SelectedEvent;

        [SerializeField] private GameObject _selectedEffect;

        public void CardButtonOnPressed()
        {
            SelectedEvent?.Invoke(this);
            _selectedEffect.SetActive(true);
        }

        public void BackToNormalState()
        {
            _selectedEffect.SetActive(false);
        }
    }
}