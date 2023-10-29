using System;
using UnityEngine;

namespace CryptoQuest.UI.Core
{
    public class LoadingController : MonoBehaviour
    {
        public static Action OnEnableLoadingPanel;
        public static Action OnDisableLoadingPanel;
        [SerializeField] private GameObject _loadingPanel;

        private void OnEnable()
        {
            OnEnableLoadingPanel += OnEnableLoading;
            OnDisableLoadingPanel += OnDisableLoading;
        }

        private void OnDisable()
        {
            OnEnableLoadingPanel -= OnEnableLoading;
            OnDisableLoadingPanel -= OnDisableLoading;
        }

        private void OnEnableLoading()
        {
            _loadingPanel.gameObject.SetActive(true);
        }

        private void OnDisableLoading()
        {
            _loadingPanel.gameObject.SetActive(false);
        }
    }
}