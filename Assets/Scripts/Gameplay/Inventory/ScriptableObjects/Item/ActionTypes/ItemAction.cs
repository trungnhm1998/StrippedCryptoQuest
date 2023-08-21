using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes
{
    public class ItemAction : ActionDefinitionBase
    {
        protected override ActionSpecificationBase CreateInternal() => new ItemActionSpec();
    }

    public class ItemActionSpec : ActionSpecificationBase
    {
        protected override void OnExecute()
        {
           Presenter.Execute(); 
        }
    }
}