using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.BlackSmith.Commons.UI;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
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
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _goldCurrencySO;
        [SerializeField] private CurrencySO _diamondCurrencySO;
        public IBeastEvolvableInfo[] BeastEvolvableInfos { get; private set; }
        private List<EvolvableBeast> _evolvableBeasts = new();
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
            _evolvableBeasts.Clear();
            StartCoroutine(CoInitUI());
        }

        private IEnumerator CoInitUI()
        {
            yield return _beastModel.CoGetData(_inventorySo);

            foreach (var beast in _beastModel.Beasts)
            {
                foreach (var evolvableInfo in BeastEvolvableInfos)
                {
                    if (evolvableInfo.BeforeStars == beast.Stars && beast.Level >= beast.MaxLevel)
                    {
                        var evolvableBeast = CreateEvolvableBeast(beast, evolvableInfo);
                        _evolvableBeasts.Add(evolvableBeast);
                    }
                }
            }
            InitializeBeastList();
            yield break;
        }

        private EvolvableBeast CreateEvolvableBeast(IBeast beast, IBeastEvolvableInfo evolvableInfo)
        {
            return new EvolvableBeast
            {
                Beast = beast,
                GoldCheck = CreateCurrencyValueEnough(_goldCurrencySO, evolvableInfo.Gold),
                DiamondCheck = CreateCurrencyValueEnough(_diamondCurrencySO, evolvableInfo.Metad)
            };
        }

        private CurrencyValueEnough CreateCurrencyValueEnough(CurrencySO currencySO, float value)
        {
            return new CurrencyValueEnough
            {
                Value = value,
                IsEnough = _wallet[currencySO].Amount >= value
            };
        }

        private void InitializeBeastList()
        {
            _beastList.Init(_evolvableBeasts);
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