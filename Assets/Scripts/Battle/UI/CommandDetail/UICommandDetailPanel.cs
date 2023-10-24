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
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private ChildButtonsActivator _childButonsActivator;
        [SerializeField] private UICommandDetailButton _buttonPrefab;
        [SerializeField] private VerticalButtonSelector _buttonSelector;
        [SerializeField] private bool _isOneTimeInit = true;

        private IObjectPool<UICommandDetailButton> _buttonPool;
        private List<UICommandDetailButton> _buttons = new();

        private bool _interactable = true;
        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                _buttonSelector.Interactable = value;
            }
        }

        private void Awake()
        {
            _buttonPool ??= new ObjectPool<UICommandDetailButton>(OnCreate, OnGet,
                OnRelease, OnDestroyPool);
        }

        public void ShowCommandDetail(ICommandDetailModel model)
        {
            SetActiveContent(true);

            if (!_isOneTimeInit || _buttonContainer.childCount <= 0)
            {
                InitButtons(model);
            }

            _childButonsActivator.CacheButtonTexts();
        }

        public void SelectFirstButton()
        {
            _buttonSelector.SelectFirstButton();
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
            _buttonContainer.gameObject.SetActive(value);
        }

        private void ReleaseAllButton()
        {
            foreach (var button in _buttons)
            {
                _buttonPool.Release(button);
            }
            _buttons = new();
        }

        private UICommandDetailButton OnCreate()
        {
            var button = Instantiate(_buttonPrefab, _buttonContainer.transform);
            return button;
        }

        private void OnGet(UICommandDetailButton button)
        {
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