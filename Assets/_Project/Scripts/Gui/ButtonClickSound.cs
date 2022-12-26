using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using UnityEngine.EventSystems;

namespace Descending.Gui
{
    public class ButtonClickSound : MonoBehaviour, IPointerClickHandler
    {
        [SoundGroupAttribute] public string _clickSound;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            MasterAudio.PlaySound(_clickSound);
        }
    }
}
