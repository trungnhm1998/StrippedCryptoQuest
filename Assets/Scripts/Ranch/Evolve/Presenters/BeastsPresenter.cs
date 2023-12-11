using System;
using System.Collections;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Evolve.Interface;
using CryptoQuest.Ranch.Evolve.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.Evolve.Presenters
{
    public class BeastsPresenter : MonoBehaviour
    {
        [SerializeField] private BeastInventorySO _inventorySo;
        [SerializeField] private UIBeastListEvolve _beastList;
        [SerializeField] private UIBeastDetail _uiBeastDetail;
        private IBeastModel _beastModel;

        private void Awake()
        {
            _beastModel = GetComponent<IBeastModel>();
        }

        private void OnEnable()
        {
            Init();
            _beastList.OnSelected += ShowBeastDetails;
        }

        private void OnDisable()
        {
            _beastList.OnSelected -= ShowBeastDetails;
        }

        private void ShowBeastDetails(UIBeastEvolve uiBeast)
        {
            _uiBeastDetail.SetupUI(uiBeast.Beast);
        }

        public void Init()
        {
            StartCoroutine(CoInitUI());
        }

        private IEnumerator CoInitUI()
        {
            yield return _beastModel.CoGetData(_inventorySo);
            _beastList.Init(_beastModel);
        }
    }
}