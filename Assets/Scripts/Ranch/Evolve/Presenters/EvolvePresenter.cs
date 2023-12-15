using System.Collections;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Evolve.Interface;
using CryptoQuest.Ranch.Evolve.UI;
using CryptoQuest.Ranch.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.Evolve.Presenters
{
    public class EvolvePresenter : MonoBehaviour
    {
        [SerializeField] private BeastInventorySO _inventorySo;
        [SerializeField] private EvolvableBeastInfoDatabaseSO _beastInfoDatabaseSo;
        [SerializeField] private UIBeastListEvolve _beastList;
        [SerializeField] private UIBeastEvolveDetail _uiBeastDetail;
        [SerializeField] private UIBeastEvolveDetail _uiBeastResultStats;
        [SerializeField] private UIBeastEvolveInfoDetail _uiBeastEvolveInfoDetail;
        [SerializeField] private UIBeastEvolveResultTitle _uiResultTitle;
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
            _uiBeastDetail.SetupUI(uiBeast.Beast, false);
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

        public void ShowBeastResult(LocalizedString localized, int beastId)
        {
            StartCoroutine(UpdateResultStats(beastId));
            _uiResultTitle.ShowResultTitle(localized);
        }

        private IEnumerator UpdateResultStats(int beastId)
        {
            yield return new WaitUntil(() => _uiBeastResultStats.gameObject.activeInHierarchy);
            foreach (var beast in _inventorySo.OwnedBeasts)
            {
                if (beast.Id == beastId)
                {
                    _uiBeastResultStats.SetupUI(beast, true);
                    break;
                }
            }
        }
    }
}