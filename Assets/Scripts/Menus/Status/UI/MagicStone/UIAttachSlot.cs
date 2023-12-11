using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIAttachSlot : MonoBehaviour
    {
        public event UnityAction Pressed;
        [SerializeField] private GameObject _selectedEffect;
        [SerializeField] private Transform _stoneContainer;

        public void OnPressed() => Pressed?.Invoke();
        public void Cache() => _selectedEffect.SetActive(true);
        public void UnCache() => _selectedEffect.SetActive(false);
    }
}