using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest;
public interface ICharacter
{
    public CharacterStateSO CharacterStateSO { get; set; }
    public void SetFacingDirection(Character.EFacingDirection facingDirection);
    public Character.EFacingDirection GetFacingDirection();
    public void SaveFacingDirection(Character.EFacingDirection facingDirection);
}
