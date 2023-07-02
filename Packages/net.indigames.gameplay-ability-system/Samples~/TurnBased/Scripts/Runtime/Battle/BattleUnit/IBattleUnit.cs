using System.Collections;
using System.Collections.Generic;

namespace Indigames.AbilitySystem.Sample
{
    public interface IBattleUnit
    {
        public void Init(BattleManager manager, AbilitySystemBehaviour owner);
        public void SetTeams(ref List<AbilitySystemBehaviour> ownerTeam, ref List<AbilitySystemBehaviour> targets);
        public IEnumerator Prepare();
        public IEnumerator Execute();
        public IEnumerator Resolve();
        public void OnDeath();
    }
}
    