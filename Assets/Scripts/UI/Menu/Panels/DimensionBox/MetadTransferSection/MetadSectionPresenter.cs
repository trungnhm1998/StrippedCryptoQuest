using System.Collections;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection
{
    public class MetadSectionPresenter : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool> _confirmTransferEvent;
        [SerializeField] private UIDimensionBoxMetadUI _dimensionBoxMetadUI;
        private IMetadModel _model;
        private float _inputDiamond;
        private bool _isIngameWallet;

        private void OnEnable()
        {
            StartCoroutine(Init());
            _model.CurrencyUpdated += UpdateCurrency;
        }

        private void OnDisable()
        {
            _model.CurrencyUpdated -= UpdateCurrency;
        }

        private void Awake()
        {
            _model = GetComponent<IMetadModel>();
        }

        private IEnumerator Init()
        {
            yield return _model.CoLoadData();
            var ingameMetad = _model.GetIngameMetad();
            var webMetad = _model.GetWebMetad();
            _dimensionBoxMetadUI.SetCurrentMetad(ingameMetad, webMetad);
        }

        public void CheckTransferStatus(float inputValue, bool isIngameWallet)
        {
            var ingameMetad = _model.GetIngameMetad();
            var webMetad = _model.GetWebMetad();
            _isIngameWallet = isIngameWallet;
            _inputDiamond = inputValue;
            float currentDiamond = _isIngameWallet ? ingameMetad : webMetad;
            var isTransferSuccess = currentDiamond >= _inputDiamond && _inputDiamond > 0;
            _confirmTransferEvent.Invoke(isTransferSuccess);
        }

        public void SetCurrency(bool isSuccess)
        {
            if (isSuccess)
                _model.UpdateCurrency(_inputDiamond, _isIngameWallet);

            var ingameMetad = _model.GetIngameMetad();
            var webMetad = _model.GetWebMetad();
            _dimensionBoxMetadUI.SetCurrentMetad(ingameMetad, webMetad);
        }

        private void UpdateCurrency(float ingameMetad, float webMetad)
        {
            _dimensionBoxMetadUI.SetCurrentMetad(ingameMetad, webMetad);
        }
    }
}