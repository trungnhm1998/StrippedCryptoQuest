using System.Collections;
using CryptoQuest.Core;
using CryptoQuest.Sagas;
using CryptoQuest.UI.Actions;
using UnityEngine;

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
                id = 100,
                unitTokenId = 10,
                HP = Random.Range(300, 500),
                addHp = Random.Range(30, 100),
                MP = Random.Range(30, 100),
                addMp = Random.Range(30, 100),
                maxHP = Random.Range(500, 1000),
                maxMP = Random.Range(100, 300),
                strength = Random.Range(30, 100),
                addStrength = Random.Range(1, 10),
                vitality = Random.Range(30, 100),
                addVitality = Random.Range(1, 10),
                intelligence = Random.Range(30, 100),
                addIntelligence = Random.Range(30, 100),
                maxAgility = Random.Range(30, 100),
                luck = Random.Range(5, 25),
                addLuck = Random.Range(5, 8),
                attack = Random.Range(30, 100),
                addAttack = Random.Range(5, 10),
                deffence = Random.Range(30, 100),
                addDeffence = Random.Range(5, 10),
                evasionRate = Random.Range(5, 10),
                criticalRate = Random.Range(5, 10),
                MATK = Random.Range(30, 100),
                addMATK = Random.Range(5, 10),
                level = 1,
                exp = 0,
                partyId = 0,
                partyOrder = 0,
                inGameStatus = 0,
                isHero = 0,
                itemAddedHP = 0,
                userId = "1",
                unitId = "10424123123",
            };

            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}