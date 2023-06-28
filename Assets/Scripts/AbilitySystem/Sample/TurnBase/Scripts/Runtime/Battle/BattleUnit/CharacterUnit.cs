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
            while (HasNoTarget())
            {
                yield return false;
            }
            
            while (!_isPerformSkillThisTurn)
            {
                yield return false;
            }

            yield return null;
        }

        private bool HasNoTarget()
        {
            return TargetContainer.Targets.Count <= 0;
        }
    }
}