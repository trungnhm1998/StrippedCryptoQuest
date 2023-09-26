using System.Collections;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public class MetadSectionPresenter : MonoBehaviour
    {
        [SerializeField] private UIDimensionBoxMetadUI _dimensionBoxMetadUI;
        private IMetadModel _model;
        private float _inputDiamond;
        private bool _isIngameWallet;

        private void OnEnable()
        {
            StartCoroutine(Init());
            UIDimensionBoxMetadUI.ConfirmTransferEvent += SetCurrency;
            UIMetadSection.InputValueEvent += CheckTransferStatus;
            _model.CurrencyUpdated += UpdateCurrency;
        }

        private void OnDisable()
        {
            UIDimensionBoxMetadUI.ConfirmTransferEvent -= SetCurrency;
            UIMetadSection.InputValueEvent -= CheckTransferStatus;
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

        private void CheckTransferStatus(float inputValue, bool isIngameWallet)
        {
            var ingameMetad = _model.GetIngameMetad();
            var webMetad = _model.GetWebMetad();
            _isIngameWallet = isIngameWallet;
            _inputDiamond = inputValue;
            float currentDiamond = _isIngameWallet ? ingameMetad : webMetad;
            var isTransferSuccess = currentDiamond >= _inputDiamond && _inputDiamond != 0;
            _dimensionBoxMetadUI.ShowMessage(isTransferSuccess);
        }

        private void SetCurrency(bool isSuccess)
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