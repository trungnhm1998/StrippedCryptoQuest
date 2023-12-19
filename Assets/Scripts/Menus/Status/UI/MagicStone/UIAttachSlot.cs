using CryptoQuest.Item.MagicStone;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIAttachSlot : MonoBehaviour
    {
        public event UnityAction Pressed;
        [SerializeField] private GameObject _selectedEffect;
        [SerializeField] private UISingleStone _singleStone;
        public UISingleStone SingleStoneUI => _singleStone;

        public void OnPressed() => Pressed?.Invoke();
        public void Cache() => _selectedEffect.SetActive(true);
        public void UnCache() => _selectedEffect.SetActive(false);

        public void Attach(IMagicStone stoneData)
        {
            _singleStone.SetInfo(stoneData);
            _singleStone.gameObject.SetActive(true);
        }

        public void Detach()
        {
            _singleStone.gameObject.SetActive(false);
        }
    }
}