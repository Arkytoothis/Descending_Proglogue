using Descending.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Equipment.Enchantments
{
    public class AttributeModifierEffect : EnchantmentEffect
    {
        [SerializeField] private string _attribute = "";
        [SerializeField] private int _modifier = 0;

        public string Attribute { get => _attribute; }
        public int Modifer { get => _modifier; }

        public override string GetTooltipText()
        {
            string text = "";

            if (_modifier < 0)
            {
                text = _modifier + " " + _attribute;
            }
            else if (_modifier >= 0)
            {
                text = "+" + _modifier + " " + _attribute;
            }

            return text;
        }
    }
}