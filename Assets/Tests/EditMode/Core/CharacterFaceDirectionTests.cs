using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CryptoQuest;
using CryptoQuest.Characters;

namespace Tests.EditMode
{
    public class CharacterFaceDirectionTests
    {
        [Test]
        public void CharacterFaceDirection_Default_Return_South()
        {
            var character = new PlayerController();
            Assert.AreEqual(Character.EFacingDirection.South, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToNorth_Return_North()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SetFacingDirection(Character.EFacingDirection.North);
            Assert.AreEqual(Character.EFacingDirection.North, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToSouth_Return_South()
        {
            var character = new PlayerController();
            character.SetFacingDirection(Character.EFacingDirection.South);
            Assert.AreEqual(Character.EFacingDirection.South, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToWest_Return_West()
        {
            var character = new PlayerController();
            character.SetFacingDirection(Character.EFacingDirection.West);
            Assert.AreEqual(Character.EFacingDirection.West, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToEast_Return_East()
        {
            var character = new PlayerController();
            character.SetFacingDirection(Character.EFacingDirection.East);
            Assert.AreEqual(Character.EFacingDirection.East, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToDefault_Return_South()
        {
            var character = new PlayerController();
            character.SetFacingDirection(new Character.EFacingDirection());
            Assert.AreEqual(Character.EFacingDirection.South, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SaveFacingDirection_Return_South()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SaveFacingDirection(Character.EFacingDirection.South);
            Assert.AreEqual(Character.EFacingDirection.South, character.CharacterStateSO.facingDirection);
        }
        [Test]
        public void CharacterFaceDirection_SaveFacingDirection_Return_North()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SaveFacingDirection(Character.EFacingDirection.North);
            Assert.AreEqual(Character.EFacingDirection.North, character.CharacterStateSO.facingDirection);
        }
        [Test]
        public void CharacterFaceDirection_SaveFacingDefaultDirection_Return_South()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SaveFacingDirection(new Character.EFacingDirection());
            Assert.AreEqual(Character.EFacingDirection.South, character.CharacterStateSO.facingDirection);
        }
    }
}

