using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIAttachSlot : MonoBehaviour
    {
        public event UnityAction Pressed;
        [SerializeField] private GameObject _selectedEffect;

        public void OnPressed() => Pressed?.Invoke();
        public void Cache() => _selectedEffect.SetActive(true);
        public void UnCache() => _selectedEffect.SetActive(false);
    }
}