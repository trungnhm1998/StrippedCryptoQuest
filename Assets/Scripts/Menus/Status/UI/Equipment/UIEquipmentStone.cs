using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    [RequireComponent(typeof(Image))]
    public class UIEquipmentStone : MonoBehaviour
    {
        [field: SerializeField] public Image StoneIcon { get; private set; }
        [SerializeField] private Sprite _default;

        private void OnValidate()
        {
            StoneIcon = GetComponent<Image>();
        }

        public void ResetUI()
        {
            StoneIcon.sprite = _default;
        }
    }
}