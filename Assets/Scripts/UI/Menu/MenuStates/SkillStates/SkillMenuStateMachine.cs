﻿using CryptoQuest.UI.Menu.MenuStates.SkillStates;
using CryptoQuest.UI.Menu.Panels.Skill;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
{
    public class SkillMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavSkill = "NavSkill";
        public static readonly string Skill = "Skill";
        public static readonly string CharacterSelection = "CharacterSelection";

        private new readonly UISkillMenu _panel;
        
        /// <summary>
        /// Setup the state machine for status menu.
        /// </summary>
        /// <param name="panel"></param>
        public SkillMenuStateMachine(UISkillMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavSkill, new UnFocusSkillState(panel));
            AddState(Skill, new FocusSkillState(panel));
            AddState(CharacterSelection, new CharacterSelectionState(panel));
            // AddState(EquipmentSelection, new ChangeEquipmentState(panel));
            //
            SetStartState(CharacterSelection);
        }
    }
}