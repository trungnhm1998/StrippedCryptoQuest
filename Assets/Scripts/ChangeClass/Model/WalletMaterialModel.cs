using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.Interfaces;
using UnityEngine;

namespace CryptoQuest.ChangeClass.Models
{
    [RequireComponent(typeof(WalletMaterialAPI))]
    public class WalletMaterialModel : MonoBehaviour, IWalletMaterialModel
    {
        private WalletMaterialAPI _walletMaterialAPI;
        public List<IMaterialModel> MaterialData { get; private set; }
        public bool IsLoaded { get; private set; }

        private void Awake()
        {
            _walletMaterialAPI = GetComponent<WalletMaterialAPI>();
        }

        public IEnumerator CoGetData()
        {
            MaterialData ??= new List<IMaterialModel>();
            MaterialData.Clear();
            IsLoaded = false;
            _walletMaterialAPI.LoadMaterialsFromWallet();
            yield return new WaitUntil(() => _walletMaterialAPI.IsFinishFetchData);

            if (_walletMaterialAPI.Data == null) yield break;
            StartCoroutine(ImportMaterial(_walletMaterialAPI.Data));
        }

        private IEnumerator ImportMaterial(List<MaterialAPI> materials)
        {
            yield return null;
            foreach (var material in materials)
            {
                var obj = new MaterialData(material);
                MaterialData.Add(obj);
            }
            IsLoaded = true;
        }
    }
}