using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MARDEK.CharacterSystem;
using UnityEngine.Serialization;

namespace MARDEK.UI
{
    using Progress;
    public class CharacterSelectable : SelectableWithCurrentSelected<CharacterSelectable>
    {
        public Character Character { 
            get
            {
                var index = transform.GetSiblingIndex();
                if (Party.Instance == null || Party.Instance.Characters.Count <= index)
                    return null;
                return Party.Instance.Characters[index];
            }
        }

        public override bool IsValid()
        {
            return Character != null;
        }
    }
}