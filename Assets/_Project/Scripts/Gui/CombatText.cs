using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    [System.Serializable]
    public class CombatText
    {
        public Transform Transform;
        public string Text;
        public string TextType;

        public CombatText(Transform transform, string text, string textType)
        {
            Transform = transform;
            Text = text;
            TextType = textType;
        }
    }
}