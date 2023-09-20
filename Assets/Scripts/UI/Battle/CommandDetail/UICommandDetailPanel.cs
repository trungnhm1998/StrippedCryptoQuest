using System;
using System.Collections.Generic;
using CryptoQuest.UI.Battle.CommandsMenu;
using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandDetail
{
    public class UICommandDetailPanel : MonoBehaviour
    {
        public static Action<List<ButtonInfoBase>> RequestShowCommandDetail;

        [SerializeField] private GameObject _scrollViewContent;
        [SerializeField] private ChildButtonsActivator _childButonsActivator;
        [SerializeField] private UICommandDetailButton _buttonPrefab;

        private IObjectPool<UICommandDetailButton> _buttonPool;

        private void Awake()
        {
            _buttonPool ??= new ObjectPool<UICommandDetailButton>(OnCreate, OnGet,
                OnRelease, OnDestroyPool);
        }

        private void OnEnable()
        {
            RequestShowCommandDetail += ShowCommandDetail;
        }

        private void OnDisable()
        {
            RequestShowCommandDetail -= ShowCommandDetail;
        }

        public void ShowCommandDetail(List<ButtonInfoBase> buttonInfos)
        {
            SetActiveContent(true);

            for (int i = 0; i < buttonInfos.Count; i++)
            {
                ButtonInfoBase info = buttonInfos[i];
                UICommandDetailButton button = _buttonPool.Get();
                button.Init(info, i);
            }

            _childButonsActivator.CacheButtonTexts();
        }

        public void SetActiveButtons(bool isActive)
        {
            _childButonsActivator.SetActiveButtons(isActive);
        }

        public void SetActiveContent(bool value)
        {
            _scrollViewContent.SetActive(value);
        }

        private UICommandDetailButton OnCreate()
        {
            var button = Instantiate(_buttonPrefab, _scrollViewContent.transform);
            button.ObjectPool = _buttonPool;
            return button;
        }

        private void OnGet(UICommandDetailButton obj)
        {
            // To make sure that buttons is in order
            obj.transform.SetAsLastSibling();
            obj.gameObject.SetActive(true);
        }

        private void OnRelease(UICommandDetailButton obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPool(UICommandDetailButton obj)
        {
            Destroy(obj.gameObject);
        }
    }
}