using System.Collections;
using System.Collections.Generic;
using Guirao.UltimateTextDamage;
using UnityEngine;

namespace Descending.Gui
{
    public class CombatTextHandler : MonoBehaviour
    {
        [SerializeField] private UltimateTextDamageManager _textManager = null;

        public void OnAddText(CombatText combatText)
        {
            _textManager.Add(combatText.Text, combatText.Position, combatText.TextType);
        }
    }
}