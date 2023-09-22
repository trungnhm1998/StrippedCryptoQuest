using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Tag
{
    
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Tag", fileName = "Create Tag", order = 0)]
    public class TagSO : TagScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}