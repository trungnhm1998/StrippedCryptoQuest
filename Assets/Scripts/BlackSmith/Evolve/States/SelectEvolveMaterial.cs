using CryptoQuest.BlackSmith.Evolve.UI;
using UnityEngine.EventSystems;

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

            EquipmentsPresenter.EvolvableModel.FilterByEquipment(StateMachine.ItemToEvolve.Equipment);
            EvolvableEquipmentList.ClearEquipmentsWithException(StateMachine.ItemToEvolve);
            EvolvableEquipmentList.RenderEquipments(EquipmentsPresenter.EvolvableModel.GetEvolableEquipments());

            StateMachine.ItemToEvolve.ButtonUI.interactable = false;
            StateMachine.ItemToEvolve.BaseTag.SetActive(true);

            if (EvolvableEquipmentList.Content.childCount > 1)
                EventSystem.current.SetSelectedGameObject(EvolvableEquipmentList.Content.GetChild(1).gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            EvolvableEquipmentList.EquipmentSelected -= OnSelectMaterial;
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
    }
}
