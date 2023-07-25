using CryptoQuest.UI.Battle.CommandsMenu;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UICommandContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _value;
        public Button Button;

        private IObjectPool<UICommandContent> _objectPool;

        public IObjectPool<UICommandContent> ObjectPool
        {
            set => _objectPool = value;
        }

        public override void Init(ButtonInfo info)
        {
            _label.text = info.Name;
            _value.text = info.Value;
            Button.onClick.AddListener(info.Clicked);
        }

        public void ReleaseToPool()
        {
            _objectPool?.Release(this);
        }

        private void OnDisable()
        {
            ReleaseToPool();
        }
    }
}