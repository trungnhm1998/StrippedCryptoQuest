using System.Collections;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Evolve.Interface;
using CryptoQuest.Ranch.Evolve.UI;
using CryptoQuest.Ranch.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Ranch.Evolve.Presenters
{
    public class EvolvePresenter : MonoBehaviour
    {
        [SerializeField] private BeastInventorySO _inventorySo;
        [SerializeField] private UIBeastListEvolve _beastList;
        [SerializeField] private UIBeastDetail _uiBeastDetail;
        [SerializeField] private EvolvableBeastInfoDatabaseSO _beastInfoDatabaseSo;
        [SerializeField] private UIBeastEvolveInfoDetail _uiBeastEvolveInfoDetail;
        public IBeastEvolvableInfo[] BeastEvolvableInfos { get; private set; }
        public UIBeastEvolve UIBeastEvolve { get; private set; }
        public IBeast BeastToEvolve { get; set; }
        public IBeast BeastMaterial { get; set; }
        private IBeastModel _beastModel;

        private void Awake()
        {
            _beastModel = GetComponent<IBeastModel>();
            InitInfos();
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

        private void InitInfos()
        {
            var infosDB = _beastInfoDatabaseSo.EvolableInfos;
            BeastEvolvableInfos = new IBeastEvolvableInfo[infosDB.Length];
            for (int i = 0; i < infosDB.Length; i++)
            {
                BeastEvolvableInfos[i] = new BeastEvolvableInfo()
                {
                    BeforeStars = infosDB[i].BeforeStars,
                    AfterStars = infosDB[i].AfterStars,
                    Gold = infosDB[i].Gold,
                    Metad = infosDB[i].Metad,
                    Rate = infosDB[i].Rate
                };
            }
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

        public void UpdateEvolvableInfo(IBeastEvolvableInfo info)
        {
            _uiBeastEvolveInfoDetail.SetConfirmInfo(info);
        }
    }
}