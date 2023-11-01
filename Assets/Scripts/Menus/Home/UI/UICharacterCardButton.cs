using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Home.UI
{
    public class UICharacterCardButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _selectedEffect;

        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }

        public void EnableSelectingEffect(bool enable = true) => _selectedEffect.SetActive(enable);
    }
}