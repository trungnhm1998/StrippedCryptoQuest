using UnityEngine;
using System.Collections;

namespace Indigames.AbilitySystem.Sample
{
    public class CharacterUnit : BattleUnitBase
    {
        public override IEnumerator Execute()
        {
            yield return base.Execute();
            // Wait Select monster > wait activate skill > run effect > next unit
            bool hasNoTarget = TargetContainer.Targets.Count <= 0;
            Debug.Log($"CharacterUnit::Execute: {hasNoTarget}");
            while (hasNoTarget)
            {
                yield return false;
            }
            
            while (!_isPerformSkillThisTurn)
            {
                yield return false;
            }

            yield return null;
        }
    }
}