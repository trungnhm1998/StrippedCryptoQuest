using System.Linq;
using CryptoQuest.BlackSmith.Evolve.UI;

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
            CurrencyPresenter.Show();

            EvolvableEquipmentList.EquipmentSelected += OnSelectBaseItem;
            EvolvableEquipmentList.EquipmentHighlighted += OnHighlightItem;

            EquipmentsPresenter.InitEquipments(StateMachine.EvolvableInfos);
            EquipmentsPresenter.RenderEquipmentsForBaseItemSelection();
        }

        public override void OnExit()
        {
            base.OnExit();
            EvolvableEquipmentList.EquipmentSelected -= OnSelectBaseItem;
            EvolvableEquipmentList.EquipmentHighlighted -= OnHighlightItem;
        }

        public override void OnCancel()
        {
            CurrencyPresenter.Hide();
            EquipmentsPresenter.gameObject.SetActive(false);
            StateMachine.BackToOverview();
        }

        private void OnSelectBaseItem(UIEquipmentItem item)
        {
            StateMachine.ItemToEvolve = item;
            var info = StateMachine.EvolvableInfos.First(f => f.BeforeStars == item.Equipment.Data.Stars && f.Rarity == item.Equipment.Rarity.ID);
            StateMachine.EvolveEquipmentData = new EvolvableEquipmentData()
            {
                Equipment = item.Equipment,
                Level = item.Equipment.Level,
                Stars = item.Equipment.Data.Stars,
                AfterStars = info.AfterStars,
                MinLevel = info.MinLevel,
                MaxLevel = info.MaxLevel,
                Gold = info.Gold,
                Metad = info.Metad,
                Rate = info.Rate
            };
            StateMachine.RequestStateChange(EStates.SelectMaterial);
        }

        private void OnHighlightItem(UIEquipmentItem item)
        {
            EvolveSystem.EquipmentDetailPresenter.ShowEquipment(item.Equipment);
        }
    }
}