using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.Skill
{
    [Serializable]
    public class SkillInformation
    {
        public SkillSO SkillSO;

        public SkillInformation(SkillSO skillSO)
        {
            SkillSO = skillSO;
        }
    }
}
