using CryptoQuest.UI.Battle.CommandsMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

namespace CryptoQuest.UI.Battle
{
    public class UICommandContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private Button _button;
        private AbstractButtonInfo _buttonInfo;

        private IObjectPool<UICommandContent> _objectPool;

        public IObjectPool<UICommandContent> ObjectPool
        {
            set => _objectPool = value;
        }

        public override void Init(AbstractButtonInfo info)
        {
            _label.text = info.Name;
            _value.text = info.Value;
            _button.interactable = info.IsInteractable; 
            _buttonInfo = info;
        }
        
        public void HandleClick()
        {
            _buttonInfo.OnHandleClick();
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