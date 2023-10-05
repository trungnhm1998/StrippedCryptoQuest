using CryptoQuest.Battle.Events;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    public class UIStatusIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public IObjectPool<UIStatusIcon> ObjectPool { get; set; }

        public void SetIcon(Sprite iconSprite)
        {
            if (iconSprite == null) return;
            _icon.sprite = iconSprite;
        }

        public void ReleaseToPool()
        {
            ObjectPool?.Release(this);
        }
    }
}
