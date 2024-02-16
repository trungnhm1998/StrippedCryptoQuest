using CryptoQuest.UI.Tooltips.Equipment;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI
{
    public class StatusMenuInspectHeroDispatcher : SagaBase<RequestInspectHeroAction>
    {
        [SerializeField] private UIStatusMenu _statusMenu;

        protected override void HandleAction(RequestInspectHeroAction ctx)
        {
            ActionDispatcher.Dispatch(new InspectHeroAction(_statusMenu.InspectingHero));
        }
    }
}