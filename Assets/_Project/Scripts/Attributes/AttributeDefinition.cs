using Descending.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Attributes
{
    [CreateAssetMenu(fileName = "Attribute Definition", menuName = "Descending/Definition/Attribute Definition")]
    public class AttributeDefinition : ScriptableObject
    {
        [SerializeField] private string _name = "";
        [SerializeField] private string _key = "";
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private Color _darkColor = Color.white;

        public string Name => _name;
        public string Key => _key;
        public Color Color { get => _color; }
        public Color DarkColor { get => _darkColor; }

        //public CharacterAttribute ConvertToAttribute()
        //{
        //    return new CharacterAttribute(_type, _attribute.ToString(), (int)_attribute, 0, 0, 0, 0, 0);
        //}
    }
}