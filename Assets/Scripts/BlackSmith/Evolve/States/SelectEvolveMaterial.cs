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
            StateMachine.MaterialItem = null;
            EvolvableEquipmentList.EquipmentSelected += OnSelectMaterial;
            EvolvableEquipmentList.Filter(StateMachine.ItemToEvolve);
            StateMachine.ItemToEvolve.GetComponent<Button>().interactable = false;

            EventSystem.current.SetSelectedGameObject(EvolvableEquipmentList.Content.GetChild(1).gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            StateMachine.ItemToEvolve.GetComponent<Button>().interactable = true;
            EvolvableEquipmentList.EquipmentSelected -= OnSelectMaterial;
        }

        private void OnSelectMaterial(UIEquipmentItem equipment)
        {
            StateMachine.MaterialItem = equipment;
            fsm.RequestStateChange(EStates.ConfirmEvolve);
        }

        public override void OnCancel()
        {
            fsm.RequestStateChange(EStates.SelectEquipment);
        }
    }
}