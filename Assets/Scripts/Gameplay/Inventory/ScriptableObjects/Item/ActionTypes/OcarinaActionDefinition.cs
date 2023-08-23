using CryptoQuest.UI.Menu.Panels.Item.Ocarina;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes
{
    public class OcarinaActionDefinition : ActionDefinitionBase
    {
        protected override ActionSpecificationBase CreateInternal() => new OcarinaActionSpec();
    }

    public class OcarinaActionSpec : ActionSpecificationBase
    {
        protected override void OnExecute()
        {
            Presenter.Execute();
        }
    }
}