using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using CryptoQuest.Menu;
using System;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Battle.CommandDetail
{
    public class UICommandDetailButton : MonoBehaviour
    {
        public static event Action<int> InspectingButton;

        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _value;
        [SerializeField] private MultiInputButton _button;

        private ButtonInfoBase _buttonInfo;
        private int _index;

        private IObjectPool<UICommandDetailButton> _objectPool;
        public IObjectPool<UICommandDetailButton> ObjectPool
        {
            set => _objectPool = value;
        }

        public void Init(ButtonInfoBase info, int index)
        {
            _index = index;
            _label.text = info.Label;
            _value.text = info.Value;
            _buttonInfo = info;
            SetupButtonInfo();
        }

        private void OnEnable()
        {
            _button.Selected += OnSelectButton;
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelectButton;
            ReleaseToPool();
        }

        private void SetupButtonInfo()
        {
            _button.interactable = _buttonInfo.IsInteractable; 
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(_buttonInfo.OnHandleClick);
            if (_index == 0) _button.Select();
        }

        public void OnSelectButton()
        {
            InspectingButton?.Invoke(_index);
        }

        private void ReleaseToPool()
        {
            _objectPool?.Release(this);
        }
    }
}