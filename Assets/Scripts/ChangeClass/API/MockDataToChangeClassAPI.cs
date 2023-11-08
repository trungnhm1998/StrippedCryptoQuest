using CryptoQuest.System;
using System;
using System.Net;
using UnityEngine;
using UniRx;
using CryptoQuest.Networking;
using CryptoQuest.ChangeClass.View;
using Newtonsoft.Json;
using CryptoQuest.UI.Actions;
using CryptoQuest.Core;
using CryptoQuest.Sagas;
using System.Collections;

namespace CryptoQuest.ChangeClass.API
{
    public class MockDataToChangeClassAPI : SagaBase<GetMockNftClassData>
    {
        public bool IsFinishFetchData { get; private set; }

        public UserMaterials Data { get; private set; } = new();

        protected override void HandleAction(GetMockNftClassData ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            IsFinishFetchData = false;
            StartCoroutine(MockData());
        }

        private IEnumerator MockData()
        {
            yield return new WaitForSeconds(.5f);
            Data.newCharacter = new()
            {
                id = "100",
                unitTokenId = "",
                HP = 2000,
                addHP = 5,
                MP = 300,
                addMP = 3,
                maxHP = 2500,
                maxMP = 500,
                strength = 50,
                addStrength = 50,
                vitality = 50,
                addVitality = 50,
                agility = 50,
                addAgility = 50,
                intelligence = 50,
                addIntelligence = 50,
                luck = 50,
                addLuck = 50,
                attack = 50,
                addAttack = 50,
                deffence = 50,
                addDeffence = 50,
                evasionRate = 50,
                criticalRate = 50,
                MATK = 50,
                addMATK = 50,
                level = 50,
                exp = 50,
                partyId = 50,
                partyOrder = 50,
                inGameStatus = 50,
                isHero = 50,
                itemAddedHP = 50,
                userId = "1",
                unitId = "10424123123",
            };

            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}