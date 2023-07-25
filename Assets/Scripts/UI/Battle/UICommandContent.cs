using CryptoQuest.UI.Battle.CommandsMenu;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.UI.Battle
{
    public class UICommandContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _value;
        private AbstractButtonInfo _abstractButtonInfo;

        private IObjectPool<UICommandContent> _objectPool;

        public IObjectPool<UICommandContent> ObjectPool
        {
            set => _objectPool = value;
        }

        public override void Init(AbstractButtonInfo info)
        {
            _label.text = info.Name;
            _value.text = info.Value;
            _abstractButtonInfo = info;
        }
        
        public void HandleClick()
        {
            _abstractButtonInfo.HandleClick();
        }

        private void ReleaseToPool()
        {
            _objectPool?.Release(this);
        }

        private void OnDisable()
        {
            ReleaseToPool();
        }
    }
}