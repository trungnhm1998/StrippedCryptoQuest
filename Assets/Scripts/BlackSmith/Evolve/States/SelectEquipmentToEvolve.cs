using CryptoQuest.BlackSmith.Evolve.UI;
using UnityEngine.EventSystems;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class SelectEquipmentToEvolve : EvolveStateBase
    {
        public SelectEquipmentToEvolve(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            DialogsPresenter.Dialogue.SetMessage(EvolveSystem.SelectEquipmentToEvolveText).Show();
            StateMachine.MaterialItem = null;
            StateMachine.ItemToEvolve = null;
            EquipmentsPresenter.gameObject.SetActive(true);
            EvolvableEquipmentList.EquipmentSelected += OnSelectBaseItem;

            EquipmentsPresenter.EvolvableModel.Init();
            EquipmentsPresenter.EvolvableModel.FilterByInfos(StateMachine.EvolvableInfos);
            EvolvableEquipmentList.ClearEquipmentsWithException();
            EvolvableEquipmentList.RenderEquipments(EquipmentsPresenter.EvolvableModel.GetEvolableEquipments());

            if (EvolvableEquipmentList.Content.childCount > 0)
                EventSystem.current.SetSelectedGameObject(EvolvableEquipmentList.Content.GetChild(0).gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            EvolvableEquipmentList.EquipmentSelected -= OnSelectBaseItem;
        }

        public override void OnCancel()
        {
            EquipmentsPresenter.gameObject.SetActive(false);
            StateMachine.BackToOverview();
        }

        private void OnSelectBaseItem(UIEquipmentItem item)
        {
            StateMachine.ItemToEvolve = item;
            StateMachine.RequestStateChange(EStates.SelectMaterial);
        }
    }
}