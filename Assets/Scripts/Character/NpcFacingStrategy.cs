
using UnityEngine;
using CryptoQuest.Character.MonoBehaviours;

public class NpcFacingStrategy : IFacingStrategy
{
    public CharacterBehaviour.EFacingDirection Execute(Vector2 myPosition, Vector2 playerPosition)
    {
        Vector2 direction = playerPosition - myPosition;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return (direction.x < 0) ? CharacterBehaviour.EFacingDirection.West : CharacterBehaviour.EFacingDirection.East;
        else
            return (direction.y < 0) ? CharacterBehaviour.EFacingDirection.South : CharacterBehaviour.EFacingDirection.North;
    }
}