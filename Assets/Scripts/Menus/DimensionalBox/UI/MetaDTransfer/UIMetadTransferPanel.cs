using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class UIMetadTransferPanel : MonoBehaviour
    {
        [field: SerializeField] public InputMediatorSO Input { get; private set; }
        [field: SerializeField] public Button GameButton { get; private set; }
        [field: SerializeField] public Button DimensionalBoxButton { get; private set; }
        [field: SerializeField] public InputField TransferAmountInput { get; private set; }

        private void OnEnable()
        {
            Invoke(nameof(SelectDefaultButton), 0);
        }

        private void SelectDefaultButton() => GameButton.Select();
    }
}