using System.Collections.Generic;
using UnityEngine;

namespace MARDEK.Skill
{
     [CreateAssetMenu(menuName = "MARDEK/Skill/Skillset/Reaction Skill Set")]
     public class ReactionSkillset : Skillset<PassiveSkill>
     {
          [field: SerializeField] public override string Description { get; set; }
          [field: SerializeField]public override Sprite Sprite { get; set; }
          [field: SerializeField]public override List<PassiveSkill> Skills { get; set; }
     }
}