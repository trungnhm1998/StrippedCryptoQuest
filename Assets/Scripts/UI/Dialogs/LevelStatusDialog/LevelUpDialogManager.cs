using CryptoQuest.Battle.Actions;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Quest;
using CryptoQuest.Quest.Controllers;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs.LevelStatusDialog
{
    public class LevelUpDialogManager : MonoBehaviour
    {
        [SerializeField] private LevelStatusDialogEventChannelSO _levelUpDialogEvent;
        [SerializeField] private LevelStatusDialogData _levelStatusData;
        private TinyMessageSubscriptionToken _levelUpEvent;
        private TinyMessageSubscriptionToken _getTargetIsLevelUp;
        
        private void OnEnable()
        {
            _levelStatusData.ClearListTarget();
            _levelUpEvent = ActionDispatcher.Bind<LevelUpAfterAddExpAction>(HandleAction);
            _getTargetIsLevelUp = ActionDispatcher.Bind<ShowLevelUpAction>(ShowDialog);
        }
        
        private void OnDisable()
        {
            ActionDispatcher.Unbind(_levelUpEvent);
            ActionDispatcher.Unbind(_getTargetIsLevelUp);
        }

        private void HandleAction(LevelUpAfterAddExpAction ctx)
        {
            if (!ctx.IsLevelUp) return;
            _levelStatusData.TargetIsLevelUp(ctx.Hero.Spec.Origin.DetailInformation.LocalizedName);
        }

        private void ShowDialog(ShowLevelUpAction ctx)
        {
            if (!_levelStatusData.IsValid())
            {
                ActionDispatcher.Dispatch(new ResumeCutsceneAction());
                return;
            }
            _levelUpDialogEvent.Show(_levelStatusData);
        }
    }
}