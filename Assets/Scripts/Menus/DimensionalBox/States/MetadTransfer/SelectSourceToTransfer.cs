using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    // public class SelectSourceToTransfer : StateBase
    // {
    //     private readonly UIMetadTransferPanel _metaDTransferPanel;
    //
    //     public SelectSourceToTransfer(GameObject transferPanel)
    //     {
    //         _metaDTransferPanel = transferPanel.GetComponent<UIMetadTransferPanel>();
    //     }
    //
    //     protected override void OnEnter()
    //     {
    //         _metaDTransferPanel.gameObject.SetActive(true);
    //         Menu.Input.MenuCancelEvent += ToSelectTransferTypeState;
    //         _metaDTransferPanel.TransferSourceChanged += FocusInputAmount;
    //         _metaDTransferPanel.SelectDefaultButton();
    //         
    //         ActionDispatcher.Dispatch(new GetToken());
    //     }
    //
    //     protected override void OnExit()
    //     {
    //         _metaDTransferPanel.TransferSourceChanged -= FocusInputAmount;
    //         Menu.Input.MenuCancelEvent -= ToSelectTransferTypeState;
    //     }
    //
    //     private void FocusInputAmount()
    //     {
    //         Menu.ChangeState(Menu.InputTransferAmount);
    //     }
    //
    //     private void ToSelectTransferTypeState() => Menu.ChangeState(Menu.Landing);
    // }
}