using System.Collections.Generic;
using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    public class UICommandDetailPanel : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private ChildButtonsActivator _childButonsActivator;
        [SerializeField] private UICommandDetailButton _buttonPrefab;
        [SerializeField] private bool _isOneTimeInit = true;

        private UICommandDetailButton _firstButton;
        private IObjectPool<UICommandDetailButton> _buttonPool;
        private List<UICommandDetailButton> _buttons = new();

        private void Awake()
        {
            _buttonPool ??= new ObjectPool<UICommandDetailButton>(OnCreate, OnGet,
                OnRelease, OnDestroyPool);
        }

        public void ShowCommandDetail(ICommandDetailModel model)
        {
            SetActiveContent(true);

            if (!_isOneTimeInit || _scrollRect.content.childCount <= 0)
            {
                InitButtons(model);
            }
            
            if (_firstButton != null) _firstButton.Select();
            _childButonsActivator.CacheButtonTexts();
        }

        private void InitButtons(ICommandDetailModel model)
        {
            ReleaseAllButton();

            for (int i = 0; i < model.Infos.Count; i++)
            {
                ButtonInfoBase info = model.Infos[i];
                UICommandDetailButton button = _buttonPool.Get();
                button.Init(info, i);
            }
        }

        public void SetActiveButtons(bool isActive)
        {
            _childButonsActivator.SetActiveButtons(isActive);
        }

        public void SetActiveContent(bool value)
        {
            _scrollRect.content.gameObject.SetActive(value);
        }

        private void ReleaseAllButton()
        {
            foreach (var button in _buttons)
            {
                _buttonPool.Release(button);
            }
            _buttons = new();
            _firstButton = null;
        }

        private UICommandDetailButton OnCreate()
        {
            var button = Instantiate(_buttonPrefab, _scrollRect.content.transform);
            return button;
        }

        private void OnGet(UICommandDetailButton button)
        {
            if (_firstButton == null) _firstButton = button;
            button.transform.SetAsLastSibling();
            button.gameObject.SetActive(true);
            _buttons.Add(button);
        }

        private void OnRelease(UICommandDetailButton button)
        {
            button.gameObject.SetActive(false);
        }

        private void OnDestroyPool(UICommandDetailButton button)
        {
            Destroy(button.gameObject);
        }
    }
}