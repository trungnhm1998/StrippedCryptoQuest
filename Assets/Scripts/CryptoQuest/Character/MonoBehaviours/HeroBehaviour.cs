using Core.Runtime.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class HeroBehaviour : CharacterBehaviour
    {
        [SerializeField, ReadOnly] private EFacingDirection _facingDirection;
    }
}