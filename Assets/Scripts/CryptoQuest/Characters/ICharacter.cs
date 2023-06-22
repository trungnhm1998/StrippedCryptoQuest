using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest;
public interface ICharacter
{
    public void SetFacingDirection(Character.EFacingDirection facingDirection);
    public Character.EFacingDirection GetFacingDirection();
}
