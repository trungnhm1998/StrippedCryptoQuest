using System;
using System.Collections;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Evolve.Interface;
using CryptoQuest.Ranch.Evolve.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.Evolve.Presenters
{
    public class EvolvePresenter : MonoBehaviour
    {
        [SerializeField] private BeastInventorySO _inventorySo;
        [SerializeField] private UIBeastListEvolve _beastList;
        [SerializeField] private UIBeastDetail _uiBeastDetail;
        public UIBeastEvolve UIBeastEvolve { get; private set; }
        public IBeast BeastToEvolve { get; set; }
        public IBeast BeastMaterial { get; set; }
        private IBeastModel _beastModel;

        private void Awake()
        {
            _beastModel = GetComponent<IBeastModel>();
        }

        private void OnEnable()
        {
            Init();
            _beastList.OnSelectedEvent += ShowBeastDetails;
            _beastList.IsOpenDetailsEvent += EnableBeastDetailEvent;
        }

        private void OnDisable()
        {
            _beastList.OnSelectedEvent -= ShowBeastDetails;
            _beastList.IsOpenDetailsEvent -= EnableBeastDetailEvent;
        }

        private void ShowBeastDetails(UIBeastEvolve uiBeast)
        {
            _uiBeastDetail.SetupUI(uiBeast.Beast);
            UIBeastEvolve = uiBeast;
        }

        private void EnableBeastDetailEvent(bool active)
        {
            _uiBeastDetail.gameObject.SetActive(active);
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

        public void FilterBeastMaterial(UIBeastEvolve beast)
        {
            _beastList.FilterMaterial(beast);
        }
    }
}