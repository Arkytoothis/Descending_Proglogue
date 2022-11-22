using System.Collections;
using System.Collections.Generic;
using Guirao.UltimateTextDamage;
using UnityEngine;

namespace Descending.Gui
{
    public class CombatTextHandler : MonoBehaviour
    {
        public static CombatTextHandler Instance { get; private set; }
        
        [SerializeField] private UltimateTextDamageManager _textManager = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple UnitManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        public void DisplayCombatText(CombatText combatText)
        {
            _textManager.Add(combatText.Text, combatText.Position, combatText.TextType);
        }
    }
}