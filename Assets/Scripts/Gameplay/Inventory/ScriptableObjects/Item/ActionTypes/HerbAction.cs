using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes
{
    public class HerbAction : ActionDefinitionBase
    {
        protected override ActionSpecificationBase CreateInternal() => new HerbActionSpec();
    }

    public class HerbActionSpec : ActionSpecificationBase
    {
        protected override void OnExecute()
        {
           Presenter.UseHerb(); 
        }
    }
}