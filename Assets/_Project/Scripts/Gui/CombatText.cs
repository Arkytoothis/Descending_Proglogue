using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    [System.Serializable]
    public class CombatText
    {
        public Vector3 Position;
        public string Text;
        public string TextType;

        public CombatText(Vector3 position, string text, string textType)
        {
            Position = position;
            Text = text;
            TextType = textType;
        }
    }
}