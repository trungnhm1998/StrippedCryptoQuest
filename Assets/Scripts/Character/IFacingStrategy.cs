using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character
{
    public interface IFacingStrategy
    {
        public CharacterBehaviour.EFacingDirection Execute(Vector2 myPosition, Vector2 playerPosition);
    }
}