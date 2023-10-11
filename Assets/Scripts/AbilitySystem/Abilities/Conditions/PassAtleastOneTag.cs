using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities.Conditions
{
    /// <summary>
    /// Return true if the system has atleast one tag in the accept list
    /// OR the system don't have 1 tag in the deny list
    /// For example: ability cure all status and heal HP, MP
    /// can be active if system has atleast 1 abnormal tag or dont have FullHP, FullMP tag
    /// </summary>
    public class PassAtleastOneTag : IAbilityCondition
    {
        [SerializeField] private TagScriptableObject[] _acceptTags;
        [SerializeField] private TagScriptableObject[] _denyTags;

        public bool IsPass(AbilityConditionContext ctx)
        {
            foreach (var tag in _acceptTags)
            {
                if (ctx.System.TagSystem.HasTag(tag)) return true;
            }    

            foreach (var tag in _denyTags)
            {
                if (!ctx.System.TagSystem.HasTag(tag)) return true;
            }    
        
            return false;
        }
    }
}