using CryptoQuest.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// Handle abnormal logic which prevent character from doing something
    /// </summary>
    public class AbnormalBehaviour : CharacterComponentBase
    {
        public override void Init()
        {
            Character.TurnStarted += PreventCommandWhenCCed;
        }

        protected override void OnReset()
        {
            Character.TurnStarted -= PreventCommandWhenCCed;
        }

        private void PreventCommandWhenCCed()
        {
            var currentTagsInSystem = Character.AbilitySystem.TagSystem.GrantedTags;
            foreach (var gameplayTag in currentTagsInSystem)
            {
                if (!gameplayTag.IsChildOf(TagsDef.CrowdControl)) continue;
                if (!Character.TryGetComponent(out CommandExecutor commandExecutor))
                {
                    Debug.Log("Character does not have CommandExecutor component");
                    return;
                }
                commandExecutor.SetCommand(NullCommand.Instance);
                return;
            }
        }
    }
}