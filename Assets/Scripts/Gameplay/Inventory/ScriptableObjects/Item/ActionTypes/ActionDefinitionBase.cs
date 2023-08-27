using CryptoQuest.Gameplay.Inventory.Items;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes
{
    public abstract class ActionDefinitionBase : ScriptableObject
    {
        [SerializeField] private PresenterBinder _provider;

        public ActionSpecificationBase Create()
        {
            var action = CreateInternal();
            action.Bind(_provider);
            return action;
        }

        protected abstract ActionSpecificationBase CreateInternal();
    }

    public abstract class ActionSpecificationBase
    {
        public struct Context
        {
            public UsableInfo Item { get; set; }
        }
        private IActionPresenter _presenter;
        protected IActionPresenter Presenter => _presenter;
        public Context ActionContext;

        public void Bind(IActionPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Execute()
        {
            OnExecute();
        }

        protected virtual void OnExecute() { }
    }

}