
using UnityEngine;
using CryptoQuest.Character.MonoBehaviours;

public class NpcFacingStrategy : IFacingStrategy
{
    public CharacterBehaviour.EFacingDirection Execute(Vector2 myPosition, Vector2 playerPosition)
    {
        Vector2 test = playerPosition - myPosition;

        Vector2 abs = new Vector2(Mathf.Abs(test.x), Mathf.Abs(test.y));


        if (test.x < 0 && abs.x > abs.y)
            return CharacterBehaviour.EFacingDirection.West;
        else if (test.x > 0 && abs.x > abs.y)
            return CharacterBehaviour.EFacingDirection.East;
        if (test.y < 0 && abs.x < abs.y)
            return CharacterBehaviour.EFacingDirection.South;
        else if (test.y > 0 && abs.x < abs.y)
            return CharacterBehaviour.EFacingDirection.North;

        return CharacterBehaviour.EFacingDirection.North;
    }
}