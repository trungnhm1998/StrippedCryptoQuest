using System;
using CryptoQuest.BlackSmith.Evolve.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class SelectEvolveMaterial : EvolveStateBase
    {
        public SelectEvolveMaterial(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            DialogsPresenter.Dialogue.SetMessage(EvolveSystem.SelectMaterialText).Show();
            StateMachine.MaterialItem = null;
            EvolvableEquipmentList.EquipmentSelected += OnSelectMaterial;
            EvolvableEquipmentList.EquipmentHighlighted += OnHighlightItem;

            EquipmentsPresenter.EvolvableModel.FilterByEquipment(StateMachine.ItemToEvolve.Equipment);
            EvolvableEquipmentList.ClearEquipmentsWithException(StateMachine.ItemToEvolve);
            EvolvableEquipmentList.RenderEquipments(EquipmentsPresenter.EvolvableModel.GetEvolableEquipments());

            StateMachine.ItemToEvolve.ButtonUI.interactable = false;
            StateMachine.ItemToEvolve.BaseTag.SetActive(true);

            var buttons = EvolvableEquipmentList.Content.GetComponentsInChildren<Button>();
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            if (buttons.Length > 1)
            {
                var item = buttons[1].GetComponent<Button>();
                item.OnSelect(null); // trigger highlight button
                return;
            }

            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            EvolvableEquipmentList.EquipmentSelected -= OnSelectMaterial;
            EvolvableEquipmentList.EquipmentHighlighted -= OnHighlightItem;
        }

        private void OnSelectMaterial(UIEquipmentItem equipment)
        {
            StateMachine.MaterialItem = equipment;
            fsm.RequestStateChange(EStates.ConfirmEvolve);
        }

        public override void OnCancel()
        {
            StateMachine.ItemToEvolve.ResetItemStates();
            fsm.RequestStateChange(EStates.SelectEquipment);
        }

        private void OnHighlightItem(UIEquipmentItem item)
        {
            if (item == StateMachine.ItemToEvolve) return;
            EvolveSystem.EquipmentDetailPresenter.ShowEquipment(item.Equipment);
            EvolveSystem.EquipmentDetailPresenter.ShowPreview(StateMachine.EvolveEquipmentData);
        }
    }
}
