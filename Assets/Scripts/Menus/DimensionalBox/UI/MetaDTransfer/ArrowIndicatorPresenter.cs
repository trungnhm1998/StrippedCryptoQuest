using UnityEngine;
using CryptoQuest.Inventory.Currency;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class ArrowIndicatorPresenter : MonoBehaviour
    {
        [SerializeField] private UIMetadSourceButton _diamondButton;
        [SerializeField] private UIMetadSourceButton _metadButton;
        [SerializeField] private GameObject _arrow;
        [SerializeField] private Vector2 _diamondDirection = Vector2.right;
        [SerializeField] private Vector2 _metadDirection = Vector2.left;

        private readonly Vector2 _defaultDirection = Vector2.up;

        private void OnEnable()
        {
            _diamondButton.SelectedCurrency += PointArrowToDiamond;
            _diamondButton.Inpspected += PointArrowToDiamond;
            _metadButton.SelectedCurrency += PointArrowToMetad;
            _metadButton.Inpspected += PointArrowToMetad;
        }
        
        private void OnDisable()
        {
            _diamondButton.SelectedCurrency -= PointArrowToDiamond;
            _diamondButton.Inpspected -= PointArrowToDiamond;
            _metadButton.SelectedCurrency -= PointArrowToMetad;
            _metadButton.Inpspected -= PointArrowToMetad;
        }

        public void SetActiveArrow(bool active)
        {
            _arrow.SetActive(active);
        }

        private void SetArrowPointing(Vector2 direction)
        {
            _arrow.transform.rotation = Quaternion.Euler(0, 0,
                Vector2.SignedAngle(_defaultDirection, direction));
        }

        private void PointArrowToDiamond(CurrencySO _)
        {
            SetArrowPointing(_diamondDirection);
        }

        private void PointArrowToMetad(CurrencySO _)
        {
            SetArrowPointing(_metadDirection);
        }
    }
}