
using UnityEngine;
using CryptoQuest.Character.MonoBehaviours;

public interface IFacingStrategy
{
    public CharacterBehaviour.EFacingDirection Execute(Vector2 myPosition, Vector2 playerPosition);
}