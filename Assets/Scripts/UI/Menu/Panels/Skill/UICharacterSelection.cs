using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UICharacterSelection : MonoBehaviour
    {
        [SerializeField] private UISkillCharacterButton _defaultSelection;

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Init()
        {
            _defaultSelection.Select();
        }

        public void DeInit()
        {
            // _defaultSelection.Select();
        }
    }
}
